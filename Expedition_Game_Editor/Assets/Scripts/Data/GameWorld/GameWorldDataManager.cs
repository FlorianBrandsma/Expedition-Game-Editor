using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class GameWorldDataManager
{
    private static ChapterBaseData chapterData;
    private static List<WorldInteractableBaseData> chapterWorldInteractableDataList;
    private static PhaseBaseData phaseData;

    private static List<RegionBaseData> regionDataList;
    private static List<TileSetBaseData> tileSetDataList;
    private static List<TerrainBaseData> terrainDataList;
    private static List<AtmosphereBaseData> atmosphereDataList;
    private static List<TerrainTileBaseData> terrainTileDataList;
    private static List<WorldObjectBaseData> worldObjectDataList;
    private static List<TaskBaseData> taskDataList;
    private static List<InteractionBaseData> interactionDataList;

    private static List<InteractionDestinationBaseData> interactionDestinationDataList;

    private static List<OutcomeBaseData> outcomeDataList;
    private static List<SceneBaseData> sceneDataList;
    private static List<SceneActorBaseData> sceneActorDataList;
    private static List<ScenePropBaseData> scenePropDataList;
    private static List<SceneShotBaseData> sceneShotDataList;
    private static List<CameraFilterBaseData> cameraFilterDataList;
    
    private static List<WorldInteractableBaseData> worldInteractableDataList;
    private static List<InteractableBaseData> interactableDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    private static List<ObjectiveBaseData> objectiveDataList;
    private static List<QuestBaseData> questDataList;

    private static List<int> defaultTimeList;

    public static List<IElementData> GetData(Search.GameWorld searchParameters)
    {
        Debug.Log("Get game data");

        GetPhaseData(searchParameters);

        if (phaseData == null) return new List<IElementData>();

        GetChapterData();
        GetChapterWorldInteractableData();

        GetRegionData();

        GetTileSetData();
        GetTerrainData();
        GetAtmosphereData();
        GetTerrainTileData();

        GetWorldObjectData();

        GetInteractionDestinationData();
        GetInteractionData();

        GetOutcomeData();
        GetSceneData();
        GetSceneActorData();
        GetScenePropData();
        GetSceneShotData();
        GetCameraFilterData();

        GetTaskData();
        GetWorldInteractableData();
        GetInteractableData();
        GetModelData();
        GetIconData();

        GetObjectiveData();
        GetQuestData();

        var gameWorldData = new GameWorldElementData()
        {
            ChapterData = new ChapterElementData()
            {
                Id = chapterData.Id,
                Index = chapterData.Index,

                Name = chapterData.Name,

                TimeSpeed = chapterData.TimeSpeed
            },

            PhaseData = new PhaseElementData()
            {
                Id = phaseData.Id,
                Index = phaseData.Index,

                ChapterId = phaseData.ChapterId,

                Name = phaseData.Name,

                DefaultRegionId = phaseData.DefaultRegionId,

                DefaultPositionX = phaseData.DefaultPositionX,
                DefaultPositionY = phaseData.DefaultPositionY,
                DefaultPositionZ = phaseData.DefaultPositionZ,

                DefaultRotationX = phaseData.DefaultRotationX,
                DefaultRotationY = phaseData.DefaultRotationY,
                DefaultRotationZ = phaseData.DefaultRotationZ,

                DefaultTime = phaseData.DefaultTime
            },

            WorldInteractableDataList = (
            from worldInteractableData  in worldInteractableDataList
            join interactableData       in interactableDataList on worldInteractableData.InteractableId equals interactableData.Id
            join modelData              in modelDataList        on interactableData.ModelId             equals modelData.Id
            join iconData               in iconDataList         on modelData.IconId                     equals iconData.Id
            select new GameWorldInteractableElementData()
            {
                Id = worldInteractableData.Id,
                
                TerrainTileId = 0,
                ModelId = modelData.Id,

                Type = (Enums.InteractableType)worldInteractableData.Type,

                ModelPath = modelData.Path,
                ModelIconPath = iconData.Path,

                InteractableName = interactableData.Name,

                Health = interactableData.Health,
                Hunger = interactableData.Hunger,
                Thirst = interactableData.Thirst,

                Weight = interactableData.Weight,
                Speed = interactableData.Speed,
                Stamina = interactableData.Stamina,

                Height = modelData.Height,
                Width = modelData.Width,
                Depth = modelData.Depth,

                Scale = interactableData.Scale,

                InteractionDataList = (
                from interactionData    in interactionDataList
                join taskData           in taskDataList on interactionData.TaskId equals taskData.Id

                where taskData.WorldInteractableId == worldInteractableData.Id
                select new GameInteractionElementData()
                {
                    Id = interactionData.Id,

                    TaskId = interactionData.TaskId,

                    Default = interactionData.Default,

                    StartTime = interactionData.StartTime,
                    EndTime = interactionData.EndTime,

                    ArrivalType = (Enums.ArrivalType)interactionData.ArrivalType,

                    TriggerAutomatically = interactionData.TriggerAutomatically,
                    BeNearDestination = interactionData.BeNearDestination,
                    FaceInteractable = interactionData.FaceInteractable,
                    FaceControllable = interactionData.FaceControllable,
                    HideInteractionIndicator = interactionData.HideInteractionIndicator,

                    InteractionRange = interactionData.InteractionRange,

                    DelayMethod = (Enums.DelayMethod)interactionData.DelayMethod,
                    DelayDuration = interactionData.DelayDuration,
                    HideDelayIndicator = interactionData.HideDelayIndicator,

                    CancelDelayOnInput = interactionData.CancelDelayOnInput,
                    CancelDelayOnMovement = interactionData.CancelDelayOnMovement,
                    CancelDelayOnHit = interactionData.CancelDelayOnHit,

                    ObjectiveId = taskData.ObjectiveId,
                    WorldInteractableId = taskData.WorldInteractableId,
                    
                    InteractionDestinationDataList = (
                    from interactionDestinationData in interactionDestinationDataList
                    where interactionData.Id == interactionDestinationData.InteractionId
                    select new GameInteractionDestinationElementData()
                    {
                        Id = interactionDestinationData.Id,

                        RegionId = interactionDestinationData.RegionId,
                        TerrainTileId = interactionDestinationData.TerrainTileId,

                        PositionX = interactionDestinationData.PositionX,
                        PositionY = interactionDestinationData.PositionY,
                        PositionZ = interactionDestinationData.PositionZ,

                        PositionVariance = interactionDestinationData.PositionVariance,

                        RotationX = interactionDestinationData.RotationX,
                        RotationY = interactionDestinationData.RotationY,
                        RotationZ = interactionDestinationData.RotationZ,

                        FreeRotation = interactionDestinationData.ChangeRotation,

                        Animation = interactionDestinationData.Animation,
                        Patience = interactionDestinationData.Patience

                    }).ToList(),
                    
                    OutcomeDataList = (
                    from outcomeData in outcomeDataList
                    where interactionData.Id == outcomeData.InteractionId
                    select new GameOutcomeElementData()
                    {
                        Id = outcomeData.Id,

                        Type = outcomeData.Type,

                        CompleteTask = outcomeData.CompleteTask,
                        ResetObjective = outcomeData.ResetObjective,

                        CancelScenarioType = outcomeData.CancelScenarioType,
                        CancelScenarioOnInteraction = outcomeData.CancelScenarioOnInteraction,
                        CancelScenarioOnInput = outcomeData.CancelScenarioOnInput,
                        CancelScenarioOnRange = outcomeData.CancelScenarioOnRange,
                        CancelScenarioOnHit = outcomeData.CancelScenarioOnHit,

                        SceneDataList = (
                        from sceneData in sceneDataList
                        where outcomeData.Id == sceneData.OutcomeId
                        select new GameSceneElementData()
                        {
                            Id = sceneData.Id,

                            RegionId = sceneData.RegionId,

                            FreezeTime = sceneData.FreezeTime,
                            AutoContinue = sceneData.AutoContinue,
                            SetActorsInstantly = sceneData.SetActorsInstantly,

                            SceneDuration = sceneData.SceneDuration,
                            ShotDuration = sceneData.ShotDuration,
                            
                            SceneShotDataList = (
                            from sceneShotData in sceneShotDataList

                            join leftJoin in (from cameraFilterData in cameraFilterDataList
                                              select new { cameraFilterData }) on sceneShotData.CameraFilterId equals leftJoin.cameraFilterData.Id into cameraFilterData

                            join leftJoin in (from sceneActorData in sceneActorDataList
                                              select new { sceneActorData }) on sceneShotData.PositionTargetSceneActorId equals leftJoin.sceneActorData.Id into positionTargetSceneActorData

                            join leftJoin in (from sceneActorData in sceneActorDataList
                                              select new { sceneActorData }) on sceneShotData.RotationTargetSceneActorId equals leftJoin.sceneActorData.Id into rotationTargetSceneActorData

                            where sceneData.Id == sceneShotData.SceneId
                            select new GameSceneShotElementData()
                            {
                                Id = sceneShotData.Id,

                                Type = sceneShotData.Type,

                                ChangePosition = sceneShotData.ChangePosition,

                                PositionX = sceneShotData.PositionX,
                                PositionY = sceneShotData.PositionY,
                                PositionZ = sceneShotData.PositionZ,

                                PositionTargetWorldInteractableId = positionTargetSceneActorData.FirstOrDefault() != null ? positionTargetSceneActorData.FirstOrDefault().sceneActorData.WorldInteractableId : 0,

                                ChangeRotation = sceneShotData.ChangeRotation,

                                RotationX = sceneShotData.RotationX,
                                RotationY = sceneShotData.RotationY,
                                RotationZ = sceneShotData.RotationZ,

                                RotationTargetWorldInteractableId = rotationTargetSceneActorData.FirstOrDefault() != null ? rotationTargetSceneActorData.FirstOrDefault().sceneActorData.WorldInteractableId : 0,

                                CameraFilterPath = cameraFilterData.FirstOrDefault() != null ? cameraFilterData.FirstOrDefault().cameraFilterData.Path : ""

                            }).ToList(),

                            SceneActorDataList = (
                            from sceneActorData in sceneActorDataList

                            join leftJoin in (from sceneActorData in sceneActorDataList
                                              select new { sceneActorData }) on sceneActorData.TargetSceneActorId equals leftJoin.sceneActorData.Id into targetSceneActorData

                            where sceneData.Id == sceneActorData.SceneId
                            select new GameSceneActorElementData()
                            {
                                Id = sceneActorData.Id,

                                WorldInteractableId = sceneActorData.WorldInteractableId,
                                TerrainTileId = sceneActorData.TerrainTileId,
        
                                SpeechMethod = sceneActorData.SpeechMethod,
                                SpeechText = sceneActorData.SpeechText,
                                ShowTextBox = sceneActorData.ShowTextBox,

                                TargetWorldInteractableId = targetSceneActorData.FirstOrDefault() != null ? targetSceneActorData.FirstOrDefault().sceneActorData.WorldInteractableId : 0,

                                ChangePosition = sceneActorData.ChangePosition,
                                FreezePosition = sceneActorData.FreezePosition,

                                PositionX = sceneActorData.PositionX,
                                PositionY = sceneActorData.PositionY,
                                PositionZ = sceneActorData.PositionZ,

                                ChangeRotation = sceneActorData.ChangeRotation,
                                FaceTarget = sceneActorData.FaceTarget,

                                RotationX = sceneActorData.RotationX,
                                RotationY = sceneActorData.RotationY,
                                RotationZ = sceneActorData.RotationZ

                            }).ToList(),

                            ScenePropDataList = (
                            from scenePropData  in scenePropDataList
                            join modelData      in modelDataList on scenePropData.ModelId equals modelData.Id
                            where sceneData.Id == scenePropData.SceneId
                            select new GameScenePropElementData()
                            {
                                Id = scenePropData.Id,

                                TerrainTileId = scenePropData.TerrainTileId,
                                ModelId = modelData.Id,

                                ModelPath = modelData.Path,

                                PositionX = scenePropData.PositionX,
                                PositionY = scenePropData.PositionY,
                                PositionZ = scenePropData.PositionZ,

                                RotationX = scenePropData.RotationX,
                                RotationY = scenePropData.RotationY,
                                RotationZ = scenePropData.RotationZ,

                                Scale = scenePropData.Scale

                            }).ToList()

                        }).ToList()

                    }).ToList()

                }).ToList()

            }).ToList(),

            RegionDataList = (
            from regionData     in regionDataList
            join tileSetData    in tileSetDataList on regionData.TileSetId equals tileSetData.Id
            select new GameRegionElementData
            {
                Id = regionData.Id,
                
                PhaseId = regionData.PhaseId,

                Type = Enums.RegionType.Game,
                RegionSize = regionData.RegionSize,
                TerrainSize = regionData.TerrainSize,

                TileSetName = tileSetData.Name,
                TileSize = tileSetData.TileSize,

                GameTerrainDataList = (
                from terrainData in terrainDataList.Where(x => x.RegionId == regionData.Id)
                select new GameTerrainElementData
                {
                    Id = terrainData.Id,

                    Index = terrainData.Index,

                    Name = terrainData.Name,

                    GridElement = TerrainGridElement(terrainData.Index, regionData.RegionSize, regionData.TerrainSize, tileSetData.TileSize),
                    
                    GameAtmosphereDataList = (
                    from atmosphereData in atmosphereDataList
                    where atmosphereData.TerrainId == terrainData.Id
                    select new GameAtmosphereElementData()
                    {
                        Id = atmosphereData.Id,
                        
                        TerrainId = atmosphereData.TerrainId,

                        Default = atmosphereData.Default,

                        StartTime = atmosphereData.StartTime,
                        EndTime = atmosphereData.EndTime

                    }).ToList(),

                    GameTerrainTileDataList = (
                    from terrainTileData in terrainTileDataList
                    where terrainTileData.TerrainId == terrainData.Id
                    select new GameTerrainTileElementData()
                    {
                        Id = terrainTileData.Id,

                        TileId = terrainTileData.TileId,

                        Index = terrainTileData.Index,

                        Active = false,

                        GridElement = TileGridElement(terrainData.Index, terrainTileData.Index, regionData.RegionSize, regionData.TerrainSize, tileSetData.TileSize)
                        
                    }).ToList(),

                    GameWorldObjectDataList = (
                    from worldObjectData    in worldObjectDataList
                    join modelData          in modelDataList    on worldObjectData.ModelId  equals modelData.Id
                    where worldObjectData.TerrainId == terrainData.Id
                    select new GameWorldObjectElementData()
                    {
                        Id = worldObjectData.Id,

                        TerrainTileId = worldObjectData.TerrainTileId,

                        ModelId = modelData.Id,

                        PositionX = worldObjectData.PositionX,
                        PositionY = worldObjectData.PositionY,
                        PositionZ = worldObjectData.PositionZ,

                        RotationX = worldObjectData.RotationX,
                        RotationY = worldObjectData.RotationY,
                        RotationZ = worldObjectData.RotationZ,

                        Scale = worldObjectData.Scale,

                        Animation = worldObjectData.Animation,
                        
                        ModelPath = modelData.Path

                    }).ToList()

                }).ToList()

            }).ToList()
        };

        return new List<IElementData>() { gameWorldData };
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

    private static Vector2 TileStartPosition(int terrainIndex, int tileIndex, int regionSize, int terrainSize, float tileSize)
    {
        var terrainStartPosition = TerrainStartPosition(terrainIndex, regionSize, terrainSize, tileSize);

        var startPosition = new Vector2(terrainStartPosition.x + (tileSize * (tileIndex % terrainSize)),
                                        terrainStartPosition.y - (tileSize * (Mathf.Floor(tileIndex / terrainSize))));

        return startPosition;
    }

    private static void GetPhaseData(Search.GameWorld searchData)
    {
        var searchParameters = new Search.Phase();
        searchParameters.id = searchData.phaseId;

        phaseData = DataManager.GetPhaseData(searchParameters).First();
    }

    private static void GetChapterData()
    {
        var searchParameters = new Search.Chapter();
        searchParameters.id = new List<int>() { phaseData.ChapterId };
        
        chapterData = DataManager.GetChapterData(searchParameters).First();
    }

    private static void GetChapterWorldInteractableData()
    {
        var searchParameters = new Search.WorldInteractable();
        searchParameters.chapterId = new List<int>() { chapterData.Id };
        
        chapterWorldInteractableDataList = DataManager.GetWorldInteractableData(searchParameters);
    }

    private static void GetRegionData()
    {
        var searchParameters = new Search.Region();
        searchParameters.phaseId = new List<int>() { phaseData.Id };

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

    private static void GetWorldObjectData()
    {
        var searchParameters = new Search.WorldObject();
        searchParameters.regionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        worldObjectDataList = DataManager.GetWorldObjectData(searchParameters);
    }

    private static void GetInteractionDestinationData()
    {
        var searchParameters = new Search.InteractionDestination();
        searchParameters.regionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        interactionDestinationDataList = DataManager.GetInteractionDestinationData(searchParameters);
    }

    private static void GetInteractionData()
    {
        var searchParameters = new Search.Interaction();
        searchParameters.id = interactionDestinationDataList.Select(x => x.InteractionId).Distinct().ToList();

        interactionDataList = DataManager.GetInteractionData(searchParameters);
    }

    private static void GetOutcomeData()
    {
        var searchParameters = new Search.Outcome();
        searchParameters.interactionId = interactionDataList.Select(x => x.Id).Distinct().ToList();

        outcomeDataList = DataManager.GetOutcomeData(searchParameters);
    }

    private static void GetSceneData()
    {
        var searchParameters = new Search.Scene();
        searchParameters.outcomeId = outcomeDataList.Select(x => x.Id).Distinct().ToList();

        sceneDataList = DataManager.GetSceneData(searchParameters);
    }
    
    private static void GetSceneActorData()
    {
        var searchParameters = new Search.SceneActor();
        searchParameters.sceneId = sceneDataList.Select(x => x.Id).Distinct().ToList();

        sceneActorDataList = DataManager.GetSceneActorData(searchParameters);
    }

    private static void GetScenePropData()
    {
        var searchParameters = new Search.SceneProp();
        searchParameters.sceneId = sceneDataList.Select(x => x.Id).Distinct().ToList();

        scenePropDataList = DataManager.GetScenePropData(searchParameters);
    }

    private static void GetSceneShotData()
    {
        var searchParameters = new Search.SceneShot();
        searchParameters.sceneId = sceneDataList.Select(x => x.Id).Distinct().ToList();

        sceneShotDataList = DataManager.GetSceneShotData(searchParameters);
    }

    private static void GetCameraFilterData()
    {
        var searchParameters = new Search.CameraFilter();
        searchParameters.id = sceneShotDataList.Select(x => x.CameraFilterId).Distinct().ToList();

        cameraFilterDataList = DataManager.GetCameraFilterData(searchParameters);
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
        searchParameters.id = taskDataList.Select(x => x.WorldInteractableId).Union(chapterWorldInteractableDataList.Select(x => x.Id)).Distinct().ToList();

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
        searchParameters.id = interactableDataList.Select(x => x.ModelId).Union(worldObjectDataList.Select(x => x.ModelId))
                                                                         .Union(scenePropDataList.Select(x => x.ModelId)).Distinct().ToList();

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
}