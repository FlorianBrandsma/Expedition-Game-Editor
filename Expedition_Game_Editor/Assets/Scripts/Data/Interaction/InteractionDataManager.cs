using UnityEngine;
using System.Collections;
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

                    }).OrderByDescending(x => x.Default).ThenBy(x => x.StartTime).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetInteractionData(Search.Interaction searchParameters)
    {
        interactionDataList = new List<InteractionBaseData>();

        foreach(InteractionBaseData interaction in Fixtures.interactionList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(interaction.Id)) continue;
            if (searchParameters.taskId.Count   > 0 && !searchParameters.taskId.Contains(interaction.TaskId)) continue;

            var interactionData = new InteractionBaseData();

            interactionData.Id = interaction.Id;

            interactionData.TaskId = interaction.TaskId;

            interactionData.Default = interaction.Default;

            interactionData.StartTime = interaction.StartTime;
            interactionData.EndTime = interaction.EndTime;

            interactionData.ArrivalType = interaction.ArrivalType;

            interactionData.TriggerAutomatically = interaction.TriggerAutomatically;
            interactionData.BeNearDestination = interaction.BeNearDestination;
            interactionData.FaceInteractable = interaction.FaceInteractable;
            interactionData.FaceControllable = interaction.FaceControllable;
            interactionData.HideInteractionIndicator = interaction.HideInteractionIndicator;

            interactionData.InteractionRange = interaction.InteractionRange;

            interactionData.DelayMethod = interaction.DelayMethod;
            interactionData.DelayDuration = interaction.DelayDuration;
            interactionData.HideDelayIndicator = interaction.HideDelayIndicator;

            interactionData.CancelDelayOnInput = interaction.CancelDelayOnInput;
            interactionData.CancelDelayOnMovement = interaction.CancelDelayOnMovement;
            interactionData.CancelDelayOnHit = interaction.CancelDelayOnHit;

            interactionData.PublicNotes = interaction.PublicNotes;
            interactionData.PrivateNotes = interaction.PrivateNotes;

            interactionDataList.Add(interactionData);
        }
    }

    private static void GetTaskData()
    {
        var taskSearchParameters = new Search.Task();

        taskSearchParameters.id = interactionDataList.Select(x => x.TaskId).Distinct().ToList();

        taskDataList = DataManager.GetTaskData(taskSearchParameters);
    }

    private static void GetWorldInteractableData()
    {
        var worldInteractableSearchParameters = new Search.WorldInteractable();

        worldInteractableSearchParameters.id = taskDataList.Select(x => x.WorldInteractableId).Distinct().ToList();

        worldInteractableDataList = DataManager.GetWorldInteractableData(worldInteractableSearchParameters);
    }

    private static void GetInteractableData()
    {
        var interactableSearchParameters = new Search.Interactable();

        interactableSearchParameters.id = worldInteractableDataList.Select(x => x.InteractableId).Distinct().ToList();

        interactableDataList = DataManager.GetInteractableData(interactableSearchParameters);
    }

    private static void GetModelData()
    {
        var modelSearchParameters = new Search.Model();

        modelSearchParameters.id = interactableDataList.Select(x => x.ModelId).Distinct().ToList();

        modelDataList = DataManager.GetModelData(modelSearchParameters);
    }

    private static void GetIconData()
    {
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(iconSearchParameters);
    }

    private static void GetInteractionDestinationData()
    {
        var interactionDestinationSearchParameters = new Search.InteractionDestination();
        interactionDestinationSearchParameters.interactionId = interactionDataList.Select(x => x.Id).Distinct().ToList();

        interactionDestinationDataList = DataManager.GetInteractionDestinationData(interactionDestinationSearchParameters);
    }

    private static void GetRegionData()
    {
        var regionSearchParameters = new Search.Region();
        regionSearchParameters.id = interactionDestinationDataList.Select(x => x.RegionId).Distinct().ToList();

        regionDataList = DataManager.GetRegionData(regionSearchParameters);
    }

    private static void GetTileSetData()
    {
        var tileSetSearchParameters = new Search.TileSet();
        tileSetSearchParameters.id = regionDataList.Select(x => x.TileSetId).Distinct().ToList();

        tileSetDataList = DataManager.GetTileSetData(tileSetSearchParameters);
    }

    private static void GetTerrainData()
    {
        var terrainSearchParameters = new Search.Terrain();
        terrainSearchParameters.regionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        terrainDataList = DataManager.GetTerrainData(terrainSearchParameters);
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

    public static void UpdateData(InteractionElementData elementData)
    {
        var data = Fixtures.interactionList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedStartTime)
            data.StartTime = elementData.StartTime;

        if (elementData.ChangedEndTime)
            data.EndTime = elementData.EndTime;

        if (elementData.ChangedArrivalType)
            data.ArrivalType = elementData.ArrivalType;

        if (elementData.ChangedTriggerAutomatically)
            data.TriggerAutomatically = elementData.TriggerAutomatically;

        if (elementData.ChangedBeNearDestination)
            data.BeNearDestination = elementData.BeNearDestination;

        if (elementData.FaceInteractable)
            data.FaceInteractable = elementData.FaceInteractable;

        if (elementData.ChangedFaceControllable)
            data.FaceControllable = elementData.FaceControllable;

        if (elementData.ChangedHideInteractionIndicator)
            data.HideInteractionIndicator = elementData.HideInteractionIndicator;

        if (elementData.ChangedInteractionRange)
            data.InteractionRange = elementData.InteractionRange;

        if (elementData.ChangedDelayMethod)
            data.DelayMethod = elementData.DelayMethod;

        if (elementData.ChangedDelayDuration)
            data.DelayDuration = elementData.DelayDuration;

        if (elementData.ChangedHideDelayIndicator)
            data.HideDelayIndicator = elementData.HideDelayIndicator;

        if (elementData.ChangedCancelDelayOnInput)
            data.CancelDelayOnInput = elementData.CancelDelayOnInput;

        if (elementData.ChangedCancelDelayOnMovement)
            data.CancelDelayOnMovement = elementData.CancelDelayOnMovement;

        if (elementData.ChangedCancelDelayOnHit)
            data.CancelDelayOnHit = elementData.CancelDelayOnHit;

        if (elementData.ChangedPublicNotes)
            data.PublicNotes = elementData.PublicNotes;

        if (elementData.ChangedPrivateNotes)
            data.PrivateNotes = elementData.PrivateNotes;
    }
}
