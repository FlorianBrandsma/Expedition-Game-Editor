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

    public static List<IElementData> GetData(Search.Interaction searchParameters)
    {
        GetInteractionData(searchParameters);

        if (searchParameters.includeAddElement)
            interactionDataList.Add(DefaultData(searchParameters.taskId.First(), false));

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

                        EditorNotes = interactionData.EditorNotes,
                        GameNotes = interactionData.GameNotes,

                        ModelIconPath = iconData.Path,

                        InteractableName = interactableData.Name,
                        LocationName = interactionDestinationData.FirstOrDefault() != null ? RegionManager.LocationName(interactionDestinationData.FirstOrDefault().interactionDestinationData.PositionX, 
                                                                                                                        interactionDestinationData.FirstOrDefault().interactionDestinationData.PositionZ,
                                                                                                                        interactionDestinationData.FirstOrDefault().tileSetData.TileSize,
                                                                                                                        interactionDestinationData.FirstOrDefault().regionData, 
                                                                                                                        terrainDataList) : "-",

                        DefaultTimes = interactionData.Default ? DefaultTimes() : new List<int>(),

                    }).OrderByDescending(x => x.Id == -1).ThenBy(x => !x.Default).ThenBy(x => x.StartTime).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static InteractionElementData DefaultData(int taskId, bool isDefault)
    {
        return new InteractionElementData()
        {
            Id = -1,

            TaskId = taskId,

            Default = isDefault,

            EndTime = (TimeManager.secondsInHour - 1)
        };
    }

    public static void SetDefaultAddValues(List<InteractionElementData> list)
    {
        var addElementData = list.Where(x => x.Id == -1).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;
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

            elementData.SetOriginalValues();

            AddDependencies(elementData, dataRequest);

        } else { }
    }

    private static void AddDependencies(InteractionElementData elementData, DataRequest dataRequest)
    {
        AddInteractionSaveData(elementData, dataRequest);

        if (!dataRequest.includeDependencies) return;

        AddInteractionDestinationData(elementData, dataRequest);
        AddOutcomeData(elementData, dataRequest);
    }

    private static void AddInteractionSaveData(InteractionElementData elementData, DataRequest dataRequest)
    {
        //Save
        var saveSearchParameters = new Search.Save();

        var saveDataList = DataManager.GetSaveData(saveSearchParameters);

        if (saveDataList.Count == 0) return;

        saveDataList.ForEach(saveData =>
        {
            var interactionSaveElementData = InteractionSaveDataManager.DefaultData(saveData.Id, elementData.Id);
            interactionSaveElementData.Add(dataRequest);
        });
    }

    private static void AddInteractionDestinationData(InteractionElementData elementData, DataRequest dataRequest)
    {
        var interactionDestinationElementData = InteractionDestinationDataManager.DefaultData(elementData.Id);
        interactionDestinationElementData.Add(dataRequest);
    }

    private static void AddOutcomeData(InteractionElementData elementData, DataRequest dataRequest)
    {
        var outcomeElementData = OutcomeDataManager.DefaultData(elementData.Id);
        outcomeElementData.Add(dataRequest);
    }

    public static void UpdateData(InteractionElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.interactionList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedStartTime)
            {
                data.StartTime = elementData.StartTime;
            }

            if (elementData.ChangedEndTime)
            {
                data.EndTime = elementData.EndTime;
            }

            if (elementData.ChangedArrivalType)
            {
                data.ArrivalType = elementData.ArrivalType;
            }

            if (elementData.ChangedTriggerAutomatically)
            {
                data.TriggerAutomatically = elementData.TriggerAutomatically;
            }

            if (elementData.ChangedBeNearDestination)
            {
                data.BeNearDestination = elementData.BeNearDestination;
            }

            if (elementData.FaceInteractable)
            {
                data.FaceInteractable = elementData.FaceInteractable;
            }

            if (elementData.ChangedFaceControllable)
            {
                data.FaceControllable = elementData.FaceControllable;
            }

            if (elementData.ChangedHideInteractionIndicator)
            {
                data.HideInteractionIndicator = elementData.HideInteractionIndicator;
            }

            if (elementData.ChangedInteractionRange)
            {
                data.InteractionRange = elementData.InteractionRange;
            }

            if (elementData.ChangedDelayMethod)
            {
                data.DelayMethod = elementData.DelayMethod;
            }

            if (elementData.ChangedDelayDuration)
            {
                data.DelayDuration = elementData.DelayDuration;
            }

            if (elementData.ChangedHideDelayIndicator)
            {
                data.HideDelayIndicator = elementData.HideDelayIndicator;
            }

            if (elementData.ChangedCancelDelayOnInput)
            {
                data.CancelDelayOnInput = elementData.CancelDelayOnInput;
            }

            if (elementData.ChangedCancelDelayOnMovement)
            {
                data.CancelDelayOnMovement = elementData.CancelDelayOnMovement;
            }

            if (elementData.ChangedCancelDelayOnHit)
            {
                data.CancelDelayOnHit = elementData.CancelDelayOnHit;
            }

            if (elementData.ChangedEditorNotes)
            {
                data.EditorNotes = elementData.EditorNotes;
            }

            if (elementData.ChangedGameNotes)
            {
                data.GameNotes = elementData.GameNotes;
            }

            elementData.SetOriginalValues();

        } else { }
    }

    public static void RemoveData(InteractionElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            RemoveDependencies(elementData, dataRequest);

            Fixtures.interactionList.RemoveAll(x => x.Id == elementData.Id);
            
        } else {

            //Anything beyond this can be removed without validation
            //RemoveDependencies(elementData, dataRequest);
        }
    }

    private static void RemoveDependencies(InteractionElementData elementData, DataRequest dataRequest)
    {
        RemoveInteractionDestinationData(elementData, dataRequest);

        RemoveOutcomeData(elementData, dataRequest);
        RemoveInteractionSaveData(elementData, dataRequest);
    }

    private static void RemoveInteractionDestinationData(InteractionElementData elementData, DataRequest dataRequest)
    {
        var interactionDestinationSearchParameters = new Search.InteractionDestination()
        {
            interactionId = new List<int>() { elementData.Id }
        };

        var interactionDestinationDataList = DataManager.GetInteractionDestinationData(interactionDestinationSearchParameters);

        interactionDestinationDataList.ForEach(interactionDestinationData =>
        {
            var interactionDestinationElementData = new InteractionDestinationElementData()
            {
                Id = interactionDestinationData.Id
            };

            interactionDestinationElementData.Remove(dataRequest);
        });
    }

    private static void RemoveOutcomeData(InteractionElementData elementData, DataRequest dataRequest)
    {
        var outcomeSearchParameters = new Search.Outcome()
        {
            interactionId = new List<int>() { elementData.Id }
        };

        var outcomeDataList = DataManager.GetOutcomeData(outcomeSearchParameters);

        outcomeDataList.ForEach(outcomeData =>
        {
            var outcomeElementData = new OutcomeElementData()
            {
                Id = outcomeData.Id
            };

            outcomeElementData.Remove(dataRequest);
        });
    }

    private static void RemoveInteractionSaveData(InteractionElementData elementData, DataRequest dataRequest)
    {
        var interactionSaveSearchParameters = new Search.InteractionSave()
        {
            interactionId = new List<int>() { elementData.Id }
        };

        var interactionSaveDataList = DataManager.GetInteractionSaveData(interactionSaveSearchParameters);

        interactionSaveDataList.ForEach(interactionSaveData =>
        {
            var interactionSaveElementData = new InteractionSaveElementData()
            {
                Id = interactionSaveData.Id
            };

            interactionSaveElementData.Remove(dataRequest);
        });
    }
}
