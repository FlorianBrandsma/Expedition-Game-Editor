using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameWorldDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private DataManager dataManager = new DataManager();

    private DataManager.ChapterData chapterData;
    private DataManager.PhaseData phaseData;
    
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

        GetRegionData();

        GetTileSetData();
        GetTerrainData();
        GetAtmosphereData();
        GetTerrainTileData();

        GetWorldObjectData();

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
                Id = chapterData.Id,
                Index = chapterData.Index,

                Name = chapterData.name
            },

            phaseData = new PhaseElementData()
            {
                Id = phaseData.Id,
                Index = phaseData.Index,

                ChapterId = phaseData.chapterId,

                Name = phaseData.name
            },

            worldInteractableDataList = (
            from worldInteractableData  in worldInteractableDataList
            join interactableData       in interactableDataList     on worldInteractableData.interactableId equals interactableData.Id
            join objectGraphicData      in objectGraphicDataList    on interactableData.objectGraphicId     equals objectGraphicData.Id
            join iconData               in iconDataList             on objectGraphicData.iconId             equals iconData.Id
            select new WorldInteractableElementData()
            {
                Id = worldInteractableData.Id,

                Type = worldInteractableData.type,

                objectGraphicPath = objectGraphicData.path,

                interactableName = interactableData.name,
                objectGraphicIconPath = iconData.path,

                interactionDataList = (
                from interactionData    in interactionDataList
                join taskData           in taskDataList on interactionData.taskId equals taskData.Id

                where taskData.worldInteractableId == worldInteractableData.Id
                select new InteractionElementData()
                {
                    Id = interactionData.Id,
                    Index = interactionData.Index,
                    
                    TaskId = interactionData.taskId,
                    RegionId = interactionData.regionId,
                    TerrainTileId = interactionData.terrainTileId,

                    Default = interactionData.isDefault,

                    StartTime = interactionData.startTime,
                    EndTime = interactionData.endTime,

                    PositionX = interactionData.positionX,
                    PositionY = interactionData.positionY,
                    PositionZ = interactionData.positionZ,

                    RotationX = interactionData.rotationX,
                    RotationY = interactionData.rotationY,
                    RotationZ = interactionData.rotationZ,

                    ScaleMultiplier = interactionData.scaleMultiplier,

                    Animation = interactionData.animation,

                    objectiveId = taskData.objectiveId,
                    worldInteractableId = taskData.worldInteractableId,

                    height = objectGraphicData.height,
                    width = objectGraphicData.width,
                    depth = objectGraphicData.depth
                    
                }).ToList()

            }).ToList(),

            regionDataList = (
            from regionData     in regionDataList
            join tileSetData    in tileSetDataList on regionData.tileSetId equals tileSetData.Id
            select new RegionElementData
            {
                Id = regionData.Id,
                Index = regionData.Index,

                PhaseId = regionData.phaseId,

                type = Enums.RegionType.Game,
                RegionSize = regionData.regionSize,
                TerrainSize = regionData.terrainSize,

                tileSetName = tileSetData.name,
                tileSize = tileSetData.tileSize,

                terrainDataList = (
                from terrainData in terrainDataList.Where(x => x.regionId == regionData.Id)
                select new TerrainElementData
                {
                    Id = terrainData.Id,

                    Name = terrainData.name,

                    gridElement = TerrainGridElement(terrainData.Index, regionData.regionSize, regionData.terrainSize, tileSetData.tileSize),
                    
                    atmosphereDataList = (
                    from atmosphereData in atmosphereDataList
                    where atmosphereData.terrainId == terrainData.Id
                    select new AtmosphereElementData()
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
                    select new TerrainTileElementData()
                    {
                        Id = terrainTileData.Id,

                        TileId = terrainTileData.tileId,

                        active = false,

                        gridElement = TileGridElement(terrainData.Index, terrainTileData.Index, regionData.regionSize, regionData.terrainSize, tileSetData.tileSize)
                        
                    }).OrderBy(x => x.Index).ToList(),

                    worldObjectDataList = (
                    from worldObjectData    in worldObjectDataList
                    join objectGraphicData  in objectGraphicDataList    on worldObjectData.objectGraphicId  equals objectGraphicData.Id
                    join iconData           in iconDataList             on objectGraphicData.iconId         equals iconData.Id
                    where worldObjectData.terrainId == terrainData.Id
                    select new WorldObjectElementData()
                    {
                        Id = worldObjectData.Id,

                        TerrainTileId = worldObjectData.terrainTileId,

                        ObjectGraphicId = objectGraphicData.Id,

                        PositionX = worldObjectData.positionX,
                        PositionY = worldObjectData.positionY,
                        PositionZ = worldObjectData.positionZ,

                        RotationX = worldObjectData.rotationX,
                        RotationY = worldObjectData.rotationY,
                        RotationZ = worldObjectData.rotationZ,

                        ScaleMultiplier = worldObjectData.scaleMultiplier,

                        Animation = worldObjectData.animation,
                        
                        objectGraphicPath = objectGraphicData.path,

                        objectGraphicName = objectGraphicData.name,
                        objectGraphicIconPath = iconData.path,

                        height = objectGraphicData.height,
                        width = objectGraphicData.width,
                        depth = objectGraphicData.depth,

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

    internal void GetRegionData()
    {
        var searchParameters = new Search.Region();
        searchParameters.phaseId = new List<int>() { phaseData.Id };

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

    internal void GetInteractionData()
    {
        var interactionSearchParameters = new Search.Interaction();
        interactionSearchParameters.regionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        interactionDataList = dataManager.GetInteractionData(interactionSearchParameters);
    }

    internal void GetWorldObjectData()
    {
        var worldObjectSearchParameters = new Search.WorldObject();
        worldObjectSearchParameters.regionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        worldObjectDataList = dataManager.GetWorldObjectData(worldObjectSearchParameters);
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