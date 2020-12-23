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

                        ChangeRotation = interactionDestinationData.ChangeRotation,

                        Animation = interactionDestinationData.Animation,
                        Patience = interactionDestinationData.Patience,

                        TaskId = interactionData.TaskId,
                        ObjectiveId = taskData.ObjectiveId,
                        WorldInteractableId = taskData.WorldInteractableId,
                        QuestId = objectiveData.FirstOrDefault()    != null ? objectiveData.FirstOrDefault().objectiveData.QuestId :
                                  questData.FirstOrDefault()        != null ? questData.FirstOrDefault().questData.Id : 0,
                        
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

            interactionDestinationDataList.Add(interactionDestination);
        }
    }

    private static void GetInteractionData()
    {
        var searchParameters = new Search.Interaction();
        searchParameters.id = interactionDestinationDataList.Select(x => x.InteractionId).Distinct().ToList();

        interactionDataList = DataManager.GetInteractionData(searchParameters);
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
        var searchParameters = new Search.TerrainTile();
        searchParameters.id = interactionDestinationDataList.Select(x => x.TerrainTileId).Distinct().ToList();

        terrainTileDataList = DataManager.GetTerrainTileData(searchParameters);
    }

    private static void GetTileData()
    {
        var searchParameters = new Search.Tile();
        searchParameters.id = terrainTileDataList.Select(x => x.TileId).Distinct().ToList();

        tileDataList = DataManager.GetTileData(searchParameters);
    }

    private static void GetTileSetData()
    {
        var searchParameters = new Search.TileSet();
        searchParameters.id = regionDataList.Select(x => x.TileSetId).Distinct().ToList();

        tileSetDataList = DataManager.GetTileSetData(searchParameters);
    }

    public static void UpdateData(InteractionDestinationElementData elementData, DataRequest dataRequest)
    {
        var data = Fixtures.interactionDestinationList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedRegionId)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.RegionId = elementData.RegionId;
            else { }
        }

        if (elementData.ChangedTerrainId)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.TerrainId = elementData.TerrainId;
            else { }
        }

        if (elementData.ChangedTerrainTileId)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.TerrainTileId = elementData.TerrainTileId;
            else { }
        }

        if (elementData.ChangedPositionX)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PositionX = elementData.PositionX;
            else { }
        }

        if (elementData.ChangedPositionY)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PositionY = elementData.PositionY;
            else { }
        }

        if (elementData.ChangedPositionZ)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PositionZ = elementData.PositionZ;
            else { }
        }

        if (elementData.ChangedPositionVariance)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PositionVariance = elementData.PositionVariance;
            else { }
        }

        if (elementData.ChangedRotationX)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.RotationX = elementData.RotationX;
            else { }
        }

        if (elementData.ChangedRotationY)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.RotationY = elementData.RotationY;
            else { }
        }

        if (elementData.ChangedRotationZ)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.RotationZ = elementData.RotationZ;
            else { }
        }

        if (elementData.ChangedChangeRotation)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.ChangeRotation = elementData.ChangeRotation;
            else { }
        }

        if (elementData.ChangedAnimation)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.Animation = elementData.Animation;
            else { }
        }

        if (elementData.ChangedPatience)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.Patience = elementData.Patience;
            else { }
        }
    }
}

