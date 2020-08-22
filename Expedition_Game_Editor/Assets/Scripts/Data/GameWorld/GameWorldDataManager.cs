using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameWorldDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private DataManager dataManager = new DataManager();

    private DataManager.ChapterData chapterData;
    private List<DataManager.PartyMemberData> partyMemberDataList;
    private DataManager.PhaseData phaseData;
    
    private List<DataManager.RegionData> regionDataList;
    private List<DataManager.TileSetData> tileSetDataList;
    private List<DataManager.TerrainData> terrainDataList;
    private List<DataManager.AtmosphereData> atmosphereDataList;
    private List<DataManager.TerrainTileData> terrainTileDataList;
    private List<DataManager.WorldObjectData> worldObjectDataList;
    private List<DataManager.TaskData> taskDataList;
    private List<DataManager.InteractionData> interactionDataList;
    private List<DataManager.InteractionDestinationData> interactionDestinationDataList;

    private List<DataManager.WorldInteractableData> worldInteractableDataList;
    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    private List<DataManager.ObjectiveData> objectiveDataList;
    private List<DataManager.QuestData> questDataList;

    private List<int> defaultTimeList;

    public GameWorldDataManager(GameWorldController gameWorldController)
    {
        DataController = gameWorldController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        Debug.Log("Get game data");

        var searchParameters = searchProperties.searchParameters.Cast<Search.GameWorld>().First();

        GetPhaseData(searchParameters);

        if (phaseData == null) return new List<IElementData>();

        GetChapterData();
        GetPartyMemberData();

        GetRegionData();

        GetTileSetData();
        GetTerrainData();
        GetAtmosphereData();
        GetTerrainTileData();

        GetWorldObjectData();

        GetInteractionDestinationData();
        GetInteractionData();
        GetTaskData();
        GetWorldInteractableData();
        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        GetObjectiveData();
        GetQuestData();

        var gameWorldData = new GameWorldElementData()
        {
            chapterData = new ChapterElementData()
            {
                Id = chapterData.id,
                Index = chapterData.index,

                Name = chapterData.name,

                TimeSpeed = chapterData.timeSpeed
            },

            phaseData = new PhaseElementData()
            {
                Id = phaseData.id,
                Index = phaseData.index,

                ChapterId = phaseData.chapterId,

                Name = phaseData.name,

                DefaultRegionId = phaseData.defaultRegionId,

                DefaultPositionX = phaseData.defaultPositionX,
                DefaultPositionY = phaseData.defaultPositionY,
                DefaultPositionZ = phaseData.defaultPositionZ,

                DefaultRotationX = phaseData.defaultRotationX,
                DefaultRotationY = phaseData.defaultRotationY,
                DefaultRotationZ = phaseData.defaultRotationZ,

                DefaultTime = phaseData.defaultTime
            },

            partyMemberList = (
            from partyMemberData    in partyMemberDataList
            join interactableData   in interactableDataList     on partyMemberData.interactableId   equals interactableData.id
            join objectGraphicData  in objectGraphicDataList    on interactableData.objectGraphicId equals objectGraphicData.id
            join iconData           in iconDataList             on objectGraphicData.iconId         equals iconData.id
            select new GamePartyMemberElementData()
            {
                Id = partyMemberData.id,

                objectGraphicId = objectGraphicData.id,

                objectGraphicPath = objectGraphicData.path,
                objectGraphicIconPath = iconData.path,
                
                interactableName = interactableData.name,
                
                scaleMultiplier = interactableData.scaleMultiplier,

                health = interactableData.health,
                hunger = interactableData.hunger,
                thirst = interactableData.thirst,

                weight = interactableData.weight,
                speed = interactableData.speed,
                stamina = interactableData.stamina

            }).ToList(),

            worldInteractableDataList = (
            from worldInteractableData  in worldInteractableDataList
            join interactableData       in interactableDataList     on worldInteractableData.interactableId equals interactableData.id
            join objectGraphicData      in objectGraphicDataList    on interactableData.objectGraphicId     equals objectGraphicData.id
            join iconData               in iconDataList             on objectGraphicData.iconId             equals iconData.id
            select new GameWorldInteractableElementData()
            {
                Id = worldInteractableData.id,
                
                type = worldInteractableData.type,

                terrainTileId = 0,

                objectGraphicId = objectGraphicData.id,

                objectGraphicPath = objectGraphicData.path,
                objectGraphicIconPath = iconData.path,

                interactableName = interactableData.name,

                health = interactableData.health,
                hunger = interactableData.hunger,
                thirst = interactableData.thirst,

                weight = interactableData.weight,
                speed = interactableData.speed,
                stamina = interactableData.stamina,

                scaleMultiplier = interactableData.scaleMultiplier,

                interactionDataList = (
                from interactionData    in interactionDataList
                join taskData           in taskDataList on interactionData.taskId equals taskData.id

                where taskData.worldInteractableId == worldInteractableData.id
                select new GameInteractionElementData()
                {
                    Id = interactionData.id,

                    taskId = interactionData.taskId,

                    isDefault = interactionData.isDefault,

                    startTime = interactionData.startTime,
                    endTime = interactionData.endTime,

                    triggerAutomatically = interactionData.triggerAutomatically,
                    beNearDestination = interactionData.beNearDestination,
                    faceAgent = interactionData.faceAgent,
                    facePartyLeader = interactionData.facePartyLeader,
                    hideInteractionIndicator = interactionData.hideInteractionIndicator,

                    interactionRange = interactionData.interactionRange,

                    delayMethod = interactionData.delayMethod,
                    delayDuration = interactionData.delayDuration,
                    hideDelayIndicator = interactionData.hideDelayIndicator,

                    cancelDelayOnInput = interactionData.cancelDelayOnInput,
                    cancelDelayOnMovement = interactionData.cancelDelayOnMovement,
                    cancelDelayOnHit = interactionData.cancelDelayOnHit,

                    objectiveId = taskData.objectiveId,
                    worldInteractableId = taskData.worldInteractableId,
                    
                    interactionDestinationDataList = (
                    from interactionDestinationData in interactionDestinationDataList
                    where interactionData.id == interactionDestinationData.interactionId
                    select new GameInteractionDestinationElementData()
                    {
                        Id = interactionDestinationData.id,

                        regionId = interactionDestinationData.regionId,
                        terrainId = interactionDestinationData.terrainId,
                        terrainTileId = interactionDestinationData.terrainTileId,

                        positionX = interactionDestinationData.positionX,
                        positionY = interactionDestinationData.positionY,
                        positionZ = interactionDestinationData.positionZ,

                        positionVariance = interactionDestinationData.positionVariance,

                        rotationX = interactionDestinationData.rotationX,
                        rotationY = interactionDestinationData.rotationY,
                        rotationZ = interactionDestinationData.rotationZ,

                        freeRotation = interactionDestinationData.freeRotation,

                        animation = interactionDestinationData.animation,
                        patience = interactionDestinationData.patience

                    }).ToList()
                    
                }).ToList()

            }).ToList(),

            regionDataList = (
            from regionData     in regionDataList
            join tileSetData    in tileSetDataList on regionData.tileSetId equals tileSetData.id
            select new GameRegionElementData
            {
                Id = regionData.id,
                
                phaseId = regionData.phaseId,

                type = Enums.RegionType.Game,
                regionSize = regionData.regionSize,
                terrainSize = regionData.terrainSize,

                tileSetName = tileSetData.name,
                tileSize = tileSetData.tileSize,

                terrainDataList = (
                from terrainData in terrainDataList.Where(x => x.regionId == regionData.id)
                select new GameTerrainElementData
                {
                    Id = terrainData.id,

                    name = terrainData.name,

                    gridElement = TerrainGridElement(terrainData.index, regionData.regionSize, regionData.terrainSize, tileSetData.tileSize),
                    
                    atmosphereDataList = (
                    from atmosphereData in atmosphereDataList
                    where atmosphereData.terrainId == terrainData.id
                    select new GameAtmosphereElementData()
                    {
                        Id = atmosphereData.id,
                        
                        terrainId = atmosphereData.terrainId,

                        isDefault = atmosphereData.isDefault,

                        startTime = atmosphereData.startTime,
                        endTime = atmosphereData.endTime

                    }).ToList(),

                    terrainTileDataList = (
                    from terrainTileData in terrainTileDataList
                    where terrainTileData.terrainId == terrainData.id
                    select new GameTerrainTileElementData()
                    {
                        Id = terrainTileData.id,

                        tileId = terrainTileData.tileId,

                        active = false,

                        gridElement = TileGridElement(terrainData.index, terrainTileData.index, regionData.regionSize, regionData.terrainSize, tileSetData.tileSize)
                        
                    }).OrderBy(x => x.Index).ToList(),

                    worldObjectDataList = (
                    from worldObjectData    in worldObjectDataList
                    join objectGraphicData  in objectGraphicDataList    on worldObjectData.objectGraphicId  equals objectGraphicData.id
                    join iconData           in iconDataList             on objectGraphicData.iconId         equals iconData.id
                    where worldObjectData.terrainId == terrainData.id
                    select new GameWorldObjectElementData()
                    {
                        Id = worldObjectData.id,

                        terrainTileId = worldObjectData.terrainTileId,

                        objectGraphicId = objectGraphicData.id,

                        positionX = worldObjectData.positionX,
                        positionY = worldObjectData.positionY,
                        positionZ = worldObjectData.positionZ,

                        rotationX = worldObjectData.rotationX,
                        rotationY = worldObjectData.rotationY,
                        rotationZ = worldObjectData.rotationZ,

                        scaleMultiplier = worldObjectData.scaleMultiplier,

                        animation = worldObjectData.animation,
                        
                        objectGraphicPath = objectGraphicData.path,

                        objectGraphicName = objectGraphicData.name,
                        objectGraphicIconPath = iconData.path

                    }).ToList()

                }).ToList()

            }).ToList()
        };
        
        return new List<IElementData>() { gameWorldData };
    }

    private GridElement TerrainGridElement(int index, int regionSize, int terrainSize, float tileSize)
    {
        var terrainStartPosition = TerrainStartPosition(index, regionSize, terrainSize, tileSize);

        var terrainRectPosition = new Vector2(terrainStartPosition.x - (tileSize / 2), 
                                              terrainStartPosition.y - (tileSize / 2));

        var terrainRectSize = new Vector2((terrainSize * tileSize),
                                         -(terrainSize * tileSize));

        var terrainRect = new Rect(terrainRectPosition, terrainRectSize);

        var gridElement = new GridElement(terrainStartPosition, terrainRect);

        return gridElement;
    }

    private GridElement TileGridElement(int terrainIndex, int tileIndex, int regionSize, int terrainSize, float tileSize)
    {
        var tileStartPosition = TileStartPosition(terrainIndex, tileIndex, regionSize, terrainSize, tileSize);

        var tileRectPosition = new Vector2(tileStartPosition.x - (tileSize / 2),
                                           tileStartPosition.y - (tileSize / 2));

        var tileRectSize = new Vector2(tileSize, -tileSize);

        var tileRect = new Rect(tileRectPosition, tileRectSize);

        var gridElement = new GridElement(tileStartPosition, tileRect);

        return gridElement;
    }

    private Vector2 TerrainStartPosition(int index, int regionSize, int terrainSize, float tileSize)
    {
        var startPosition = new Vector2((tileSize / 2) + ((index % regionSize) * (terrainSize * tileSize)),
                                       -(tileSize / 2) - (Mathf.Floor(index / regionSize) * (terrainSize * tileSize)));

        return startPosition;
    }

    public Vector2 TileStartPosition(int terrainIndex, int tileIndex, int regionSize, int terrainSize, float tileSize)
    {
        var terrainStartPosition = TerrainStartPosition(terrainIndex, regionSize, terrainSize, tileSize);

        var startPosition = new Vector2(terrainStartPosition.x + (tileSize * (tileIndex % terrainSize)),
                                        terrainStartPosition.y - (tileSize * (Mathf.Floor(tileIndex / terrainSize))));

        return startPosition;
    }

    internal void GetPhaseData(Search.GameWorld searchData)
    {
        var searchParameters = new Search.Phase();
        searchParameters.id = searchData.phaseId;

        phaseData = dataManager.GetPhaseData(searchParameters).First();
    }

    internal void GetChapterData()
    {
        var searchParameters = new Search.Chapter();
        searchParameters.id = new List<int>() { phaseData.chapterId };
        
        chapterData = dataManager.GetChapterData(searchParameters).First();
    }

    internal void GetPartyMemberData()
    {
        var searchParameters = new Search.PartyMember();
        searchParameters.chapterId = new List<int>() { chapterData.id };

        partyMemberDataList = dataManager.GetPartyMemberData(searchParameters);
    }

    internal void GetRegionData()
    {
        var searchParameters = new Search.Region();
        searchParameters.phaseId = new List<int>() { phaseData.id };

        regionDataList = dataManager.GetRegionData(searchParameters);
    }

    internal void GetTileSetData()
    {
        var tileSetSearchParameters = new Search.TileSet();
        tileSetSearchParameters.id = regionDataList.Select(x => x.tileSetId).Distinct().ToList();

        tileSetDataList = dataManager.GetTileSetData(tileSetSearchParameters);
    }

    internal void GetTerrainData()
    {
        var terrainSearchParameters = new Search.Terrain();
        terrainSearchParameters.regionId = regionDataList.Select(x => x.id).Distinct().ToList();

        terrainDataList = dataManager.GetTerrainData(terrainSearchParameters);
    }

    internal void GetAtmosphereData()
    {
        var atmosphereSearchParameters = new Search.Atmosphere();
        atmosphereSearchParameters.terrainId = terrainDataList.Select(x => x.id).Distinct().ToList();

        atmosphereDataList = dataManager.GetAtmosphereData(atmosphereSearchParameters);
    }

    internal void GetTerrainTileData()
    {
        var terrainTileSearchParameters = new Search.TerrainTile();
        terrainTileSearchParameters.terrainId = terrainDataList.Select(x => x.id).Distinct().ToList();

        terrainTileDataList = dataManager.GetTerrainTileData(terrainTileSearchParameters);
    }
    
    internal void GetWorldObjectData()
    {
        var worldObjectSearchParameters = new Search.WorldObject();
        worldObjectSearchParameters.regionId = regionDataList.Select(x => x.id).Distinct().ToList();

        worldObjectDataList = dataManager.GetWorldObjectData(worldObjectSearchParameters);
    }

    internal void GetInteractionDestinationData()
    {
        var interactionDestinationSearchParameters = new Search.InteractionDestination();
        interactionDestinationSearchParameters.regionId = regionDataList.Select(x => x.id).Distinct().ToList();

        interactionDestinationDataList = dataManager.GetInteractionDestinationData(interactionDestinationSearchParameters);
    }

    internal void GetInteractionData()
    {
        var interactionSearchParameters = new Search.Interaction();
        interactionSearchParameters.id = interactionDestinationDataList.Select(x => x.interactionId).Distinct().ToList();

        interactionDataList = dataManager.GetInteractionData(interactionSearchParameters);
    }

    internal void GetTaskData()
    {
        var taskSearchParameters = new Search.Task();
        taskSearchParameters.id = interactionDataList.Select(x => x.taskId).Distinct().ToList();

        taskDataList = dataManager.GetTaskData(taskSearchParameters);
    }

    internal void GetWorldInteractableData()
    {
        var worldInteractableSearchParameters = new Search.WorldInteractable();
        worldInteractableSearchParameters.id = taskDataList.Select(x => x.worldInteractableId).Distinct().ToList();

        worldInteractableDataList = dataManager.GetWorldInteractableData(worldInteractableSearchParameters);
    }

    internal void GetInteractableData()
    {
        var interactableSearchParameters = new Search.Interactable();
        interactableSearchParameters.id = worldInteractableDataList.Select(x => x.interactableId).Union(partyMemberDataList.Select(x => x.interactableId)).Distinct().ToList();

        interactableDataList = dataManager.GetInteractableData(interactableSearchParameters);
    }

    internal void GetObjectGraphicData()
    {
        var objectGraphicSearchParameters = new Search.ObjectGraphic();
        objectGraphicSearchParameters.id = interactableDataList.Select(x => x.objectGraphicId).Union(worldObjectDataList.Select(x => x.objectGraphicId)).Distinct().ToList();

        objectGraphicDataList = dataManager.GetObjectGraphicData(objectGraphicSearchParameters);
    }

    internal void GetIconData()
    {
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = objectGraphicDataList.Select(x => x.iconId).Distinct().ToList();

        iconDataList = dataManager.GetIconData(iconSearchParameters);
    }

    internal void GetObjectiveData()
    {
        var objectiveSearchParameters = new Search.Objective();
        objectiveSearchParameters.id = worldInteractableDataList.Select(x => x.objectiveId).Union(taskDataList.Select(x => x.objectiveId)).Distinct().ToList();

        objectiveDataList = dataManager.GetObjectiveData(objectiveSearchParameters);
    }

    internal void GetQuestData()
    {
        var questSearchParameters = new Search.Quest();
        questSearchParameters.id = objectiveDataList.Select(x => x.questId).Distinct().ToList();

        questDataList = dataManager.GetQuestData(questSearchParameters);
    }
}