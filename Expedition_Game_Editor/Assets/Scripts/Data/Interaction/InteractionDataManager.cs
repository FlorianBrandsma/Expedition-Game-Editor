using UnityEngine;
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

    private List<DataManager.ObjectiveData> objectiveDataList;
    private List<DataManager.QuestData> questDataList;

    private List<DataManager.RegionData> regionDataList;
    private List<DataManager.TerrainData> terrainDataList;

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

        GetObjectiveData();
        GetQuestData();

        GetRegionData();
        GetTerrainData();

        var list = (from interactionData        in interactionDataList
                    join taskData               in taskDataList                 on interactionData.taskId                   equals taskData.Id
                    join worldInteractableData  in worldInteractableDataList    on taskData.worldInteractableId             equals worldInteractableData.Id
                    join interactableData       in interactableDataList         on worldInteractableData.interactableId     equals interactableData.Id
                    join objectGraphicData      in objectGraphicDataList        on interactableData.objectGraphicId         equals objectGraphicData.Id
                    join iconData               in iconDataList                 on objectGraphicData.iconId                 equals iconData.Id

                    join leftJoin in (from objectiveData in objectiveDataList
                                      select new { objectiveData }) on worldInteractableData.objectiveId equals leftJoin.objectiveData.Id into objectiveData

                    join leftJoin in (from questData in questDataList
                                      select new { questData }) on worldInteractableData.questId equals leftJoin.questData.Id into questData

                    join leftJoin in (from regionData in regionDataList
                                      select new { regionData }) on interactionData.regionId equals leftJoin.regionData.Id into regionData

                    select new InteractionElementData()
                    {
                        Id = interactionData.Id,

                        TaskId = interactionData.taskId,
                        RegionId = interactionData.regionId,
                        TerrainId = interactionData.terrainId,
                        TerrainTileId = interactionData.terrainTileId,

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

                        PositionX = interactionData.positionX,
                        PositionY = interactionData.positionY,
                        PositionZ = interactionData.positionZ,

                        RotationX = interactionData.rotationX,
                        RotationY = interactionData.rotationY,
                        RotationZ = interactionData.rotationZ,

                        ScaleMultiplier = interactionData.scaleMultiplier,

                        Animation = interactionData.animation,

                        PublicNotes = interactionData.publicNotes,
                        PrivateNotes = interactionData.privateNotes,

                        worldInteractableId = worldInteractableData.Id,

                        objectiveId = taskData.objectiveId,
                        questId = objectiveData.FirstOrDefault() != null ? objectiveData.FirstOrDefault().objectiveData.questId :
                                  questData.FirstOrDefault() != null ? questData.FirstOrDefault().questData.Id : 0,

                        objectGraphicId = objectGraphicData.Id,
                        objectGraphicPath = objectGraphicData.path,

                        objectGraphicIconPath = iconData.path,
                        
                        height = objectGraphicData.height,
                        width = objectGraphicData.width,
                        depth = objectGraphicData.depth,

                        interactableName = interactableData.name,
                        locationName = regionData.FirstOrDefault() != null ? LocationName(regionData.First().regionData.Id, interactionData.positionX, interactionData.positionY, interactionData.positionZ) : "-",

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
            interactionData.regionId = interaction.regionId;
            interactionData.terrainId = interaction.terrainId;
            interactionData.terrainTileId = interaction.terrainTileId;

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
            
            interactionData.positionX = interaction.positionX;
            interactionData.positionY = interaction.positionY;
            interactionData.positionZ = interaction.positionZ;

            interactionData.rotationX = interaction.rotationX;
            interactionData.rotationY = interaction.rotationY;
            interactionData.rotationZ = interaction.rotationZ;

            interactionData.scaleMultiplier = interaction.scaleMultiplier;

            interactionData.animation = interaction.animation;

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

    internal void GetRegionData()
    {
        var searchParameters = new Search.Region();
        searchParameters.id = interactionDataList.Select(x => x.regionId).Distinct().ToList();

        regionDataList = dataManager.GetRegionData(searchParameters);
    }

    internal void GetTerrainData()
    {
        var searchParameters = new Search.Terrain();
        searchParameters.regionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        terrainDataList = dataManager.GetTerrainData(searchParameters);
    }

    internal string LocationName(int regionId, float positionX, float positionY, float positionZ)
    {
        var region = regionDataList.Where(x => x.Id == regionId).First();

        var terrainId = Fixtures.GetTerrain(regionId, positionX, positionZ);

        var terrain = terrainDataList.Where(x => x.Id == terrainId).FirstOrDefault();

        return region.name + ", " + terrain.name;
    }

    internal List<int> DefaultTimes(int taskId)
    {
        var dataList = interactionDataList.Where(x => x.taskId == taskId && !x.isDefault).ToList();

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

        public int regionId;
        public int terrainId;
        public int terrainTileId;

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

        public float positionX;
        public float positionY;
        public float positionZ;

        public int rotationX;
        public int rotationY;
        public int rotationZ;

        public float scaleMultiplier;

        public int animation;

        public string publicNotes;
        public string privateNotes;
    }
}
