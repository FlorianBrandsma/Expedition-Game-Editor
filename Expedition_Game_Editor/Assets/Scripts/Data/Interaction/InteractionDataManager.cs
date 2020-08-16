﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InteractionDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<InteractionData> interactionDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.TaskData> taskDataList;
    private List<DataManager.WorldInteractableData> worldInteractableDataList;
    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    private List<DataManager.InteractionDestinationData> interactionDestinationDataList;
    private List<DataManager.RegionData> regionDataList;
    private List<DataManager.TerrainData> terrainDataList;
    private List<DataManager.TileSetData> tileSetDataList;

    public InteractionDataManager(IDataController dataController)
    {
        DataController = dataController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Interaction>().First();

        GetInteractionData(searchParameters);

        if (interactionDataList.Count == 0) return new List<IElementData>();

        GetTaskData();
        GetWorldInteractableData();
        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        GetInteractionDestinationData();
        GetRegionData();
        GetTileSetData();
        GetTerrainData();

        var list = (from interactionData        in interactionDataList
                    join taskData               in taskDataList                 on interactionData.taskId                   equals taskData.Id
                    join worldInteractableData  in worldInteractableDataList    on taskData.worldInteractableId             equals worldInteractableData.Id
                    join interactableData       in interactableDataList         on worldInteractableData.interactableId     equals interactableData.Id
                    join objectGraphicData      in objectGraphicDataList        on interactableData.objectGraphicId         equals objectGraphicData.Id
                    join iconData               in iconDataList                 on objectGraphicData.iconId                 equals iconData.Id

                    join leftJoin in (from interactionDestinationData   in interactionDestinationDataList
                                      join regionData                   in regionDataList   on interactionDestinationData.regionId  equals regionData.Id
                                      join tileSetData                  in tileSetDataList  on regionData.tileSetId                 equals tileSetData.Id
                                      select new { interactionDestinationData, regionData, tileSetData }) on interactionData.Id equals leftJoin.interactionDestinationData.interactionId into interactionDestinationData

                    select new InteractionElementData()
                    {
                        Id = interactionData.Id,

                        TaskId = interactionData.taskId,
                        
                        Default = interactionData.isDefault,

                        StartTime = interactionData.startTime,
                        EndTime = interactionData.endTime,
                        
                        TriggerAutomatically = interactionData.triggerAutomatically,
                        BeNearDestination = interactionData.beNearDestination,
                        FaceAgent = interactionData.faceAgent,
                        FacePartyLeader = interactionData.facePartyLeader,
                        HideInteractionIndicator = interactionData.hideInteractionIndicator,

                        InteractionRange = interactionData.interactionRange,

                        DelayMethod = interactionData.delayMethod,
                        DelayDuration = interactionData.delayDuration,
                        HideDelayIndicator = interactionData.hideDelayIndicator,

                        CancelDelayOnInput = interactionData.cancelDelayOnInput,
                        CancelDelayOnMovement = interactionData.cancelDelayOnMovement,
                        CancelDelayOnHit = interactionData.cancelDelayOnHit,

                        PublicNotes = interactionData.publicNotes,
                        PrivateNotes = interactionData.privateNotes,

                        objectGraphicIconPath = iconData.path,

                        interactableName = interactableData.name,
                        locationName = interactionDestinationData.FirstOrDefault() != null ? RegionManager.LocationName(interactionDestinationData.FirstOrDefault().interactionDestinationData.positionX, 
                                                                                                                        interactionDestinationData.FirstOrDefault().interactionDestinationData.positionZ,
                                                                                                                        interactionDestinationData.FirstOrDefault().tileSetData.tileSize,
                                                                                                                        interactionDestinationData.FirstOrDefault().regionData, 
                                                                                                                        terrainDataList) : "-",

                        defaultTimes = interactionData.isDefault ? DefaultTimes(taskData.Id) : new List<int>(),

                    }).OrderByDescending(x => x.Default).ThenBy(x => x.StartTime).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public void GetInteractionData(Search.Interaction searchParameters)
    {
        interactionDataList = new List<InteractionData>();

        foreach(Fixtures.Interaction interaction in Fixtures.interactionList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(interaction.Id)) continue;
            if (searchParameters.taskId.Count   > 0 && !searchParameters.taskId.Contains(interaction.taskId)) continue;

            var interactionData = new InteractionData();

            interactionData.Id = interaction.Id;

            interactionData.taskId = interaction.taskId;

            interactionData.isDefault = interaction.isDefault;

            interactionData.startTime = interaction.startTime;
            interactionData.endTime = interaction.endTime;

            interactionData.triggerAutomatically = interaction.triggerAutomatically;
            interactionData.beNearDestination = interaction.beNearDestination;
            interactionData.faceAgent = interaction.faceAgent;
            interactionData.facePartyLeader = interaction.facePartyLeader;
            interactionData.hideInteractionIndicator = interaction.hideInteractionIndicator;

            interactionData.interactionRange = interaction.interactionRange;

            interactionData.delayMethod = interaction.delayMethod;
            interactionData.delayDuration = interaction.delayDuration;
            interactionData.hideDelayIndicator = interaction.hideDelayIndicator;

            interactionData.cancelDelayOnInput = interaction.cancelDelayOnInput;
            interactionData.cancelDelayOnMovement = interaction.cancelDelayOnMovement;
            interactionData.cancelDelayOnHit = interaction.cancelDelayOnHit;

            interactionData.publicNotes = interaction.publicNotes;
            interactionData.privateNotes = interaction.privateNotes;

            interactionDataList.Add(interactionData);
        }
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

        interactableSearchParameters.id = worldInteractableDataList.Select(x => x.interactableId).Distinct().ToList();

        interactableDataList = dataManager.GetInteractableData(interactableSearchParameters);
    }

    internal void GetObjectGraphicData()
    {
        var objectGraphicSearchParameters = new Search.ObjectGraphic();

        objectGraphicSearchParameters.id = interactableDataList.Select(x => x.objectGraphicId).Distinct().ToList();

        objectGraphicDataList = dataManager.GetObjectGraphicData(objectGraphicSearchParameters);
    }

    internal void GetIconData()
    {
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = objectGraphicDataList.Select(x => x.iconId).Distinct().ToList();

        iconDataList = dataManager.GetIconData(iconSearchParameters);
    }

    internal void GetInteractionDestinationData()
    {
        var interactionDestinationSearchParameters = new Search.InteractionDestination();
        interactionDestinationSearchParameters.interactionId = interactionDataList.Select(x => x.Id).Distinct().ToList();

        interactionDestinationDataList = dataManager.GetInteractionDestinationData(interactionDestinationSearchParameters);
    }

    internal void GetRegionData()
    {
        var regionSearchParameters = new Search.Region();
        regionSearchParameters.id = interactionDestinationDataList.Select(x => x.regionId).Distinct().ToList();

        regionDataList = dataManager.GetRegionData(regionSearchParameters);
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
        terrainSearchParameters.regionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        terrainDataList = dataManager.GetTerrainData(terrainSearchParameters);
    }

    internal List<int> DefaultTimes(int taskId)
    {
        var timeFrameList = (from interactionData in interactionDataList.Where(x => !x.isDefault)
                             select new TimeManager.TimeFrame()
                             {
                                 StartTime = interactionData.startTime,
                                 EndTime = interactionData.endTime

                             }).ToList();

        var defaultTimes = TimeManager.AvailableTimes(timeFrameList);

        return defaultTimes;
    }

    internal class InteractionData : GeneralData
    {
        public int taskId;
        
        public bool isDefault;

        public int startTime;
        public int endTime;

        public bool triggerAutomatically;
        public bool beNearDestination;
        public bool faceAgent;
        public bool facePartyLeader;
        public bool hideInteractionIndicator;

        public float interactionRange;

        public int delayMethod;
        public int delayDuration;
        public bool hideDelayIndicator;

        public bool cancelDelayOnInput;
        public bool cancelDelayOnMovement;
        public bool cancelDelayOnHit;

        public string publicNotes;
        public string privateNotes;
    }
}
