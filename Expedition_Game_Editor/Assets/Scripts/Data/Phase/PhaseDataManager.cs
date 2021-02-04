using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public static class PhaseDataManager
{
    private static List<PhaseBaseData> phaseDataList;

    private static List<ChapterBaseData> chapterDataList;
    private static List<WorldInteractableBaseData> worldInteractableDataList;
    private static List<InteractableBaseData> interactableDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    private static List<RegionBaseData> regionDataList;
    private static List<TerrainBaseData> terrainDataList;
    private static List<TerrainTileBaseData> terrainTileDataList;
    private static List<TileSetBaseData> tileSetDataList;

    public static List<IElementData> GetData(Search.Phase searchParameters)
    {
        GetPhaseData(searchParameters);

        if (searchParameters.includeAddElement)
            phaseDataList.Add(DefaultData(searchParameters.chapterId.First()));

        if (phaseDataList.Count == 0) return new List<IElementData>();
        
        GetChapterData();
        GetWorldInteractableData();
        GetInteractableData();
        GetModelData();
        GetIconData();

        GetRegionData();
        GetTileSetData();
        GetTerrainData();
        GetTerrainTileData();

        var list = (from phaseData      in phaseDataList
                    join chapterData    in chapterDataList  on phaseData.ChapterId equals chapterData.Id

                    join leftJoin in (from regionData   in regionDataList
                                      join tileSetData  in tileSetDataList on regionData.TileSetId equals tileSetData.Id
                                      select new { regionData, tileSetData }) on phaseData.DefaultRegionId equals leftJoin.regionData.Id into regionData

                    join leftJoin in (from worldInteractableData    in worldInteractableDataList
                                      join interactableData         in interactableDataList on worldInteractableData.InteractableId equals interactableData.Id
                                      join modelData                in modelDataList        on interactableData.ModelId             equals modelData.Id
                                      join iconData                 in iconDataList         on modelData.IconId                     equals iconData.Id
                                      select new { worldInteractableData, interactableData, modelData, iconData }) on chapterData.Id equals leftJoin.worldInteractableData.ChapterId into worldInteractableData

                    select new PhaseElementData()
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

                        Scale = worldInteractableData.First().interactableData.Scale,

                        DefaultTime = phaseData.DefaultTime,

                        PublicNotes = phaseData.PublicNotes,
                        PrivateNotes = phaseData.PrivateNotes,

                        TerrainTileId = regionData.FirstOrDefault() != null ? RegionManager.GetTerrainTileId(regionData.FirstOrDefault().regionData, terrainDataList, terrainTileDataList, regionData.FirstOrDefault().tileSetData.TileSize, phaseData.DefaultPositionX, phaseData.DefaultPositionZ) : 0,

                        WorldInteractableId = worldInteractableData.First().worldInteractableData.Id,
                        
                        ModelId = worldInteractableData.First().modelData.Id,
                        ModelPath = worldInteractableData.First().modelData.Path,

                        ModelIconPath = worldInteractableData.First().iconData.Path,
                        
                        Height = worldInteractableData.First().modelData.Height,
                        Width = worldInteractableData.First().modelData.Width,
                        Depth = worldInteractableData.First().modelData.Depth,

                        InteractableName = worldInteractableData.First().interactableData.Name,
                        LocationName = regionData.FirstOrDefault() != null ? RegionManager.LocationName(phaseData.DefaultPositionX, phaseData.DefaultPositionZ, regionData.FirstOrDefault().tileSetData.TileSize, regionData.FirstOrDefault().regionData, terrainDataList) : ""
                        
                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Index).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static PhaseElementData DefaultData(int chapterId)
    {
        return new PhaseElementData()
        {
            Id = -1,

            ChapterId = chapterId
        };
    }

    public static void SetDefaultAddValues(List<PhaseElementData> list)
    {
        var addElementData = list.Where(x => x.Id == -1).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;

        addElementData.Index = list.Count - 1;
    }

    private static void GetPhaseData(Search.Phase searchParameters)
    {
        phaseDataList = new List<PhaseBaseData>();

        foreach(PhaseBaseData phase in Fixtures.phaseList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(phase.Id))                 continue;
            if (searchParameters.chapterId.Count    > 0 && !searchParameters.chapterId.Contains(phase.ChapterId))   continue;

            phaseDataList.Add(phase);
        }
    }

    private static void GetChapterData()
    {
        var searchParameters = new Search.Chapter();
        searchParameters.id = phaseDataList.Select(x => x.ChapterId).Distinct().ToList();

        chapterDataList = DataManager.GetChapterData(searchParameters);
    }

    private static void GetWorldInteractableData()
    {
        var searchParameters = new Search.WorldInteractable();
        searchParameters.chapterId = chapterDataList.Select(x => x.Id).Distinct().ToList();

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

    private static void GetRegionData()
    {
        var searchParameters = new Search.Region();
        searchParameters.id = phaseDataList.Select(x => x.DefaultRegionId).Distinct().ToList();

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

    private static void GetTerrainTileData()
    {
        var searchParameters = new Search.TerrainTile();
        searchParameters.terrainId = terrainDataList.Select(x => x.Id).Distinct().ToList();

        terrainTileDataList = DataManager.GetTerrainTileData(searchParameters);
    }

    public static void AddData(PhaseElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.phaseList.Count > 0 ? (Fixtures.phaseList[Fixtures.phaseList.Count - 1].Id + 1) : 1;
            Fixtures.phaseList.Add(((PhaseData)elementData).Clone());

            elementData.SetOriginalValues();

            AddDependencies(elementData, dataRequest);
            
        } else { }
    }

    private static void AddDependencies(PhaseElementData elementData, DataRequest dataRequest)
    {
        AddPhaseSaveData(elementData, dataRequest);

        if (!dataRequest.includeDependencies) return;

        AddRegionData(elementData, dataRequest);
        AddWorldInteractableData(elementData, dataRequest);
    }

    private static void AddPhaseSaveData(PhaseElementData elementData, DataRequest dataRequest)
    {
        //Save
        var saveSearchParameters = new Search.Save();

        var saveDataList = DataManager.GetSaveData(saveSearchParameters);

        saveDataList.ForEach(saveData =>
        {
            var phaseSaveElementData = PhaseSaveDataManager.DefaultData(saveData.Id, elementData.Id);
            phaseSaveElementData.Add(dataRequest);
        });
    }

    private static void AddRegionData(PhaseElementData elementData, DataRequest dataRequest)
    {
        var chapterRegionSearchParameters = new Search.ChapterRegion()
        {
            chapterId = new List<int>() { elementData.ChapterId }
        };

        var chapterRegionBaseDataList = DataManager.GetChapterRegionData(chapterRegionSearchParameters);

        chapterRegionBaseDataList.ForEach(chapterRegion =>
        {
            CopyRegionData(elementData, chapterRegion, dataRequest);
        });
    }

    public static void AddWorldInteractableData(PhaseElementData phaseElementData, DataRequest dataRequest)
    {
        //Chapter
        var chapterSearchParameters = new Search.Chapter()
        {
            id = new List<int>() { phaseElementData.ChapterId }
        };

        var chapterData = DataManager.GetChapterData(chapterSearchParameters).First();

        //Chapter interactables
        var chapterInteractableSearchParameters = new Search.ChapterInteractable()
        {
            chapterId = new List<int>() { chapterData.Id }
        };

        var chapterInteractableDataList = DataManager.GetChapterInteractableData(chapterInteractableSearchParameters);

        chapterInteractableDataList.ForEach(chapterInteractableData =>
        {
            var worldInteractableElementData = new WorldInteractableElementData()
            {
                PhaseId = phaseElementData.Id,

                ChapterInteractableId = chapterInteractableData.Id,
                InteractableId = chapterInteractableData.InteractableId,

                Type = (int)Enums.InteractableType.Agent
            };

            worldInteractableElementData.Add(dataRequest);
        });
    }

    public static void CopyRegionData(PhaseElementData phaseElementData, ChapterRegionBaseData chapterRegionData, DataRequest dataRequest)
    {
        dataRequest.includeDependencies = false;

        //Region
        var regionSearchParameters = new Search.Region()
        {
            id = new List<int>() { chapterRegionData.RegionId }
        };

        var regionDataSource = DataManager.GetRegionData(regionSearchParameters).First();

        var tileSetSearchParameters = new Search.TileSet()
        {
            id = new List<int>() { regionDataSource.TileSetId }
        };

        var tileSetData = DataManager.GetTileSetData(tileSetSearchParameters).First();

        var regionElementData = new RegionElementData()
        {
            PhaseId = phaseElementData.Id,
            ChapterRegionId = chapterRegionData.Id,

            TileSetId = regionDataSource.TileSetId,

            Name = regionDataSource.Name,
            RegionSize = regionDataSource.RegionSize,
            TerrainSize = regionDataSource.TerrainSize,

            TileSize = tileSetData.TileSize
        };

        regionElementData.Add(dataRequest);
        
        UpdateDefaultRegion(phaseElementData, dataRequest);

        //Terrain
        var terrainSearchParameters = new Search.Terrain()
        {
            regionId = new List<int>() { regionDataSource.Id }
        };

        var terrainDataSourceList = DataManager.GetTerrainData(terrainSearchParameters);

        terrainDataSourceList.ForEach(terrainDataSource =>
        {
            var terrainElementData = new TerrainElementData()
            {
                RegionId = regionElementData.Id,

                Index = terrainDataSource.Index,

                IconId = terrainDataSource.IconId,
                Name = terrainDataSource.Name
            };

            terrainElementData.Add(dataRequest);

            regionElementData.TerrainDataList.Add(terrainElementData);

            //TerrainTile
            var terrainTileSearchParameters = new Search.TerrainTile()
            {
                terrainId = new List<int>() { terrainDataSource.Id }
            };

            var terrainTileDataSourceList = DataManager.GetTerrainTileData(terrainTileSearchParameters);

            terrainTileDataSourceList.ForEach(terrainTileDataSource =>
            {
                var terrainTileElementData = new TerrainTileElementData()
                {
                    TerrainId = terrainElementData.Id,

                    Index = terrainTileDataSource.Index,

                    TileId = terrainTileDataSource.TileId
                };

                terrainTileElementData.Add(dataRequest);
                
                regionElementData.TerrainDataList.Where(x => x.Id == terrainTileElementData.TerrainId).First().TerrainTileDataList.Add(terrainTileElementData);
            });

            //Atmosphere
            var atmosphereSearchParameters = new Search.Atmosphere()
            {
                terrainId = new List<int>() { terrainDataSource.Id }
            };

            var atmosphereDataSourceList = DataManager.GetAtmosphereData(atmosphereSearchParameters);

            atmosphereDataSourceList.ForEach(atmosphereDataSource =>
            {
                var atmosphereElementData = new AtmosphereElementData()
                {
                    TerrainId = terrainElementData.Id,

                    Default = atmosphereDataSource.Default,

                    StartTime = atmosphereDataSource.StartTime,
                    EndTime = atmosphereDataSource.EndTime
                };

                atmosphereElementData.Add(dataRequest);
            });
        });

        //World object
        var worldObjectSearchParameters = new Search.WorldObject()
        {
            regionId = new List<int>() { regionDataSource.Id }
        };

        var worldObjectDataSourceList = DataManager.GetWorldObjectData(worldObjectSearchParameters);

        worldObjectDataSourceList.ForEach(worldObjectDataSource =>
        {
            var worldObjectElementData = new WorldObjectElementData()
            {
                RegionId = regionElementData.Id,

                PositionX = worldObjectDataSource.PositionX,
                PositionY = worldObjectDataSource.PositionY,
                PositionZ = worldObjectDataSource.PositionZ,

                TerrainId = RegionManager.GetTerrainId(regionElementData, regionElementData.TerrainDataList.Cast<TerrainBaseData>().ToList(), regionElementData.TileSize, worldObjectDataSource.PositionX, worldObjectDataSource.PositionZ),
                TerrainTileId = RegionManager.GetTerrainTileId(regionElementData, worldObjectDataSource.PositionX, worldObjectDataSource.PositionZ),

                RotationX = worldObjectDataSource.RotationX,
                RotationY = worldObjectDataSource.RotationY,
                RotationZ = worldObjectDataSource.RotationZ,

                Scale = worldObjectDataSource.Scale,

                ModelId = worldObjectDataSource.ModelId
            };

            worldObjectElementData.Add(dataRequest);
        });

        //World interactable, task, interaction & interaction destination

        //Id groups for elements which need to be re-assigned to other elements after everything has been created
        var worldInteractableIdGroupList    = new List<Tuple<int, int>>().Select(x => new { originalId = x.Item1, newId = x.Item2 }).ToList();
        var sceneActorIdGroupList           = new List<Tuple<int, int>>().Select(x => new { originalId = x.Item1, newId = x.Item2 }).ToList();

        var sceneActorElementDataList   = new List<SceneActorElementData>();
        var sceneShotElementDataList    = new List<SceneShotElementData>();

        //Get all world interactables belonging to this region source based on the interaction destinations

        //Interaction destination
        var interactionDestinationSearchParameters = new Search.InteractionDestination()
        {
            regionId = new List<int>() { regionDataSource.Id }
        };

        var interactionDestinationDataSourceList = DataManager.GetInteractionDestinationData(interactionDestinationSearchParameters);

        if (interactionDestinationDataSourceList.Count == 0) return;

        //Interaction
        var interactionSearchParameters = new Search.Interaction()
        {
            id = interactionDestinationDataSourceList.Select(x => x.InteractionId).ToList()
        };

        var interactionDataSourceList = DataManager.GetInteractionData(interactionSearchParameters);

        //Task
        var taskSearchParameters = new Search.Task()
        {
            id = interactionDataSourceList.Select(x => x.TaskId).ToList()
        };

        var taskDataSourceList = DataManager.GetTaskData(taskSearchParameters);

        //World interactable
        var worldInteractableSearchParameters = new Search.WorldInteractable()
        {
            id = taskDataSourceList.Select(x => x.WorldInteractableId).ToList()
        };

        var worldInteractableDataSourceList = DataManager.GetWorldInteractableData(worldInteractableSearchParameters);

        worldInteractableDataSourceList.ForEach(worldInteractableDataSource =>
        {
            var worldInteractableElementData = new WorldInteractableElementData()
            {
                InteractableId = worldInteractableDataSource.InteractableId,

                Type = worldInteractableDataSource.Type
            };

            worldInteractableElementData.Add(dataRequest);

            worldInteractableIdGroupList.Add(new { originalId = worldInteractableDataSource.Id, newId = worldInteractableElementData.Id });

            //Task
            taskDataSourceList.Where(x => x.WorldInteractableId == worldInteractableDataSource.Id).ToList().ForEach(taskDataSource =>
            {
                var taskElementData = new TaskElementData()
                {
                    Index = taskDataSource.Index,

                    WorldInteractableId = worldInteractableElementData.Id,

                    Name = taskDataSource.Name
                };

                taskElementData.Add(dataRequest);

                //Interaction
                interactionDataSourceList.Where(x => x.TaskId == taskDataSource.Id).ToList().ForEach(interactionDataSource =>
                {
                    var interactionElementData = new InteractionElementData()
                    {
                        TaskId = taskElementData.Id,

                        Default = interactionDataSource.Default,

                        StartTime = interactionDataSource.StartTime,
                        EndTime = interactionDataSource.EndTime,

                        ArrivalType = interactionDataSource.ArrivalType,

                        TriggerAutomatically = interactionDataSource.TriggerAutomatically,
                        BeNearDestination = interactionDataSource.BeNearDestination,
                        FaceInteractable = interactionDataSource.FaceInteractable,
                        FaceControllable = interactionDataSource.FaceControllable,
                        HideInteractionIndicator = interactionDataSource.HideInteractionIndicator,

                        InteractionRange = interactionDataSource.InteractionRange,

                        DelayMethod = interactionDataSource.DelayMethod,
                        DelayDuration = interactionDataSource.DelayDuration,
                        HideDelayIndicator = interactionDataSource.HideDelayIndicator,

                        CancelDelayOnInput = interactionDataSource.CancelDelayOnInput,
                        CancelDelayOnMovement = interactionDataSource.CancelDelayOnMovement,
                        CancelDelayOnHit = interactionDataSource.CancelDelayOnHit,

                        PublicNotes = interactionDataSource.PublicNotes,
                        PrivateNotes = interactionDataSource.PrivateNotes
                    };

                    interactionElementData.Add(dataRequest);

                    //Interaction destination
                    interactionDestinationDataSourceList.Where(x => x.InteractionId == interactionDataSource.Id).ToList().ForEach(interactionDestinationDataSource =>
                    {
                        var interactionDestinationElementData = new InteractionDestinationElementData()
                        {
                            InteractionId = interactionElementData.Id,

                            PositionX = interactionDestinationDataSource.PositionX,
                            PositionY = interactionDestinationDataSource.PositionY,
                            PositionZ = interactionDestinationDataSource.PositionZ,

                            PositionVariance = interactionDestinationDataSource.PositionVariance,

                            RegionId = regionElementData.Id,

                            TerrainId = RegionManager.GetTerrainId(regionElementData, regionElementData.TerrainDataList.Cast<TerrainBaseData>().ToList(), regionElementData.TileSize, interactionDestinationDataSource.PositionX, interactionDestinationDataSource.PositionZ),
                            TerrainTileId = RegionManager.GetTerrainTileId(regionElementData, interactionDestinationDataSource.PositionX, interactionDestinationDataSource.PositionZ),

                            ChangeRotation = interactionDestinationDataSource.ChangeRotation,

                            RotationX = interactionDestinationDataSource.RotationX,
                            RotationY = interactionDestinationDataSource.RotationY,
                            RotationZ = interactionDestinationDataSource.RotationZ,

                            Animation = interactionDestinationDataSource.Animation,
                            Patience = interactionDestinationDataSource.Patience
                        };

                        interactionDestinationElementData.Add(dataRequest);
                    });

                    //Outcome
                    var outcomeSearchParameters = new Search.Outcome()
                    {
                        interactionId = new List<int>() { interactionDataSource.Id }
                    };

                    var outcomeDataSourceList = DataManager.GetOutcomeData(outcomeSearchParameters);

                    outcomeDataSourceList.ForEach(outcomeDataSource =>
                    {
                        var outcomeElementData = new OutcomeElementData()
                        {
                            InteractionId = interactionElementData.Id,

                            Type = outcomeDataSource.Type,

                            CompleteTask = outcomeDataSource.CompleteTask,
                            ResetObjective = outcomeDataSource.ResetObjective,

                            CancelScenarioType = outcomeDataSource.CancelScenarioType,
                            CancelScenarioOnInteraction = outcomeDataSource.CancelScenarioOnInteraction,
                            CancelScenarioOnInput = outcomeDataSource.CancelScenarioOnInput,
                            CancelScenarioOnRange = outcomeDataSource.CancelScenarioOnRange,
                            CancelScenarioOnHit = outcomeDataSource.CancelScenarioOnHit,

                            PublicNotes = outcomeDataSource.PublicNotes,
                            PrivateNotes = outcomeDataSource.PrivateNotes
                        };

                        outcomeElementData.Add(dataRequest);

                        //Scene
                        var sceneSearchParameters = new Search.Scene()
                        {
                            outcomeId = new List<int>() { outcomeDataSource.Id }
                        };

                        var sceneDataSourceList = DataManager.GetSceneData(sceneSearchParameters);

                        sceneDataSourceList.ForEach(sceneDataSource =>
                        {
                            var sceneElementData = new SceneElementData()
                            {
                                OutcomeId = outcomeElementData.Id,
                                RegionId = regionElementData.Id,

                                Index = sceneDataSource.Index,

                                Name = sceneDataSource.Name,

                                FreezeTime = sceneDataSource.FreezeTime,
                                AutoContinue = sceneDataSource.AutoContinue,
                                SetActorsInstantly = sceneDataSource.SetActorsInstantly,

                                SceneDuration = sceneDataSource.SceneDuration,
                                ShotDuration = sceneDataSource.ShotDuration,

                                PublicNotes = sceneDataSource.PublicNotes,
                                PrivateNotes = sceneDataSource.PrivateNotes
                            };

                            sceneElementData.Add(dataRequest);

                            //Scene shot
                            var sceneShotSearchParameters = new Search.SceneShot()
                            {
                                sceneId = new List<int>() { sceneDataSource.Id }
                            };

                            var sceneShotDataSourceList = DataManager.GetSceneShotData(sceneShotSearchParameters);

                            sceneShotDataSourceList.ForEach(sceneShotDataSource =>
                            {
                                var sceneShotElementData = new SceneShotElementData()
                                {
                                    SceneId = sceneElementData.Id,

                                    Type = sceneShotDataSource.Type,

                                    ChangePosition = sceneShotDataSource.ChangePosition,

                                    PositionX = sceneShotDataSource.PositionX,
                                    PositionY = sceneShotDataSource.PositionY,
                                    PositionZ = sceneShotDataSource.PositionZ,

                                    PositionTargetSceneActorId = sceneShotDataSource.PositionTargetSceneActorId,

                                    ChangeRotation = sceneShotDataSource.ChangeRotation,

                                    RotationX = sceneShotDataSource.RotationX,
                                    RotationY = sceneShotDataSource.RotationY,
                                    RotationZ = sceneShotDataSource.RotationZ,

                                    RotationTargetSceneActorId = sceneShotDataSource.RotationTargetSceneActorId,

                                    CameraFilterId = sceneShotDataSource.CameraFilterId
                                };

                                sceneShotElementData.Add(dataRequest);

                                sceneShotElementDataList.Add(sceneShotElementData);
                            });

                            //Scene actor
                            var sceneActorSearchParameters = new Search.SceneActor()
                            {
                                sceneId = new List<int>() { sceneDataSource.Id }
                            };

                            var sceneActorDataSourceList = DataManager.GetSceneActorData(sceneActorSearchParameters);

                            sceneActorDataSourceList.ForEach(sceneActorDataSource =>
                            {
                                var sceneActorElementData = new SceneActorElementData()
                                {
                                    SceneId = sceneElementData.Id,

                                    //Ids which refer to other copied data is marked by the source id and replaced at the end
                                    WorldInteractableId = worldInteractableDataSource.Id,

                                    ChangePosition = sceneActorDataSource.ChangePosition,
                                    FreezePosition = sceneActorDataSource.FreezePosition,

                                    SpeechMethod = sceneActorDataSource.SpeechMethod,
                                    SpeechText = sceneActorDataSource.SpeechText,
                                    ShowTextBox = sceneActorDataSource.ShowTextBox,

                                    TargetSceneActorId = sceneActorDataSource.TargetSceneActorId,

                                    PositionX = sceneActorDataSource.PositionX,
                                    PositionY = sceneActorDataSource.PositionY,
                                    PositionZ = sceneActorDataSource.PositionZ,

                                    TerrainId = RegionManager.GetTerrainId(regionElementData, regionElementData.TerrainDataList.Cast<TerrainBaseData>().ToList(), regionElementData.TileSize, sceneActorDataSource.PositionX, sceneActorDataSource.PositionZ),
                                    TerrainTileId = RegionManager.GetTerrainTileId(regionElementData, sceneActorDataSource.PositionX, sceneActorDataSource.PositionZ),

                                    ChangeRotation = sceneActorDataSource.ChangeRotation,
                                    FaceTarget = sceneActorDataSource.FaceTarget,

                                    RotationX = sceneActorDataSource.RotationX,
                                    RotationY = sceneActorDataSource.RotationY,
                                    RotationZ = sceneActorDataSource.RotationZ
                                };

                                sceneActorElementData.Add(dataRequest);

                                sceneActorIdGroupList.Add(new { originalId = sceneActorDataSource.Id, newId = sceneActorElementData.Id });
                                sceneActorElementDataList.Add(sceneActorElementData);
                            });

                            //Scene prop
                            var scenePropSearchParameters = new Search.SceneProp()
                            {
                                sceneId = new List<int>() { sceneDataSource.Id }
                            };

                            var scenePropDataSourceList = DataManager.GetScenePropData(scenePropSearchParameters);

                            scenePropDataSourceList.ForEach(scenePropDataSource =>
                            {
                                var scenePropElementData = new ScenePropElementData()
                                {
                                    SceneId = sceneElementData.Id,
                                    ModelId = scenePropDataSource.ModelId,

                                    PositionX = scenePropDataSource.PositionX,
                                    PositionY = scenePropDataSource.PositionY,
                                    PositionZ = scenePropDataSource.PositionZ,

                                    TerrainId = RegionManager.GetTerrainId(regionElementData, regionElementData.TerrainDataList.Cast<TerrainBaseData>().ToList(), regionElementData.TileSize, scenePropDataSource.PositionX, scenePropDataSource.PositionZ),
                                    TerrainTileId = RegionManager.GetTerrainTileId(regionElementData, scenePropDataSource.PositionX, scenePropDataSource.PositionZ),

                                    RotationX = scenePropDataSource.RotationX,
                                    RotationY = scenePropDataSource.RotationY,
                                    RotationZ = scenePropDataSource.RotationZ,

                                    Scale = scenePropDataSource.Scale
                                };

                                scenePropElementData.Add(dataRequest);
                            });
                        });
                    });
                });
            });
        });

        //Only perform during execution since elements do not have their original values set during validation
        //Original values can be set if necessary
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            //Replace old ids here at the end as new ids might not yet exist when elements are added
            sceneActorElementDataList.ForEach(sceneElementDataActor =>
            {
                sceneElementDataActor.WorldInteractableId = worldInteractableIdGroupList.Where(idGroup => idGroup.originalId == sceneElementDataActor.WorldInteractableId)
                                                                                        .Select(idGroup => idGroup.newId).First();

                if (sceneElementDataActor.TargetSceneActorId > 0)
                    sceneElementDataActor.TargetSceneActorId = sceneActorIdGroupList.Where(idGroup => idGroup.originalId == sceneElementDataActor.TargetSceneActorId)
                                                                                    .Select(idGroup => idGroup.newId).First();

                sceneElementDataActor.Update(dataRequest);
            });

            sceneShotElementDataList.ForEach(sceneShotElementData =>
            {
                if (sceneShotElementData.PositionTargetSceneActorId > 0)
                    sceneShotElementData.PositionTargetSceneActorId = sceneActorIdGroupList.Where(idGroup => idGroup.originalId == sceneShotElementData.PositionTargetSceneActorId)
                                                                                           .Select(idGroup => idGroup.newId).First();

                if (sceneShotElementData.RotationTargetSceneActorId > 0)
                    sceneShotElementData.RotationTargetSceneActorId = sceneActorIdGroupList.Where(idGroup => idGroup.originalId == sceneShotElementData.RotationTargetSceneActorId)
                                                                                           .Select(idGroup => idGroup.newId).First();

                sceneShotElementData.Update(dataRequest);
            });
        }
    }

    public static void UpdateDefaultRegion(PhaseElementData elementData, DataRequest dataRequest)
    {
        //After the region is added, get a list of all the phase regions. If the phase's default region is not listed, set the first region of the list as default at zero position
        var updateDefaultRegion = false;

        //Region
        var regionSearchParameters = new Search.Region()
        {
            phaseId = new List<int>() { elementData.Id }
        };

        var regionDataList = DataManager.GetRegionData(regionSearchParameters);

        if (regionDataList.Count == 0)
        {
            elementData.DefaultRegionId = 0;
            updateDefaultRegion = true;
            
        } else if (!regionDataList.Select(x => x.Id).Contains(elementData.DefaultRegionId)) {

            elementData.DefaultRegionId = regionDataList.First().Id;
            updateDefaultRegion = true;
        }

        if (updateDefaultRegion)
        {
            elementData.DefaultPositionX = 0;
            elementData.DefaultPositionY = 0;
            elementData.DefaultPositionZ = 0;

            elementData.DefaultRotationX = 0;
            elementData.DefaultRotationY = 0;
            elementData.DefaultRotationZ = 0;

            elementData.Update(dataRequest);
        }
    }

    public static void UpdateData(PhaseElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.phaseList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedName)
            {
                data.Name = elementData.Name;
            }

            if (elementData.ChangedDefaultRegionId)
            {
                data.DefaultRegionId = elementData.DefaultRegionId;
            }

            if (elementData.ChangedDefaultPositionX)
            {
                data.DefaultPositionX = elementData.DefaultPositionX;
            }

            if (elementData.ChangedDefaultPositionY)
            {
                data.DefaultPositionY = elementData.DefaultPositionY;
            }

            if (elementData.ChangedDefaultPositionZ)
            {
                data.DefaultPositionZ = elementData.DefaultPositionZ;
            }

            if (elementData.ChangedDefaultRotationX)
            {
                data.DefaultRotationX = elementData.DefaultRotationX;
            }

            if (elementData.ChangedDefaultRotationY)
            {
                data.DefaultRotationY = elementData.DefaultRotationY;
            }

            if (elementData.ChangedDefaultRotationZ)
            {
                data.DefaultRotationZ = elementData.DefaultRotationZ;
            }

            if (elementData.ChangedDefaultTime)
            {
                data.DefaultTime = elementData.DefaultTime;
            }

            if (elementData.ChangedPublicNotes)
            {
                data.PublicNotes = elementData.PublicNotes;
            }

            if (elementData.ChangedPrivateNotes)
            {
                data.PrivateNotes = elementData.PrivateNotes;
            }

            elementData.SetOriginalValues();

        } else { }    
    }

    static public void UpdateIndex(PhaseElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.ChangedIndex) return;

        var data = Fixtures.phaseList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            data.Index = elementData.Index;

            elementData.OriginalData.Index = elementData.Index;

            SaveDataManager.UpdateReferences(dataRequest);

        } else { }
    }

    static public void RemoveData(PhaseElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            RemoveDependencies(elementData, dataRequest);

            Fixtures.phaseList.RemoveAll(x => x.Id == elementData.Id);

            elementData.RemoveIndex(dataRequest);

            SaveDataManager.UpdateReferences(dataRequest);

        } else {

            RemoveDependencies(elementData, dataRequest);
        }
    }

    private static void RemoveDependencies(PhaseElementData elementData, DataRequest dataRequest)
    {
        RemoveRegionData(elementData, dataRequest);

        RemoveQuestData(elementData, dataRequest);
        RemovePhaseSaveData(elementData, dataRequest);
    }

    private static void RemoveRegionData(PhaseElementData elementData, DataRequest dataRequest)
    {
        var regionSearchParameters = new Search.Region()
        {
            phaseId = new List<int>() { elementData.Id }
        };

        var regionDataList = DataManager.GetRegionData(regionSearchParameters);

        regionDataList.ForEach(questData =>
        {
            var regionElementData = new RegionElementData()
            {
                Id = questData.Id
            };

            regionElementData.Remove(dataRequest);
        });
    }

    private static void RemoveQuestData(PhaseElementData elementData, DataRequest dataRequest)
    {
        var questSearchParameters = new Search.Quest()
        {
            phaseId = new List<int>() { elementData.Id }
        };

        var questDataList = DataManager.GetQuestData(questSearchParameters);

        questDataList.ForEach(questData =>
        {
            var questElementData = new QuestElementData()
            {
                Id = questData.Id
            };

            questElementData.Remove(dataRequest);
        });
    }

    private static void RemovePhaseSaveData(PhaseElementData elementData, DataRequest dataRequest)
    {
        var phaseSaveSearchParameters = new Search.PhaseSave()
        {
            phaseId = new List<int>() { elementData.Id }
        };

        var phaseSaveDataList = DataManager.GetPhaseSaveData(phaseSaveSearchParameters);

        phaseSaveDataList.ForEach(phaseSaveData =>
        {
            var phaseSaveElementData = new PhaseSaveElementData()
            {
                Id = phaseSaveData.Id
            };

            phaseSaveElementData.Remove(dataRequest);
        });
    }

    public static void RemoveIndex(PhaseElementData elementData, DataRequest dataRequest)
    {
        var phaseSearchParameters = new Search.Phase()
        {
            chapterId = new List<int>() { elementData.ChapterId }
        };

        var phaseDataList = DataManager.GetPhaseData(phaseSearchParameters);

        phaseDataList.Where(x => x.Index > elementData.Index).ToList().ForEach(phaseData =>
        {
            var phaseElementData = new PhaseElementData()
            {
                Id = phaseData.Id,
                Index = phaseData.Index
            };

            phaseElementData.SetOriginalValues();

            phaseElementData.Index--;

            phaseElementData.UpdateIndex(dataRequest);
        });
    }
}
