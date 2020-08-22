using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InteractionDestinationDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<InteractionDestinationData> interactionDestinationDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.InteractionData> interactionDataList;
    private List<DataManager.TaskData> taskDataList;
    private List<DataManager.WorldInteractableData> worldInteractableDataList;
    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    private List<DataManager.ObjectiveData> objectiveDataList;
    private List<DataManager.QuestData> questDataList;

    private List<DataManager.RegionData> regionDataList;
    private List<DataManager.TerrainData> terrainDataList;
    private List<DataManager.TerrainTileData> terrainTileDataList;
    private List<DataManager.TileData> tileDataList;
    private List<DataManager.TileSetData> tileSetDataList;

    public InteractionDestinationDataManager(IDataController dataController)
    {
        DataController = dataController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.InteractionDestination>().First();

        GetInteractionDestinationData(searchParameters);

        if (interactionDestinationDataList.Count == 0) return new List<IElementData>();

        GetInteractionData();
        GetTaskData();
        GetWorldInteractableData();
        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        GetObjectiveData();
        GetQuestData();

        GetRegionData();
        GetTerrainData();
        GetTerrainTileData();
        GetTileData();
        GetTileSetData();

        var list = (from interactionDestinationData in interactionDestinationDataList
                    join interactionData            in interactionDataList          on interactionDestinationData.interactionId equals interactionData.id
                    join taskData                   in taskDataList                 on interactionData.taskId                   equals taskData.id
                    join worldInteractableData      in worldInteractableDataList    on taskData.worldInteractableId             equals worldInteractableData.id
                    join interactableData           in interactableDataList         on worldInteractableData.interactableId     equals interactableData.id
                    join objectGraphicData          in objectGraphicDataList        on interactableData.objectGraphicId         equals objectGraphicData.id
                    join iconData                   in iconDataList                 on objectGraphicData.iconId                 equals iconData.id

                    join regionData                 in regionDataList               on interactionDestinationData.regionId      equals regionData.id
                    join terrainData                in terrainDataList              on regionData.id                            equals terrainData.regionId
                    join terrainTileData            in terrainTileDataList          on terrainData.id                           equals terrainTileData.terrainId
                    join tileData                   in tileDataList                 on terrainTileData.tileId                   equals tileData.id
                    join tileSetData                in tileSetDataList              on regionData.tileSetId                     equals tileSetData.id

                    join leftJoin in (from objectiveData in objectiveDataList
                                      select new { objectiveData }) on worldInteractableData.objectiveId equals leftJoin.objectiveData.id into objectiveData

                    join leftJoin in (from questData in questDataList
                                      select new { questData }) on worldInteractableData.questId equals leftJoin.questData.id into questData

                    select new InteractionDestinationElementData()
                    {
                        Id = interactionDestinationData.id,

                        InteractionId = interactionDestinationData.interactionId,

                        RegionId = interactionDestinationData.regionId,
                        TerrainId = interactionDestinationData.terrainId,
                        TerrainTileId = interactionDestinationData.terrainTileId,

                        PositionX = interactionDestinationData.positionX,
                        PositionY = interactionDestinationData.positionY,
                        PositionZ = interactionDestinationData.positionZ,

                        PositionVariance = interactionDestinationData.positionVariance,

                        RotationX = interactionDestinationData.rotationX,
                        RotationY = interactionDestinationData.rotationY,
                        RotationZ = interactionDestinationData.rotationZ,

                        FreeRotation = interactionDestinationData.freeRotation,

                        Animation = interactionDestinationData.animation,
                        Patience = interactionDestinationData.patience,
                        
                        questId = objectiveData.FirstOrDefault()    != null ? objectiveData.FirstOrDefault().objectiveData.questId :
                                  questData.FirstOrDefault()        != null ? questData.FirstOrDefault().questData.id : 0,
                        objectiveId = taskData.objectiveId,
                        worldInteractableId = taskData.worldInteractableId,
                        taskId = interactionData.taskId,
                        
                        objectGraphicId = objectGraphicData.id,
                        objectGraphicPath = objectGraphicData.path,

                        objectGraphicIconPath = iconData.path,
                        
                        interactableName = interactableData.name,

                        height = objectGraphicData.height,
                        width = objectGraphicData.width,
                        depth = objectGraphicData.depth,

                        scaleMultiplier = interactableData.scaleMultiplier,

                        tileIconPath = tileData.iconPath,
                        tileSize = tileSetData.tileSize,

                        localPosition = RegionManager.PositionOnTile(regionData.regionSize, regionData.terrainSize, tileSetData.tileSize, interactionDestinationData.positionX, interactionDestinationData.positionZ),

                        locationName = regionData.name + ", " + terrainData.name,

                        interactableStatus = "Idle, " + interactableData.name,
                        
                        isDefault = interactionData.isDefault,

                        startTime = interactionData.startTime,
                        endTime = interactionData.endTime

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public void GetInteractionDestinationData(Search.InteractionDestination searchParameters)
    {
        interactionDestinationDataList = new List<InteractionDestinationData>();

        foreach (Fixtures.InteractionDestination interactionDestination in Fixtures.interactionDestinationList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(interactionDestination.id)) continue;
            if (searchParameters.interactionId.Count    > 0 && !searchParameters.interactionId.Contains(interactionDestination.interactionId)) continue;

            var interactionDestinationData = new InteractionDestinationData();

            interactionDestinationData.id = interactionDestination.id;

            interactionDestinationData.interactionId = interactionDestination.interactionId;

            interactionDestinationData.regionId = interactionDestination.regionId;
            interactionDestinationData.terrainId = interactionDestination.terrainId;
            interactionDestinationData.terrainTileId = interactionDestination.terrainTileId;

            interactionDestinationData.positionX = interactionDestination.positionX;
            interactionDestinationData.positionY = interactionDestination.positionY;
            interactionDestinationData.positionZ = interactionDestination.positionZ;

            interactionDestinationData.positionVariance = interactionDestination.positionVariance;

            interactionDestinationData.rotationX = interactionDestination.rotationX;
            interactionDestinationData.rotationY = interactionDestination.rotationY;
            interactionDestinationData.rotationZ = interactionDestination.rotationZ;

            interactionDestinationData.freeRotation = interactionDestination.freeRotation;

            interactionDestinationData.animation = interactionDestination.animation;
            interactionDestinationData.patience = interactionDestination.patience;

            interactionDestinationDataList.Add(interactionDestinationData);
        }
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
        searchParameters.id = interactionDestinationDataList.Select(x => x.regionId).Distinct().ToList();

        regionDataList = dataManager.GetRegionData(searchParameters);
    }

    internal void GetTerrainData()
    {
        var searchParameters = new Search.Terrain();
        searchParameters.regionId = regionDataList.Select(x => x.id).Distinct().ToList();

        terrainDataList = dataManager.GetTerrainData(searchParameters);
    }

    internal void GetTerrainTileData()
    {
        var terrainTileSearchParameters = new Search.TerrainTile();
        terrainTileSearchParameters.id = interactionDestinationDataList.Select(x => x.terrainTileId).Distinct().ToList();

        terrainTileDataList = dataManager.GetTerrainTileData(terrainTileSearchParameters);
    }

    internal void GetTileData()
    {
        var tileSearchParameters = new Search.Tile();
        tileSearchParameters.id = terrainTileDataList.Select(x => x.tileId).Distinct().ToList();

        tileDataList = dataManager.GetTileData(tileSearchParameters);
    }

    private void GetTileSetData()
    {
        var tileSetSearchParameters = new Search.TileSet();
        tileSetSearchParameters.id = regionDataList.Select(x => x.tileSetId).Distinct().ToList();

        tileSetDataList = dataManager.GetTileSetData(tileSetSearchParameters);
    }

    internal class InteractionDestinationData
    {
        public int id;

        public int interactionId;

        public int regionId;
        public int terrainId;
        public int terrainTileId;

        public float positionX;
        public float positionY;
        public float positionZ;

        public float positionVariance;

        public bool freeRotation;

        public int rotationX;
        public int rotationY;
        public int rotationZ;

        public int animation;
        public float patience;
    }
}

