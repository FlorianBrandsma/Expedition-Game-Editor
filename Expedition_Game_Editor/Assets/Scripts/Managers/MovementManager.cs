using UnityEngine;
using System.Collections.Generic;
using System.Linq;

static public class MovementManager
{
    static public List<GameWorldInteractableElementData> movableWorldInteractableList;
    
    static public void SetDestination(GameWorldInteractableElementData worldInteractableElementData)
    {
        var interactionData = worldInteractableElementData.ActiveInteraction;
        
        //When the world interactable is at its final destination...
        if(interactionData.ActiveDestinationIndex == interactionData.InteractionDestinationDataList.Count - 1)
        {
            //Stay at the destination, but possibly apply position variance by re-assigning the active destination index
            if (interactionData.ArrivalType == Enums.ArrivalType.Stay)
            {
                worldInteractableElementData.ActiveInteraction.ActiveDestinationIndex = interactionData.ActiveDestinationIndex;
                return;
            }
            
            if (interactionData.ArrivalType == Enums.ArrivalType.Backtrace)
            {
                worldInteractableElementData.Backtracing = true;
            }
            
            if (interactionData.ArrivalType == Enums.ArrivalType.Repeat)
            {
                worldInteractableElementData.ActiveInteraction.ActiveDestinationIndex = 0;
                return;
            }
        }

        if(interactionData.ActiveDestinationIndex == 0 && worldInteractableElementData.Backtracing)
        {
            worldInteractableElementData.Backtracing = false;
        }

        if (!worldInteractableElementData.Backtracing)
        {
            worldInteractableElementData.ActiveInteraction.ActiveDestinationIndex++;
        } else {
            worldInteractableElementData.ActiveInteraction.ActiveDestinationIndex--;
        }
    }

    static public void SetDestinationPosition(GameInteractionDestinationElementData destinationElementData)
    {
        //NOTE! Variance can be limited to only small ranges, removing the need to find the terrain tile if it proves to be too much to process

        var position = new Vector3( destinationElementData.PositionX + Random.Range(-destinationElementData.PositionVariance, destinationElementData.PositionVariance),
                                    destinationElementData.PositionY,
                                    destinationElementData.PositionZ + Random.Range(-destinationElementData.PositionVariance, destinationElementData.PositionVariance));

        if(destinationElementData.PositionVariance > 0)
        {
            var regionData = GameManager.instance.gameWorldData.RegionDataList.Where(x => x.Id == destinationElementData.RegionId).First();

            //Corrects position if it's out of the region bounds
            if (position.x < 0)                 position = new Vector3(0, position.y, position.z);
            if (position.z < 0)                 position = new Vector3(position.x, position.y, 0);
            if (position.x > regionData.Size)   position = new Vector3(regionData.Size, position.y, position.z);
            if (position.z > regionData.Size)   position = new Vector3(position.x, position.y, regionData.Size);

            var terrainTileId = RegionManager.GetGameTerrainTileId(regionData, position.x, position.z);

            destinationElementData.TerrainTileId = terrainTileId;
        }

        destinationElementData.Position = position;
    }

    static public void StartTravel(GameWorldInteractableElementData worldInteractableElementData)
    {
        var distance = Vector3.Distance(worldInteractableElementData.Position, worldInteractableElementData.ActiveInteraction.ActiveDestination.Position);

        //At the moment, 0.5f is approximately 1m
        var tempDistanceInMeters = (distance / 2f);

        var ms = (worldInteractableElementData.Speed / 3.6f);

        worldInteractableElementData.TravelTime = (tempDistanceInMeters / ms);
        worldInteractableElementData.AgentState = AgentState.Move;
    }

    static public void Arrive(GameWorldInteractableElementData worldInteractableElementData)
    {
        var destinationData = worldInteractableElementData.ActiveInteraction.ActiveDestination;

        worldInteractableElementData.Position = new Vector3(destinationData.Position.x, destinationData.Position.y, destinationData.Position.z);

        worldInteractableElementData.AgentState = AgentState.Idle;
    }
}
