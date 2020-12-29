using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class InteractionDataManager
{
    private static List<InteractionBaseData> interactionDataList;

    private static List<TaskBaseData> taskDataList;
    private static List<WorldInteractableBaseData> worldInteractableDataList;
    private static List<InteractableBaseData> interactableDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    private static List<InteractionDestinationBaseData> interactionDestinationDataList;
    private static List<RegionBaseData> regionDataList;
    private static List<TerrainBaseData> terrainDataList;
    private static List<TileSetBaseData> tileSetDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Interaction>().First();

        GetInteractionData(searchParameters);

        if (interactionDataList.Count == 0) return new List<IElementData>();
        
        GetTaskData();
        GetWorldInteractableData();
        GetInteractableData();
        GetModelData();
        GetIconData();

        GetInteractionDestinationData();
        GetRegionData();
        GetTileSetData();
        GetTerrainData();

        var list = (from interactionData        in interactionDataList
                    join taskData               in taskDataList                 on interactionData.TaskId                   equals taskData.Id
                    join worldInteractableData  in worldInteractableDataList    on taskData.WorldInteractableId             equals worldInteractableData.Id
                    join interactableData       in interactableDataList         on worldInteractableData.InteractableId     equals interactableData.Id
                    join modelData              in modelDataList                on interactableData.ModelId                 equals modelData.Id
                    join iconData               in iconDataList                 on modelData.IconId                         equals iconData.Id

                    join leftJoin in (from interactionDestinationData   in interactionDestinationDataList
                                      join regionData                   in regionDataList   on interactionDestinationData.RegionId  equals regionData.Id
                                      join tileSetData                  in tileSetDataList  on regionData.TileSetId                 equals tileSetData.Id
                                      select new { interactionDestinationData, regionData, tileSetData }) on interactionData.Id equals leftJoin.interactionDestinationData.InteractionId into interactionDestinationData

                    select new InteractionElementData()
                    {
                        Id = interactionData.Id,

                        TaskId = interactionData.TaskId,
                        
                        Default = interactionData.Default,

                        StartTime = interactionData.StartTime,
                        EndTime = interactionData.EndTime,
                        
                        ArrivalType = interactionData.ArrivalType,

                        TriggerAutomatically = interactionData.TriggerAutomatically,
                        BeNearDestination = interactionData.BeNearDestination,
                        FaceInteractable = interactionData.FaceInteractable,
                        FaceControllable = interactionData.FaceControllable,
                        HideInteractionIndicator = interactionData.HideInteractionIndicator,

                        InteractionRange = interactionData.InteractionRange,

                        DelayMethod = interactionData.DelayMethod,
                        DelayDuration = interactionData.DelayDuration,
                        HideDelayIndicator = interactionData.HideDelayIndicator,

                        CancelDelayOnInput = interactionData.CancelDelayOnInput,
                        CancelDelayOnMovement = interactionData.CancelDelayOnMovement,
                        CancelDelayOnHit = interactionData.CancelDelayOnHit,

                        PublicNotes = interactionData.PublicNotes,
                        PrivateNotes = interactionData.PrivateNotes,

                        ModelIconPath = iconData.Path,

                        InteractableName = interactableData.Name,
                        LocationName = interactionDestinationData.FirstOrDefault() != null ? RegionManager.LocationName(interactionDestinationData.FirstOrDefault().interactionDestinationData.PositionX, 
                                                                                                                        interactionDestinationData.FirstOrDefault().interactionDestinationData.PositionZ,
                                                                                                                        interactionDestinationData.FirstOrDefault().tileSetData.TileSize,
                                                                                                                        interactionDestinationData.FirstOrDefault().regionData, 
                                                                                                                        terrainDataList) : "-",

                        DefaultTimes = interactionData.Default ? DefaultTimes() : new List<int>(),

                    }).OrderByDescending(x => x.Id == 0).ThenBy(x => !x.Default).ThenBy(x => x.StartTime).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetInteractionData(Search.Interaction searchParameters)
    {
        interactionDataList = new List<InteractionBaseData>();

        foreach(InteractionBaseData interaction in Fixtures.interactionList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(interaction.Id))           continue;
            if (searchParameters.taskId.Count   > 0 && !searchParameters.taskId.Contains(interaction.TaskId))   continue;

            interactionDataList.Add(interaction);
        }
    }

    private static void GetTaskData()
    {
        var searchParameters = new Search.Task();
        searchParameters.id = interactionDataList.Select(x => x.TaskId).Distinct().ToList();

        taskDataList = DataManager.GetTaskData(searchParameters);
    }

    private static void GetWorldInteractableData()
    {
        var searchParameters = new Search.WorldInteractable();
        searchParameters.id = taskDataList.Select(x => x.WorldInteractableId).Distinct().ToList();

        worldInteractableDataList = DataManager.GetWorldInteractableData(searchParameters);
    }

    private static void GetInteractableData()
    {
        var searchParameters = new Search.Interactable();
        searchParameters.id = worldInteractableDataList.Select(x => x.InteractableId).Distinct().ToList();

        interactableDataList = DataManager.GetInteractableData(searchParameters);
    }

    private static void GetModelData()
    {
        var searchParameters = new Search.Model();
        searchParameters.id = interactableDataList.Select(x => x.ModelId).Distinct().ToList();

        modelDataList = DataManager.GetModelData(searchParameters);
    }

    private static void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(searchParameters);
    }

    private static void GetInteractionDestinationData()
    {
        var searchParameters = new Search.InteractionDestination();
        searchParameters.interactionId = interactionDataList.Select(x => x.Id).Distinct().ToList();

        interactionDestinationDataList = DataManager.GetInteractionDestinationData(searchParameters);
    }

    private static void GetRegionData()
    {
        var searchParameters = new Search.Region();
        searchParameters.id = interactionDestinationDataList.Select(x => x.RegionId).Distinct().ToList();

        regionDataList = DataManager.GetRegionData(searchParameters);
    }

    private static void GetTileSetData()
    {
        var searchParameters = new Search.TileSet();
        searchParameters.id = regionDataList.Select(x => x.TileSetId).Distinct().ToList();

        tileSetDataList = DataManager.GetTileSetData(searchParameters);
    }

    private static void GetTerrainData()
    {
        var searchParameters = new Search.Terrain();
        searchParameters.regionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        terrainDataList = DataManager.GetTerrainData(searchParameters);
    }

    private static List<int> DefaultTimes()
    {
        var timeFrameList = (from interactionData in interactionDataList.Where(x => !x.Default)
                             select new TimeManager.TimeFrame()
                             {
                                 StartTime = interactionData.StartTime,
                                 EndTime = interactionData.EndTime

                             }).ToList();

        var defaultTimes = TimeManager.AvailableTimes(timeFrameList);

        return defaultTimes;
    }

    public static void AddData(InteractionElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.interactionList.Count > 0 ? (Fixtures.interactionList[Fixtures.interactionList.Count - 1].Id + 1) : 1;
            Fixtures.interactionList.Add(((InteractionData)elementData).Clone());

            AddDefaultInteractionDestination(elementData, dataRequest);
            AddDefaultOutcome(elementData, dataRequest);

        } else { }
    }

    private static void AddDefaultInteractionDestination(InteractionElementData elementData, DataRequest dataRequest)
    {
        var regionElementData = new RegionElementData();

        var regionRoute = RenderManager.layoutManager.forms[0].activePath.FindLastRoute(Enums.DataType.Region);

        if (regionRoute != null)
        {
            regionElementData = (RegionElementData)regionRoute.ElementData;

        } else {

            //First region of the selected phase
        }

        var defaultPosition = new Vector3(0, 0, 0);

        var terrainSearchParameters = new Search.Terrain()
        {
            regionId = new List<int>() { regionElementData.Id }
        };

        var terrainData = DataManager.GetTerrainData(terrainSearchParameters);
        
        var terrainId = RegionManager.GetTerrainId(regionElementData, terrainData, regionElementData.TileSize, defaultPosition.x, defaultPosition.z);
        var terrainTileId = RegionManager.GetTerrainTileId(regionElementData, defaultPosition.x, defaultPosition.z);

        var interactionDestinationElementData = new InteractionDestinationElementData()
        {
            InteractionId = elementData.Id,

            RegionId = regionElementData.Id,
            TerrainId = terrainId,
            TerrainTileId = terrainTileId,
        };

        interactionDestinationElementData.Add(dataRequest);
    }

    private static void AddDefaultOutcome(InteractionElementData elementData, DataRequest dataRequest)
    {
        var outcomeElementData = new OutcomeElementData()
        {
            InteractionId = elementData.Id,

            Type = (int)Enums.OutcomeType.Positive
        };

        outcomeElementData.Add(dataRequest);
    }

    public static void UpdateData(InteractionElementData elementData, DataRequest dataRequest)
    {
        var data = Fixtures.interactionList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedStartTime)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.StartTime = elementData.StartTime;
            else { }
        }

        if (elementData.ChangedEndTime)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.EndTime = elementData.EndTime;
            else { }
        }

        if (elementData.ChangedArrivalType)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.ArrivalType = elementData.ArrivalType;
            else { }
        }

        if (elementData.ChangedTriggerAutomatically)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.TriggerAutomatically = elementData.TriggerAutomatically;
            else { }
        }

        if (elementData.ChangedBeNearDestination)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.BeNearDestination = elementData.BeNearDestination;
            else { }
        }

        if (elementData.FaceInteractable)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.FaceInteractable = elementData.FaceInteractable;
            else { }
        }

        if (elementData.ChangedFaceControllable)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.FaceControllable = elementData.FaceControllable;
            else { }
        }

        if (elementData.ChangedHideInteractionIndicator)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.HideInteractionIndicator = elementData.HideInteractionIndicator;
            else { }
        }

        if (elementData.ChangedInteractionRange)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.InteractionRange = elementData.InteractionRange;
            else { }
        }

        if (elementData.ChangedDelayMethod)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.DelayMethod = elementData.DelayMethod;
            else { }
        }

        if (elementData.ChangedDelayDuration)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.DelayDuration = elementData.DelayDuration;
            else { }
        }

        if (elementData.ChangedHideDelayIndicator)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.HideDelayIndicator = elementData.HideDelayIndicator;
            else { }
        }

        if (elementData.ChangedCancelDelayOnInput)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.CancelDelayOnInput = elementData.CancelDelayOnInput;
            else { }
        }

        if (elementData.ChangedCancelDelayOnMovement)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.CancelDelayOnMovement = elementData.CancelDelayOnMovement;
            else { }
        }

        if (elementData.ChangedCancelDelayOnHit)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.CancelDelayOnHit = elementData.CancelDelayOnHit;
            else { }
        }

        if (elementData.ChangedPublicNotes)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PublicNotes = elementData.PublicNotes;
            else { }
        }

        if (elementData.ChangedPrivateNotes)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PrivateNotes = elementData.PrivateNotes;
            else { }
        }
    }
}
