using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class EditorWorldDataManager
{
    private static Enums.RegionType regionType;

    private static List<RegionBaseData> regionDataList;
    private static List<TileSetBaseData> tileSetDataList;
    private static List<TerrainBaseData> terrainDataList;
    private static List<AtmosphereBaseData> atmosphereDataList;
    private static List<TerrainTileBaseData> terrainTileDataList;
    private static List<WorldObjectBaseData> worldObjectDataList;
    private static List<InteractionDestinationBaseData> interactionDestinationDataList;
    private static List<InteractionBaseData> interactionDataList;
    private static List<TaskBaseData> taskDataList;

    private static List<PhaseBaseData> phaseDataList;
    private static List<ChapterBaseData> chapterDataList;

    private static List<WorldInteractableBaseData> chapterWorldInteractableDataList;

    private static List<WorldInteractableBaseData> worldInteractableDataList;
    private static List<InteractableBaseData> interactableDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    private static List<ObjectiveBaseData> objectiveDataList;
    private static List<QuestBaseData> questDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.EditorWorld>().First();
        
        regionType = searchParameters.regionType;

        GetRegionData(searchParameters);

        if (regionDataList.Count == 0) return new List<IElementData>();

        GetTileSetData();
        GetTerrainData();
        GetAtmosphereData();
        GetTerrainTileData();
        GetWorldObjectData(searchParameters);
        GetInteractionDestinationData(searchParameters);
        GetInteractionData();
        GetTaskData(searchParameters);
        GetWorldInteractableData();

        GetPhaseData();
        GetChapterData();
        GetChapterWorldInteractableData();
        
        GetInteractableData();
        GetModelData();
        GetIconData();

        GetObjectiveData();
        GetQuestData();

        var list = (
            from regionData     in regionDataList
            join tileSetData    in tileSetDataList on regionData.TileSetId equals tileSetData.Id
            select new EditorWorldElementData
            {
                Id = regionData.Id,

                RegionType = regionType,
                RegionSize = regionData.RegionSize,
                TerrainSize = regionData.TerrainSize,

                TileSetName = tileSetData.Name,
                TileSize = tileSetData.TileSize,

                TerrainDataList = (
                from terrainData in terrainDataList
                select new TerrainElementData()
                {
                    Id = terrainData.Id,
                    Index = terrainData.Index,

                    Name = terrainData.Name,

                    GridElement = TerrainGridElement(terrainData.Index, regionData.RegionSize, regionData.TerrainSize, tileSetData.TileSize),

                    AtmosphereDataList = (
                    from atmosphereData in atmosphereDataList
                    where atmosphereData.TerrainId == terrainData.Id
                    select new AtmosphereElementData()
                    {
                        Id = atmosphereData.Id,

                        TerrainId = atmosphereData.TerrainId,

                        Default = atmosphereData.Default,

                        StartTime = atmosphereData.StartTime,
                        EndTime = atmosphereData.EndTime

                    }).ToList(),

                    TerrainTileDataList = (
                    from terrainTileData in terrainTileDataList
                    where terrainTileData.TerrainId == terrainData.Id
                    select new TerrainTileElementData()
                    {
                        Id = terrainTileData.Id,
                        Index = terrainTileData.Index,

                        TileId = terrainTileData.TileId,

                        Active = false,

                        GridElement = TileGridElement(terrainData.Index, terrainTileData.Index, regionData.RegionSize, regionData.TerrainSize, tileSetData.TileSize)

                    }).OrderBy(x => x.Index).ToList(),

                    WorldInteractableDataList = regionType != Enums.RegionType.InteractionDestination ? (
                    from worldInteractableData      in worldInteractableDataList
                    join interactableData           in interactableDataList on worldInteractableData.InteractableId equals interactableData.Id
                    join modelData                  in modelDataList on interactableData.ModelId             equals modelData.Id
                    join iconData                   in iconDataList on modelData.IconId                     equals iconData.Id
                    join taskData                   in taskDataList on worldInteractableData.Id             equals taskData.WorldInteractableId
                    join interactionData            in interactionDataList on taskData.Id                          equals interactionData.TaskId

                    from interactionDestinationData in interactionDestinationDataList.Where(x => interactionData.Id == x.InteractionId).Take(1)
                    where interactionDestinationData.TerrainId == terrainData.Id
                    select new WorldInteractableElementData()
                    {
                        Id = worldInteractableData.Id,

                        Type = worldInteractableData.Type,

                        PhaseId = worldInteractableData.PhaseId,
                        QuestId = worldInteractableData.QuestId,
                        ObjectiveId = worldInteractableData.ObjectiveId,

                        ChapterInteractableId = worldInteractableData.ChapterInteractableId,
                        InteractableId = interactableData.Id,

                        TerrainTileId = interactionDestinationData.TerrainTileId,

                        Default = interactionData.Default,
                        TaskGroup = taskData.Id,

                        InteractableName = interactableData.Name,

                        PositionX = interactionDestinationData.PositionX,
                        PositionY = interactionDestinationData.PositionY,
                        PositionZ = interactionDestinationData.PositionZ,

                        RotationX = interactionDestinationData.RotationX,
                        RotationY = interactionDestinationData.RotationY,
                        RotationZ = interactionDestinationData.RotationZ,

                        Height = modelData.Height,
                        Width = modelData.Width,
                        Depth = modelData.Depth,

                        Scale = interactableData.Scale,

                        Animation = interactionDestinationData.Animation,

                        ModelId = modelData.Id,
                        ModelPath = modelData.Path,

                        ModelIconPath = iconData.Path,

                        StartTime = interactionData.StartTime,
                        EndTime = interactionData.EndTime,

                    }).ToList() : new List<WorldInteractableElementData>(),

                    InteractionDestinationDataList = regionType == Enums.RegionType.InteractionDestination ? (
                    from interactionDestinationData in interactionDestinationDataList
                    join interactionData            in interactionDataList on interactionDestinationData.InteractionId equals interactionData.Id
                    join taskData                   in taskDataList on interactionData.TaskId                   equals taskData.Id
                    join worldInteractableData      in worldInteractableDataList on taskData.WorldInteractableId             equals worldInteractableData.Id
                    join interactableData           in interactableDataList on worldInteractableData.InteractableId     equals interactableData.Id
                    join modelData                  in modelDataList on interactableData.ModelId                 equals modelData.Id
                    join iconData                   in iconDataList on modelData.IconId                         equals iconData.Id

                    join leftJoin in (from objectiveData in objectiveDataList
                                      select new { objectiveData }) on worldInteractableData.ObjectiveId equals leftJoin.objectiveData.Id into objectiveData

                    join leftJoin in (from questData in questDataList
                                      select new { questData }) on worldInteractableData.QuestId equals leftJoin.questData.Id into questData

                    where interactionDestinationData.TerrainId == terrainData.Id
                    select new InteractionDestinationElementData()
                    {
                        Id = interactionDestinationData.Id,

                        InteractionId = interactionDestinationData.InteractionId,

                        RegionId = interactionDestinationData.RegionId,
                        TerrainId = interactionDestinationData.TerrainId,
                        TerrainTileId = interactionDestinationData.TerrainTileId,
                        
                        PositionX = interactionDestinationData.PositionX,
                        PositionY = interactionDestinationData.PositionY,
                        PositionZ = interactionDestinationData.PositionZ,

                        PositionVariance = interactionDestinationData.PositionVariance,

                        RotationX = interactionDestinationData.RotationX,
                        RotationY = interactionDestinationData.RotationY,
                        RotationZ = interactionDestinationData.RotationZ,

                        FreeRotation = interactionDestinationData.FreeRotation,

                        Animation = interactionDestinationData.Animation,
                        Patience = interactionDestinationData.Patience,

                        QuestId = objectiveData.FirstOrDefault()    != null ? objectiveData.FirstOrDefault().objectiveData.QuestId :
                                  questData.FirstOrDefault()        != null ? questData.FirstOrDefault().questData.Id : 0,
                        ObjectiveId = taskData.ObjectiveId,
                        WorldInteractableId = taskData.WorldInteractableId,
                        TaskId = interactionData.TaskId,

                        ModelId = modelData.Id,
                        ModelPath = modelData.Path,

                        ModelIconPath = iconData.Path,

                        InteractableName = interactableData.Name,

                        Height = modelData.Height,
                        Width = modelData.Width,
                        Depth = modelData.Depth,

                        Scale = interactableData.Scale,

                        Default = interactionData.Default,

                        StartTime = interactionData.StartTime,
                        EndTime = interactionData.EndTime

                    }).ToList() : new List<InteractionDestinationElementData>(),

                    WorldObjectDataList = (
                    from worldObjectData    in worldObjectDataList
                    join modelData          in modelDataList on worldObjectData.ModelId  equals modelData.Id
                    join iconData           in iconDataList on modelData.IconId         equals iconData.Id
                    where worldObjectData.TerrainId == terrainData.Id
                    select new WorldObjectElementData()
                    {
                        Id = worldObjectData.Id,
                        TerrainId = worldObjectData.TerrainId,
                        TerrainTileId = worldObjectData.TerrainTileId,

                        PositionX = worldObjectData.PositionX,
                        PositionY = worldObjectData.PositionY,
                        PositionZ = worldObjectData.PositionZ,

                        RotationX = worldObjectData.RotationX,
                        RotationY = worldObjectData.RotationY,
                        RotationZ = worldObjectData.RotationZ,

                        Scale = worldObjectData.Scale,

                        Animation = worldObjectData.Animation,

                        ModelId = modelData.Id,
                        ModelPath = modelData.Path,

                        ModelName = modelData.Name,
                        ModelIconPath = iconData.Path,

                        Height = modelData.Height,
                        Width = modelData.Width,
                        Depth = modelData.Depth

                    }).ToList()

                }).OrderBy(x => x.Index).ToList(),

                PhaseDataList = (
                from phaseData      in phaseDataList
                join chapterData    in chapterDataList on phaseData.ChapterId equals chapterData.Id

                join leftJoin in (from worldInteractableData    in chapterWorldInteractableDataList
                                  join interactableData         in interactableDataList on worldInteractableData.InteractableId equals interactableData.Id
                                  join modelData                in modelDataList        on interactableData.ModelId             equals modelData.Id
                                  join iconData                 in iconDataList         on modelData.IconId                     equals iconData.Id
                                  select new { worldInteractableData, interactableData, modelData, iconData }) on chapterData.Id equals leftJoin.worldInteractableData.ChapterId into worldInteractableData

                select new PhaseElementData()
                {
                    Id = phaseData.Id,

                    DefaultRegionId = phaseData.DefaultRegionId,

                    DefaultPositionX = phaseData.DefaultPositionX,
                    DefaultPositionY = phaseData.DefaultPositionY,
                    DefaultPositionZ = phaseData.DefaultPositionZ,

                    DefaultRotationX = phaseData.DefaultRotationX,
                    DefaultRotationY = phaseData.DefaultRotationY,
                    DefaultRotationZ = phaseData.DefaultRotationZ,

                    TerrainTileId = RegionManager.GetTerrainTileId(regionData, terrainDataList, terrainTileDataList, tileSetData.TileSize, phaseData.DefaultPositionX, phaseData.DefaultPositionZ),

                    ModelId = worldInteractableData.First().modelData.Id,
                    ModelPath = worldInteractableData.First().modelData.Path,
                    
                    Height = worldInteractableData.First().modelData.Height,
                    Width = worldInteractableData.First().modelData.Width,
                    Depth = worldInteractableData.First().modelData.Depth,

                    Scale = worldInteractableData.First().interactableData.Scale,

                    DefaultTime = phaseData.DefaultTime

                }).ToList()

            }).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetRegionData(Search.EditorWorld searchData)
    {
        var searchParameters = new Search.Region();
        searchParameters.id = searchData.regionId;

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

    private static void GetAtmosphereData()
    {
        var searchParameters = new Search.Atmosphere();
        searchParameters.terrainId = terrainDataList.Select(x => x.Id).Distinct().ToList();

        atmosphereDataList = DataManager.GetAtmosphereData(searchParameters);
    }

    private static void GetTerrainTileData()
    {
        var searchParameters = new Search.TerrainTile();
        searchParameters.terrainId = terrainDataList.Select(x => x.Id).Distinct().ToList();

        terrainTileDataList = DataManager.GetTerrainTileData(searchParameters);
    }

    private static void GetWorldObjectData(Search.EditorWorld searchData)
    {
        var searchParameters = new Search.WorldObject();
        searchParameters.regionId = searchData.id;

        worldObjectDataList = DataManager.GetWorldObjectData(searchParameters);
    }

    private static void GetInteractionDestinationData(Search.EditorWorld searchData)
    {
        var searchParameters = new Search.InteractionDestination();
        searchParameters.regionId = searchData.regionId;

        interactionDestinationDataList = DataManager.GetInteractionDestinationData(searchParameters);
    }

    private static void GetInteractionData()
    {
        var searchParameters = new Search.Interaction();
        searchParameters.id = interactionDestinationDataList.Select(x => x.InteractionId).Distinct().ToList();

        interactionDataList = DataManager.GetInteractionData(searchParameters);
    }

    private static void GetTaskData(Search.EditorWorld searchData)
    {
        var searchParameters = new Search.Task();
        searchParameters.id = interactionDataList.Select(x => x.TaskId).Distinct().ToList();
        searchParameters.objectiveId = searchData.objectiveId;

        taskDataList = DataManager.GetTaskData(searchParameters);
    }

    private static void GetWorldInteractableData()
    {
        var searchParameters = new Search.WorldInteractable();
        searchParameters.id = taskDataList.Select(x => x.WorldInteractableId).Distinct().ToList();

        worldInteractableDataList = DataManager.GetWorldInteractableData(searchParameters);
    }

    private static void GetPhaseData()
    {
        var searchParameters = new Search.Phase();
        searchParameters.defaultRegionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        phaseDataList = DataManager.GetPhaseData(searchParameters);
    }

    private static void GetChapterData()
    {
        var searchParameters = new Search.Chapter();
        searchParameters.id = phaseDataList.Select(x => x.ChapterId).Distinct().ToList();

        chapterDataList = DataManager.GetChapterData(searchParameters);
    }

    private static void GetChapterWorldInteractableData()
    {
        var searchParameters = new Search.WorldInteractable();
        searchParameters.chapterId = chapterDataList.Select(x => x.Id).Distinct().ToList();

        chapterWorldInteractableDataList = DataManager.GetWorldInteractableData(searchParameters);
    }

    private static void GetInteractableData()
    {
        var searchParameters = new Search.Interactable();
        searchParameters.id = worldInteractableDataList.Select(x => x.InteractableId).Union(chapterWorldInteractableDataList.Select(x => x.InteractableId)).Distinct().ToList();

        interactableDataList = DataManager.GetInteractableData(searchParameters);
    }

    private static void GetModelData()
    {
        var searchParameters = new Search.Model();
        searchParameters.id = interactableDataList.Select(x => x.ModelId).Union(worldObjectDataList.Select(x => x.ModelId).Distinct().ToList()).Distinct().ToList();

        modelDataList = DataManager.GetModelData(searchParameters);
    }

    private static void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(searchParameters);
    }

    private static void GetObjectiveData()
    {
        var searchParameters = new Search.Objective();
        searchParameters.id = worldInteractableDataList.Select(x => x.ObjectiveId).Union(taskDataList.Select(x => x.ObjectiveId)).Distinct().ToList();

        objectiveDataList = DataManager.GetObjectiveData(searchParameters);
    }

    private static void GetQuestData()
    {
        var searchParameters = new Search.Quest();
        searchParameters.id = objectiveDataList.Select(x => x.QuestId).Distinct().ToList();

        questDataList = DataManager.GetQuestData(searchParameters);
    }

    private static GridElement TerrainGridElement(int index, int regionSize, int terrainSize, float tileSize)
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

    private static GridElement TileGridElement(int terrainIndex, int tileIndex, int regionSize, int terrainSize, float tileSize)
    {
        var tileStartPosition = TileStartPosition(terrainIndex, tileIndex, regionSize, terrainSize, tileSize);

        var tileRectPosition = new Vector2(tileStartPosition.x - (tileSize / 2),
                                           tileStartPosition.y - (tileSize / 2));

        var tileRectSize = new Vector2(tileSize, -tileSize);

        var tileRect = new Rect(tileRectPosition, tileRectSize);

        var gridElement = new GridElement(tileStartPosition, tileRect);

        return gridElement;
    }

    private static Vector2 TerrainStartPosition(int index, int regionSize, int terrainSize, float tileSize)
    {
        var startPosition = new Vector2((tileSize / 2) + ((index % regionSize) * (terrainSize * tileSize)),
                                       -(tileSize / 2) - (Mathf.Floor(index / regionSize) * (terrainSize * tileSize)));

        return startPosition;
    }

    public static Vector2 TileStartPosition(int terrainIndex, int tileIndex, int regionSize, int terrainSize, float tileSize)
    {
        var terrainStartPosition = TerrainStartPosition(terrainIndex, regionSize, terrainSize, tileSize);

        var startPosition = new Vector2(terrainStartPosition.x + (tileSize * (tileIndex % terrainSize)),
                                        terrainStartPosition.y - (tileSize * (Mathf.Floor(tileIndex / terrainSize))));

        return startPosition;
    }
}