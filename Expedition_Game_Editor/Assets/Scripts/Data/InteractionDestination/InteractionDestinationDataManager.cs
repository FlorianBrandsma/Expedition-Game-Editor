using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class InteractionDestinationDataManager
{
    private static List<InteractionDestinationBaseData> interactionDestinationDataList;

    private static List<InteractionBaseData> interactionDataList;
    private static List<TaskBaseData> taskDataList;
    private static List<WorldInteractableBaseData> worldInteractableDataList;
    private static List<InteractableBaseData> interactableDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    private static List<ObjectiveBaseData> objectiveDataList;
    private static List<QuestBaseData> questDataList;

    private static List<RegionBaseData> regionDataList;
    private static List<TerrainBaseData> terrainDataList;
    private static List<TerrainTileBaseData> terrainTileDataList;
    private static List<TileBaseData> tileDataList;
    private static List<TileSetBaseData> tileSetDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.InteractionDestination>().First();

        GetInteractionDestinationData(searchParameters);

        if (interactionDestinationDataList.Count == 0) return new List<IElementData>();

        GetInteractionData();
        GetTaskData();
        GetWorldInteractableData();
        GetInteractableData();
        GetModelData();
        GetIconData();

        GetObjectiveData();
        GetQuestData();

        GetRegionData();
        GetTerrainData();
        GetTerrainTileData();
        GetTileData();
        GetTileSetData();

        var list = (from interactionDestinationData in interactionDestinationDataList
                    join interactionData            in interactionDataList          on interactionDestinationData.InteractionId equals interactionData.Id
                    join taskData                   in taskDataList                 on interactionData.TaskId                   equals taskData.Id
                    join worldInteractableData      in worldInteractableDataList    on taskData.WorldInteractableId             equals worldInteractableData.Id
                    join interactableData           in interactableDataList         on worldInteractableData.InteractableId     equals interactableData.Id
                    join modelData                  in modelDataList                on interactableData.ModelId                 equals modelData.Id
                    join iconData                   in iconDataList                 on modelData.IconId                         equals iconData.Id

                    join terrainTileData            in terrainTileDataList          on interactionDestinationData.TerrainTileId equals terrainTileData.Id
                    join terrainData                in terrainDataList              on terrainTileData.TerrainId                equals terrainData.Id
                    join regionData                 in regionDataList               on terrainData.RegionId                     equals regionData.Id
                    join tileData                   in tileDataList                 on terrainTileData.TileId                   equals tileData.Id
                    join tileSetData                in tileSetDataList              on regionData.TileSetId                     equals tileSetData.Id

                    join leftJoin in (from objectiveData in objectiveDataList
                                      select new { objectiveData }) on worldInteractableData.ObjectiveId equals leftJoin.objectiveData.Id into objectiveData

                    join leftJoin in (from questData in questDataList
                                      select new { questData }) on worldInteractableData.QuestId equals leftJoin.questData.Id into questData

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

                        TileIconPath = tileData.IconPath,
                        TileSize = tileSetData.TileSize,

                        LocalPosition = RegionManager.PositionOnTile(regionData.RegionSize, regionData.TerrainSize, tileSetData.TileSize, interactionDestinationData.PositionX, interactionDestinationData.PositionZ),

                        LocationName = regionData.Name + ", " + terrainData.Name,

                        InteractableStatus = "Idle, " + interactableData.Name,
                        
                        Default = interactionData.Default,

                        StartTime = interactionData.StartTime,
                        EndTime = interactionData.EndTime

                    }).OrderBy(x => x.Id).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetInteractionDestinationData(Search.InteractionDestination searchParameters)
    {
        interactionDestinationDataList = new List<InteractionDestinationBaseData>();

        foreach (InteractionDestinationBaseData interactionDestination in Fixtures.interactionDestinationList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(interactionDestination.Id)) continue;
            if (searchParameters.interactionId.Count    > 0 && !searchParameters.interactionId.Contains(interactionDestination.InteractionId)) continue;

            var interactionDestinationData = new InteractionDestinationBaseData();

            interactionDestinationData.Id = interactionDestination.Id;

            interactionDestinationData.InteractionId = interactionDestination.InteractionId;

            interactionDestinationData.RegionId = interactionDestination.RegionId;
            interactionDestinationData.TerrainId = interactionDestination.TerrainId;
            interactionDestinationData.TerrainTileId = interactionDestination.TerrainTileId;

            interactionDestinationData.PositionX = interactionDestination.PositionX;
            interactionDestinationData.PositionY = interactionDestination.PositionY;
            interactionDestinationData.PositionZ = interactionDestination.PositionZ;

            interactionDestinationData.PositionVariance = interactionDestination.PositionVariance;

            interactionDestinationData.RotationX = interactionDestination.RotationX;
            interactionDestinationData.RotationY = interactionDestination.RotationY;
            interactionDestinationData.RotationZ = interactionDestination.RotationZ;

            interactionDestinationData.FreeRotation = interactionDestination.FreeRotation;

            interactionDestinationData.Animation = interactionDestination.Animation;
            interactionDestinationData.Patience = interactionDestination.Patience;

            interactionDestinationDataList.Add(interactionDestinationData);
        }
    }

    private static void GetInteractionData()
    {
        var interactionSearchParameters = new Search.Interaction();

        interactionSearchParameters.id = interactionDestinationDataList.Select(x => x.InteractionId).Distinct().ToList();

        interactionDataList = DataManager.GetInteractionData(interactionSearchParameters);
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

    private static void GetObjectiveData()
    {
        var objectiveSearchParameters = new Search.Objective();
        objectiveSearchParameters.id = worldInteractableDataList.Select(x => x.ObjectiveId).Union(taskDataList.Select(x => x.ObjectiveId)).Distinct().ToList();

        objectiveDataList = DataManager.GetObjectiveData(objectiveSearchParameters);
    }

    private static void GetQuestData()
    {
        var questSearchParameters = new Search.Quest();
        questSearchParameters.id = objectiveDataList.Select(x => x.QuestId).Distinct().ToList();

        questDataList = DataManager.GetQuestData(questSearchParameters);
    }

    private static void GetRegionData()
    {
        var searchParameters = new Search.Region();
        searchParameters.id = interactionDestinationDataList.Select(x => x.RegionId).Distinct().ToList();

        regionDataList = DataManager.GetRegionData(searchParameters);
    }

    private static void GetTerrainData()
    {
        var searchParameters = new Search.Terrain();
        searchParameters.regionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        terrainDataList = DataManager.GetTerrainData(searchParameters);
    }

    private static void GetTerrainTileData()
    {
        var terrainTileSearchParameters = new Search.TerrainTile();
        terrainTileSearchParameters.id = interactionDestinationDataList.Select(x => x.TerrainTileId).Distinct().ToList();

        terrainTileDataList = DataManager.GetTerrainTileData(terrainTileSearchParameters);
    }

    private static void GetTileData()
    {
        var tileSearchParameters = new Search.Tile();
        tileSearchParameters.id = terrainTileDataList.Select(x => x.TileId).Distinct().ToList();

        tileDataList = DataManager.GetTileData(tileSearchParameters);
    }

    private static void GetTileSetData()
    {
        var tileSetSearchParameters = new Search.TileSet();
        tileSetSearchParameters.id = regionDataList.Select(x => x.TileSetId).Distinct().ToList();

        tileSetDataList = DataManager.GetTileSetData(tileSetSearchParameters);
    }

    public static void UpdateData(InteractionDestinationElementData elementData)
    {
        var data = Fixtures.interactionDestinationList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedRegionId)
            data.RegionId = elementData.RegionId;

        if (elementData.ChangedTerrainId)
            data.TerrainId = elementData.TerrainId;

        if (elementData.ChangedTerrainTileId)
            data.TerrainTileId = elementData.TerrainTileId;

        if (elementData.ChangedPositionX)
            data.PositionX = elementData.PositionX;

        if (elementData.ChangedPositionY)
            data.PositionY = elementData.PositionY;

        if (elementData.ChangedPositionZ)
            data.PositionZ = elementData.PositionZ;

        if (elementData.ChangedPositionVariance)
            data.PositionVariance = elementData.PositionVariance;

        if (elementData.ChangedRotationX)
            data.RotationX = elementData.RotationX;

        if (elementData.ChangedRotationY)
            data.RotationY = elementData.RotationY;

        if (elementData.ChangedRotationZ)
            data.RotationZ = elementData.RotationZ;

        if (elementData.ChangedFreeRotation)
            data.FreeRotation = elementData.FreeRotation;

        if (elementData.ChangedAnimation)
            data.Animation = elementData.Animation;

        if (elementData.ChangedPatience)
            data.Patience = elementData.Patience;
    }
}

