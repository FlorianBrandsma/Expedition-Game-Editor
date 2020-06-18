using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class EditorWorldDataManager : IDataManager
{
    public IDataController DataController { get; set; }
    
    private Enums.RegionType regionType;

    private DataManager dataManager = new DataManager();

    private List<DataManager.RegionData> regionDataList;
    private List<DataManager.TileSetData> tileSetDataList;
    private List<DataManager.TerrainData> terrainDataList;
    private List<DataManager.AtmosphereData> atmosphereDataList;
    private List<DataManager.TerrainTileData> terrainTileDataList;
    private List<DataManager.WorldObjectData> worldObjectDataList;
    private List<DataManager.TaskData> taskDataList;
    private List<DataManager.InteractionData> interactionDataList;
    
    private List<DataManager.WorldInteractableData> worldInteractableDataList;
    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    private List<DataManager.ObjectiveData> objectiveDataList;
    private List<DataManager.QuestData> questDataList;

    public EditorWorldDataManager(EditorWorldController editorWorldController)
    {
        DataController = editorWorldController;
    }

    public List<IDataElement> GetDataElements(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.EditorWorld>().First();

        regionType = searchParameters.regionType;
        
        GetRegionData(searchParameters);

        if (regionDataList.Count == 0) return new List<IDataElement>();

        GetTileSetData();
        GetTerrainData();
        GetAtmosphereData();
        GetTerrainTileData();

        GetWorldObjectData(searchParameters);

        GetInteractionData(searchParameters);
        GetTaskData(searchParameters);
        GetWorldInteractableData();
        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        GetObjectiveData();
        GetQuestData();

        var list = (
            from regionData in regionDataList
            join tileSetData in tileSetDataList on regionData.tileSetId equals tileSetData.Id
            select new EditorWorldDataElement
            {
                Id = regionData.Id,
                Index = regionData.Index,

                regionType = regionType,
                regionSize = regionData.regionSize,
                terrainSize = regionData.terrainSize,

                tileSetName = tileSetData.name,
                tileSize = tileSetData.tileSize,

                terrainDataList = (
                from terrainData in terrainDataList
                select new TerrainDataElement()
                {
                    Id = terrainData.Id,
                    Index = terrainData.Index,

                    Name = terrainData.name,

                    gridElement = TerrainGridElement(terrainData.Index, regionData.regionSize, regionData.terrainSize, tileSetData.tileSize),

                    atmosphereDataList = (
                    from atmosphereData in atmosphereDataList
                    where atmosphereData.terrainId == terrainData.Id
                    select new AtmosphereDataElement()
                    {
                        Id = atmosphereData.Id,

                        TerrainId = atmosphereData.terrainId,

                        Default = atmosphereData.isDefault,

                        StartTime = atmosphereData.startTime,
                        EndTime = atmosphereData.endTime

                    }).ToList(),

                    terrainTileDataList = (
                    from terrainTileData in terrainTileDataList
                    where terrainTileData.terrainId == terrainData.Id
                    select new TerrainTileDataElement()
                    {
                        Id = terrainTileData.Id,
                        Index = terrainTileData.Index,

                        TileId = terrainTileData.tileId,

                        active = false,

                        gridElement = TileGridElement(terrainData.Index, terrainTileData.Index, regionData.regionSize, regionData.terrainSize, tileSetData.tileSize)

                    }).OrderBy(x => x.Index).ToList(),

                    worldInteractableDataList = regionType != Enums.RegionType.Interaction ? (
                    from worldInteractableData  in worldInteractableDataList
                    join taskData               in taskDataList             on worldInteractableData.Id                 equals taskData.worldInteractableId
                    join interactionData        in interactionDataList      on taskData.Id                              equals interactionData.taskId
                    join interactableData       in interactableDataList     on worldInteractableData.interactableId     equals interactableData.Id
                    join objectGraphicData      in objectGraphicDataList    on interactableData.objectGraphicId         equals objectGraphicData.Id
                    join iconData               in iconDataList             on objectGraphicData.iconId                 equals iconData.Id

                    where interactionData.terrainId == terrainData.Id
                    select new WorldInteractableDataElement()
                    {
                        Id = worldInteractableData.Id,

                        Type = worldInteractableData.type,

                        PhaseId = worldInteractableData.phaseId,
                        QuestId = worldInteractableData.questId,
                        ObjectiveId = worldInteractableData.objectiveId,

                        ChapterInteractableId = worldInteractableData.chapterInteractableId,
                        InteractableId = interactableData.Id,

                        terrainTileId = interactionData.terrainTileId,

                        isDefault = interactionData.isDefault,
                        taskGroup = taskData.Id,

                        interactableName = interactableData.name,

                        positionX = interactionData.positionX,
                        positionY = interactionData.positionY,
                        positionZ = interactionData.positionZ,

                        rotationX = interactionData.rotationX,
                        rotationY = interactionData.rotationY,
                        rotationZ = interactionData.rotationZ,

                        height = objectGraphicData.height,
                        width = objectGraphicData.width,
                        depth = objectGraphicData.depth,

                        scaleMultiplier = interactionData.scaleMultiplier,

                        animation = interactionData.animation,

                        objectGraphicId = objectGraphicData.Id,
                        objectGraphicPath = objectGraphicData.path,

                        objectGraphicIconPath = iconData.path,

                        startTime = interactionData.startTime,
                        endTime = interactionData.endTime,

                    }).ToList() : new List<WorldInteractableDataElement>(),

                    interactionDataList = regionType == Enums.RegionType.Interaction ? (
                    from interactionData        in interactionDataList
                    join taskData               in taskDataList                 on interactionData.taskId                   equals taskData.Id
                    join worldInteractableData  in worldInteractableDataList    on taskData.worldInteractableId             equals worldInteractableData.Id
                    join interactableData       in interactableDataList         on worldInteractableData.interactableId     equals interactableData.Id
                    join objectGraphicData      in objectGraphicDataList        on interactableData.objectGraphicId         equals objectGraphicData.Id
                    join iconData               in iconDataList                 on objectGraphicData.iconId                 equals iconData.Id

                    join leftJoin in (from objectiveData in objectiveDataList
                                      select new { objectiveData }) on worldInteractableData.objectiveId equals leftJoin.objectiveData.Id into objectiveData

                    join leftJoin in (from questData in questDataList
                                      select new { questData }) on worldInteractableData.questId equals leftJoin.questData.Id into questData

                    where interactionData.terrainId == terrainData.Id
                    select new InteractionDataElement()
                    {
                        Id = interactionData.Id,
                        Index = interactionData.Index,

                        TaskId = interactionData.taskId,
                        TerrainId = interactionData.terrainId,
                        TerrainTileId = interactionData.terrainTileId,

                        Default = interactionData.isDefault,

                        StartTime = interactionData.startTime,
                        EndTime = interactionData.endTime,

                        PublicNotes = interactionData.publicNotes,
                        PrivateNotes = interactionData.privateNotes,

                        PositionX = interactionData.positionX,
                        PositionY = interactionData.positionY,
                        PositionZ = interactionData.positionZ,

                        RotationX = interactionData.rotationX,
                        RotationY = interactionData.rotationY,
                        RotationZ = interactionData.rotationZ,

                        ScaleMultiplier = interactionData.scaleMultiplier,

                        Animation = interactionData.animation,
                        
                        worldInteractableId = taskData.worldInteractableId,

                        objectiveId = taskData.objectiveId,
                        questId = objectiveData.FirstOrDefault()    != null ? objectiveData.FirstOrDefault().objectiveData.questId : 
                                  questData.FirstOrDefault()        != null ? questData.FirstOrDefault().questData.Id : 0,

                        objectGraphicId = objectGraphicData.Id,
                        objectGraphicPath = objectGraphicData.path,

                        objectGraphicIconPath = iconData.path,

                        height = objectGraphicData.height,
                        width = objectGraphicData.width,
                        depth = objectGraphicData.depth,

                        defaultTimes = interactionData.isDefault ? DefaultTimes(taskData.Id) : new List<int>()

                    }).OrderBy(x => x.Index).ToList() : new List<InteractionDataElement>(),
                    
                    worldObjectDataList = (
                    from worldObjectData    in worldObjectDataList
                    join objectGraphicData  in objectGraphicDataList    on worldObjectData.objectGraphicId  equals objectGraphicData.Id
                    join iconData           in iconDataList             on objectGraphicData.iconId         equals iconData.Id
                    where worldObjectData.terrainId == terrainData.Id
                    select new WorldObjectDataElement()
                    {
                        Id = worldObjectData.Id,
                        TerrainId = worldObjectData.terrainId,
                        TerrainTileId = worldObjectData.terrainTileId,

                        PositionX = worldObjectData.positionX,
                        PositionY = worldObjectData.positionY,
                        PositionZ = worldObjectData.positionZ,

                        RotationX = worldObjectData.rotationX,
                        RotationY = worldObjectData.rotationY,
                        RotationZ = worldObjectData.rotationZ,

                        ScaleMultiplier = worldObjectData.scaleMultiplier,

                        Animation = worldObjectData.animation,

                        ObjectGraphicId = objectGraphicData.Id,
                        objectGraphicPath = objectGraphicData.path,

                        objectGraphicName = objectGraphicData.name,
                        objectGraphicIconPath = iconData.path,

                        height = objectGraphicData.height,
                        width = objectGraphicData.width,
                        depth = objectGraphicData.depth

                    }).ToList()

                }).OrderBy(x => x.Index).ToList()

            }).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
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

    internal void GetRegionData(Search.EditorWorld searchData)
    {
        var searchParameters = new Search.Region();
        searchParameters.id = searchData.regionId;

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
        terrainSearchParameters.regionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        terrainDataList = dataManager.GetTerrainData(terrainSearchParameters);
    }

    internal void GetAtmosphereData()
    {
        var atmosphereSearchParameters = new Search.Atmosphere();
        atmosphereSearchParameters.terrainId = terrainDataList.Select(x => x.Id).Distinct().ToList();

        atmosphereDataList = dataManager.GetAtmosphereData(atmosphereSearchParameters);
    }

    internal void GetTerrainTileData()
    {
        var terrainTileSearchParameters = new Search.TerrainTile();
        terrainTileSearchParameters.terrainId = terrainDataList.Select(x => x.Id).Distinct().ToList();

        terrainTileDataList = dataManager.GetTerrainTileData(terrainTileSearchParameters);
    }

    internal void GetInteractionData(Search.EditorWorld searchData)
    {
        var interactionSearchParameters = new Search.Interaction();
        interactionSearchParameters.regionId = searchData.regionId;
        
        interactionDataList = dataManager.GetInteractionData(interactionSearchParameters);
    }

    internal void GetWorldObjectData(Search.EditorWorld searchData)
    {
        var worldObjectSearchParameters = new Search.WorldObject();
        worldObjectSearchParameters.regionId = searchData.id;

        worldObjectDataList = dataManager.GetWorldObjectData(worldObjectSearchParameters);
    }

    internal void GetTaskData(Search.EditorWorld searchData)
    {
        var taskSearchParameters = new Search.Task();
        taskSearchParameters.id = interactionDataList.Select(x => x.taskId).Distinct().ToList();
        taskSearchParameters.objectiveId = searchData.objectiveId;

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
        objectGraphicSearchParameters.id = interactableDataList.Select(x => x.objectGraphicId).Distinct().ToList().Union(worldObjectDataList.Select(x => x.objectGraphicId).Distinct().ToList()).Distinct().ToList();

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
}