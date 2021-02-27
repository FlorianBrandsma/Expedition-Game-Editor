using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class SaveDataManager
{
    private static List<SaveBaseData> saveDataList;
    
    private static List<WorldInteractableBaseData> worldInteractableDataList;
    private static List<InteractableBaseData> interactableDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    private static List<RegionBaseData> regionDataList;
    private static List<TerrainBaseData> terrainDataList;
    private static List<TileSetBaseData> tileSetDataList;

    private static List<PhaseBaseData> phaseDataList;
    private static List<ChapterBaseData> chapterDataList;

    public static List<IElementData> GetData(Search.Save searchParameters)
    {
        GetSaveData(searchParameters);

        if (searchParameters.includeAddElement)
            saveDataList.Add(DefaultData(searchParameters.gameId.First()));

        if (saveDataList.Count == 0) return new List<IElementData>();

        GetWorldInteractableData();
        GetInteractableData();
        GetModelData();
        GetIconData();

        GetRegionData();
        GetTerrainData();
        GetTileSetData();

        GetPhaseData();
        GetChapterData();

        var list = (from saveData               in saveDataList
                    join worldInteractableData  in worldInteractableDataList    on saveData.WorldInteractableId         equals worldInteractableData.Id
                    join interactableData       in interactableDataList         on worldInteractableData.InteractableId equals interactableData.Id
                    join modelData              in modelDataList                on interactableData.ModelId             equals modelData.Id
                    join iconData               in iconDataList                 on modelData.IconId                     equals iconData.Id

                    join regionData             in regionDataList               on saveData.RegionId                    equals regionData.Id
                    join tileSetData            in tileSetDataList              on regionData.TileSetId                 equals tileSetData.Id

                    join phaseData              in phaseDataList                on regionData.PhaseId                   equals phaseData.Id
                    join chapterData            in chapterDataList              on phaseData.ChapterId                  equals chapterData.Id
                                      
                    select new SaveElementData()
                    {
                        Id = saveData.Id,

                        GameId = saveData.GameId,
                        RegionId = saveData.RegionId,
                        WorldInteractableId = saveData.WorldInteractableId,

                        Index = saveData.Index,

                        PositionX = saveData.PositionX,
                        PositionY = saveData.PositionY,
                        PositionZ = saveData.PositionZ,

                        GameTime = saveData.GameTime,
                        PlayTime = saveData.PlayTime,
                        SaveTime = saveData.SaveTime,
                        
                        SaveType = searchParameters.saveType,

                        ChapterName = "Chapter " + (chapterData.Index + 1) + ": " + chapterData.Name,
                        LocationName = RegionManager.LocationName(saveData.PositionX,
                                                                saveData.PositionZ, 
                                                                tileSetData.TileSize, 
                                                                regionData, 
                                                                terrainDataList),

                        InteractableName = interactableData.Name,
                        ModelIconPath = iconData.Path,

                        PhaseName = phaseData.Name,
                        PhaseGameNotes = phaseData.GameNotes

                        //Time = playerSaveData.FirstOrDefault() != null ? TimeManager.TimeFromSeconds(playerSaveData.FirstOrDefault().playerSaveData.PlayedTime) : ""

                    }).OrderBy(x => x.Id).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static SaveElementData DefaultData(int gameId)
    {
        //Game
        var gameSearchParameters = new Search.Game()
        {
            id = new List<int>() { gameId }
        };

        var gameData = DataManager.GetGameData(gameSearchParameters).First();

        //Project
        var projectSearchParameters = new Search.Project()
        {
            id = new List<int>() { gameData.Id }
        };

        var projectData = DataManager.GetProjectData(projectSearchParameters).First();

        //Chapter
        var chapterSearchParameters = new Search.Chapter()
        {
            id = new List<int>() { projectData.Id }
        };

        var chapterData = DataManager.GetChapterData(chapterSearchParameters).Where(x => x.Index == 0).First();

        //Phase
        var phaseSearchParameters = new Search.Phase()
        {
            chapterId = new List<int>() { chapterData.Id }
        };

        var phaseData = DataManager.GetPhaseData(phaseSearchParameters).Where(x => x.Index == 0).First();

        //World interactable
        var worldInteractableSearchParameters = new Search.WorldInteractable()
        {
            chapterId = new List<int>() { chapterData.Id }
        };

        var worldInteractableData = DataManager.GetWorldInteractableData(worldInteractableSearchParameters).First();

        return new SaveElementData()
        {
            Id = -1,

            GameId = gameId,
            RegionId = phaseData.DefaultRegionId,
            WorldInteractableId = worldInteractableData.Id,

            PositionX = phaseData.DefaultPositionX,
            PositionY = phaseData.DefaultPositionY,
            PositionZ = phaseData.DefaultPositionZ,

            GameTime = phaseData.DefaultTime
        };
    }

    public static void SetDefaultAddValues(List<SaveElementData> list)
    {
        var addElementData = list.Where(x => x.Id == -1).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;

        addElementData.Index = list.Count - 1;
    }

    private static void GetSaveData(Search.Save searchParameters)
    {
        saveDataList = DataManager.GetSaveData(searchParameters);
    }

    private static void GetWorldInteractableData()
    {
        var searchParameters = new Search.WorldInteractable();
        searchParameters.id = saveDataList.Select(x => x.WorldInteractableId).Distinct().ToList();

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
        searchParameters.id = saveDataList.Select(x => x.RegionId).Distinct().ToList();

        regionDataList = DataManager.GetRegionData(searchParameters);
    }

    private static void GetTerrainData()
    {
        var searchParameters = new Search.Terrain();
        searchParameters.regionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        terrainDataList = DataManager.GetTerrainData(searchParameters);
    }

    private static void GetTileSetData()
    {
        var searchParameters = new Search.TileSet();
        searchParameters.id = regionDataList.Select(x => x.TileSetId).Distinct().ToList();

        tileSetDataList = DataManager.GetTileSetData(searchParameters);
    }

    private static void GetPhaseData()
    {
        var searchParameters = new Search.Phase();
        searchParameters.id = regionDataList.Select(x => x.PhaseId).Distinct().ToList();

        phaseDataList = DataManager.GetPhaseData(searchParameters);
    }

    private static void GetChapterData()
    {
        var searchParameters = new Search.Chapter();
        searchParameters.id = phaseDataList.Select(x => x.ChapterId).Distinct().ToList();

        chapterDataList = DataManager.GetChapterData(searchParameters);
    }

    public static void AddData(SaveElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.saveList.Count > 0 ? (Fixtures.saveList[Fixtures.saveList.Count - 1].Id + 1) : 1;
            Fixtures.saveList.Add(((SaveData)elementData).Clone());

            elementData.SetOriginalValues();

            AddDependencies(elementData, dataRequest);

        } else { }
    }

    private static void AddDependencies(SaveElementData elementData, DataRequest dataRequest)
    {
        AddInteractableSaveData(elementData, dataRequest);
        AddChapterSaveData(elementData, dataRequest);
    }

    private static void AddInteractableSaveData(SaveElementData elementData, DataRequest dataRequest)
    {
        //Game
        var gameSearchParameters = new Search.Game()
        {
            id = new List<int>() { elementData.GameId }
        };

        var gameData = DataManager.GetGameData(gameSearchParameters).First();

        //Project
        var projectSearchParameters = new Search.Project()
        {
            id = new List<int>() { gameData.Id }
        };

        var projectData = DataManager.GetProjectData(projectSearchParameters).First();

        //Interactable
        var interactableSearchParameters = new Search.Interactable()
        {
            projectId = new List<int>() { projectData.Id }
        };

        var interactableDataList = DataManager.GetInteractableData(interactableSearchParameters);

        interactableDataList.ForEach(interactableData =>
        {
            var interactableSaveElementData = InteractableSaveDataManager.DefaultData(elementData.Id, interactableData.Id);
            interactableSaveElementData.Add(dataRequest);
        });
    }

    private static void AddChapterSaveData(SaveElementData elementData, DataRequest dataRequest)
    {
        //Game
        var gameSearchParameters = new Search.Game()
        {
            id = new List<int>() { elementData.GameId }
        };

        var gameData = DataManager.GetGameData(gameSearchParameters).First();

        //Project
        var projectSearchParameters = new Search.Project()
        {
            id = new List<int>() { gameData.Id }
        };

        var projectData = DataManager.GetProjectData(projectSearchParameters).First();

        //Chapter
        var chapterSearchParameters = new Search.Chapter()
        {
            projectId = new List<int>() { projectData.Id }
        };

        var chapterDataList = DataManager.GetChapterData(chapterSearchParameters);

        chapterDataList.ForEach(chapterData =>
        {
            var chapterSaveElementData = ChapterSaveDataManager.DefaultData(elementData.Id, chapterData.Id);
            chapterSaveElementData.Add(dataRequest);
        });

        AddPhaseSaveData(elementData, chapterDataList, dataRequest);
    }

    private static void AddPhaseSaveData(SaveElementData elementData, List<ChapterBaseData> chapterDataList, DataRequest dataRequest)
    {
        var phaseSearchParameters = new Search.Phase()
        {
            chapterId = chapterDataList.Select(x => x.Id).ToList()
        };

        var phaseDataList = DataManager.GetPhaseData(phaseSearchParameters);

        phaseDataList.ForEach(phaseData =>
        {
            var phaseSaveElementData = PhaseSaveDataManager.DefaultData(elementData.Id, phaseData.Id);
            phaseSaveElementData.Add(dataRequest);
        });

        AddQuestSaveData(elementData, phaseDataList, dataRequest);
        AddInteractionSaveData(elementData, phaseDataList, dataRequest);
    }

    private static void AddQuestSaveData(SaveElementData elementData, List<PhaseBaseData> phaseDataList, DataRequest dataRequest)
    {
        var questSearchParameters = new Search.Quest()
        {
            phaseId = phaseDataList.Select(x => x.Id).ToList()
        };

        var questDataList = DataManager.GetQuestData(questSearchParameters);

        questDataList.ForEach(questData =>
        {
            var questSaveElementData = QuestSaveDataManager.DefaultData(elementData.Id, questData.Id);
            questSaveElementData.Add(dataRequest);
        });

        AddObjectiveSaveData(elementData, questDataList, dataRequest);
    }

    private static void AddObjectiveSaveData(SaveElementData elementData, List<QuestBaseData> questDataList, DataRequest dataRequest)
    {
        var objectiveSearchParameters = new Search.Objective()
        {
            questId = questDataList.Select(x => x.Id).ToList()
        };

        var objectiveDataList = DataManager.GetObjectiveData(objectiveSearchParameters);

        objectiveDataList.ForEach(objectiveData =>
        {
            var objectiveSaveElementData = ObjectiveSaveDataManager.DefaultData(elementData.Id, objectiveData.Id);
            objectiveSaveElementData.Add(dataRequest);
        });
    }

    private static void AddInteractionSaveData(SaveElementData elementData, List<PhaseBaseData> phaseDataList, DataRequest dataRequest)
    {
        //Region
        var regionSearchParameters = new Search.Region()
        {
            phaseId = phaseDataList.Select(x => x.Id).ToList()
        };

        var regionDataList = DataManager.GetRegionData(regionSearchParameters);

        //Interaction destination
        var interactionDestinationSearchParameters = new Search.InteractionDestination()
        {
            regionId = regionDataList.Select(x => x.Id).ToList()
        };

        var interactionDestinationDataList = DataManager.GetInteractionDestinationData(interactionDestinationSearchParameters);

        //Interaction
        var interactionSearchParameters = new Search.Interaction()
        {
            id = interactionDestinationDataList.Select(x => x.InteractionId).ToList()
        };

        var interactionDataList = DataManager.GetInteractionData(interactionSearchParameters);

        interactionDataList.ForEach(interactionData =>
        {
            var interactionSaveElementData = InteractionSaveDataManager.DefaultData(elementData.Id, interactionData.Id);
            interactionSaveElementData.Add(dataRequest);
        });

        AddTaskSaveData(elementData, interactionDataList, dataRequest);
    }

    private static void AddTaskSaveData(SaveElementData elementData, List<InteractionBaseData> interactionDataList, DataRequest dataRequest)
    {
        var taskSearchParameters = new Search.Task()
        {
            id = interactionDataList.Select(x => x.TaskId).ToList()
        };

        var taskDataList = DataManager.GetTaskData(taskSearchParameters);

        taskDataList.ForEach(taskData =>
        {
            var taskSaveElementData = TaskSaveDataManager.DefaultData(elementData.Id, taskData.Id, taskData.WorldInteractableId);
            taskSaveElementData.Add(dataRequest);
        });
    }

    public static void UpdateData(SaveElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.saveList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedRegionId)
            {
                data.RegionId = elementData.RegionId;
            }

            if (elementData.ChangedWorldInteractableId)
            {
                data.WorldInteractableId = elementData.WorldInteractableId;
            }

            if (elementData.ChangedPositionX)
            {
                data.PositionX = elementData.PositionX;
            }

            if (elementData.ChangedPositionY)
            {
                data.PositionY = elementData.PositionY;
            }

            if (elementData.ChangedPositionZ)
            {
                data.PositionZ = elementData.PositionZ;
            }

            if (elementData.ChangedGameTime)
            {
                data.GameTime = elementData.GameTime;
            }

            if (elementData.ChangedPlayTime)
            {
                data.PlayTime = elementData.PlayTime;
            }

            if (elementData.ChangedSaveTime)
            {
                data.SaveTime = elementData.SaveTime;
            }

            elementData.SetOriginalValues();

        } else { }
    }

    static public void UpdateIndex(SaveElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.ChangedIndex) return;

        var data = Fixtures.saveList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            data.Index = elementData.Index;

            elementData.OriginalData.Index = elementData.Index;

        } else { }
    }

    public static void UpdateReferences(DataRequest dataRequest)
    {
        //Save
        var saveSearchParameters = new Search.Save();

        var saveDataList = DataManager.GetSaveData(saveSearchParameters);

        if (saveDataList.Count == 0) return;

        foreach (SaveBaseData saveData in saveDataList)
        {
            //ChapterSave
            var chapterSaveSearchParameters = new Search.ChapterSave()
            {
                saveId = new List<int>() { saveData.Id },
                complete = false
            };

            //Get data from chapter save data manager so the elements are sorted by index
            var chapterSaveElementData = (ChapterSaveElementData)ChapterSaveDataManager.GetData(chapterSaveSearchParameters).First();

            if (chapterSaveElementData == null) continue;

            //PhaseSave
            var phaseSaveSearchParameters = new Search.PhaseSave()
            {
                saveId = saveDataList.Select(x => x.Id).ToList(),
                chapterId = new List<int>() { chapterSaveElementData.ChapterId },
                complete = false
            };

            var phaseSaveElementData = (PhaseSaveElementData)PhaseSaveDataManager.GetData(phaseSaveSearchParameters).First();

            //Region
            var regionSearchParameters = new Search.Region()
            {
                phaseId = new List<int>() { phaseSaveElementData.Id }
            };

            var regionDataList = DataManager.GetRegionData(regionSearchParameters);

            //World interactable
            var worldInteractableSearchParameters = new Search.WorldInteractable()
            {
                chapterId = new List<int>() { chapterSaveElementData.ChapterId }
            };

            var worldInteractableDataList = DataManager.GetWorldInteractableData(worldInteractableSearchParameters);

            var saveElementData = new SaveElementData()
            {
                Id = saveData.Id,
                RegionId = saveData.RegionId,
                WorldInteractableId = saveData.WorldInteractableId,

                PositionX = saveData.PositionX,
                PositionY = saveData.PositionY,
                PositionZ = saveData.PositionZ,

                GameTime = saveData.GameTime
            };

            saveElementData.SetOriginalValues();

            //Pick first region if current saved region is not contained in list
            if (!regionDataList.Select(x => x.Id).Contains(saveData.RegionId))
            {
                //Phase
                var phaseSeachParameters = new Search.Phase()
                {
                    id = new List<int>() { phaseSaveElementData.PhaseId }
                };

                var phaseData = DataManager.GetPhaseData(phaseSeachParameters).First();

                saveElementData.RegionId = regionDataList.First().Id;

                saveElementData.PositionX = phaseData.DefaultPositionX;
                saveElementData.PositionY = phaseData.DefaultPositionY;
                saveElementData.PositionZ = phaseData.DefaultPositionZ;

                saveElementData.GameTime = phaseData.DefaultTime;
            }

            var filteredWorldInteractableDataList = worldInteractableDataList.Where(x => x.ChapterId == chapterSaveElementData.ChapterId).ToList();

            //Pick first world interactable if current saved world interactable is not contained in list
            if (!filteredWorldInteractableDataList.Select(x => x.Id).Contains(saveData.WorldInteractableId))
                saveElementData.WorldInteractableId = filteredWorldInteractableDataList.Count > 0 ? filteredWorldInteractableDataList.First().Id : 0;

            saveElementData.Update(dataRequest);
        }
    }

    public static void RemoveData(SaveElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            RemoveDependencies(elementData, dataRequest);

            Fixtures.saveList.RemoveAll(x => x.Id == elementData.Id);

            elementData.RemoveIndex(dataRequest);

        } else {

            RemoveDependencies(elementData, dataRequest);
        }
    }

    private static void RemoveDependencies(SaveElementData elementData, DataRequest dataRequest)
    {
        RemoveInteractableSaveData(elementData, dataRequest);

        RemoveChapterSaveData(elementData, dataRequest);
        RemovePhaseSaveData(elementData, dataRequest);
        RemoveQuestSaveData(elementData, dataRequest);
        RemoveObjectiveSaveData(elementData, dataRequest);
        RemoveTaskSaveData(elementData, dataRequest);
        RemoveInteractionSaveData(elementData, dataRequest);
    }

    private static void RemoveInteractableSaveData(SaveElementData elementData, DataRequest dataRequest)
    {
        var interactableSaveSearchParameters = new Search.InteractableSave()
        {
            saveId = new List<int>() { elementData.Id }
        };

        var interactableSaveDataList = DataManager.GetInteractableSaveData(interactableSaveSearchParameters);

        interactableSaveDataList.ForEach(interactableSaveData =>
        {
            var interactableSaveElementData = new InteractableSaveElementData()
            {
                Id = interactableSaveData.Id
            };

            interactableSaveElementData.Remove(dataRequest);
        });
    }

    private static void RemoveChapterSaveData(SaveElementData elementData, DataRequest dataRequest)
    {
        var chapterSaveSearchParameters = new Search.ChapterSave()
        {
            saveId = new List<int>() { elementData.Id }
        };

        var chapterSaveDataList = DataManager.GetChapterSaveData(chapterSaveSearchParameters);

        chapterSaveDataList.ForEach(chapterSaveData =>
        {
            var chapterSaveElementData = new ChapterSaveElementData()
            {
                Id = chapterSaveData.Id
            };

            chapterSaveElementData.Remove(dataRequest);
        });
    }

    private static void RemovePhaseSaveData(SaveElementData elementData, DataRequest dataRequest)
    {
        var phaseSaveSearchParameters = new Search.PhaseSave()
        {
            saveId = new List<int>() { elementData.Id }
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

    private static void RemoveQuestSaveData(SaveElementData elementData, DataRequest dataRequest)
    {
        var questSaveSearchParameters = new Search.QuestSave()
        {
            saveId = new List<int>() { elementData.Id }
        };

        var questSaveDataList = DataManager.GetQuestSaveData(questSaveSearchParameters);

        questSaveDataList.ForEach(questSaveData =>
        {
            var questSaveElementData = new QuestSaveElementData()
            {
                Id = questSaveData.Id
            };

            questSaveElementData.Remove(dataRequest);
        });
    }

    private static void RemoveObjectiveSaveData(SaveElementData elementData, DataRequest dataRequest)
    {
        var objectiveSaveSearchParameters = new Search.ObjectiveSave()
        {
            saveId = new List<int>() { elementData.Id }
        };

        var objectiveSaveDataList = DataManager.GetObjectiveSaveData(objectiveSaveSearchParameters);

        objectiveSaveDataList.ForEach(objectiveSaveData =>
        {
            var objectiveSaveElementData = new ObjectiveSaveElementData()
            {
                Id = objectiveSaveData.Id
            };

            objectiveSaveElementData.Remove(dataRequest);
        });
    }

    private static void RemoveTaskSaveData(SaveElementData elementData, DataRequest dataRequest)
    {
        var taskSaveSearchParameters = new Search.TaskSave()
        {
            saveId = new List<int>() { elementData.Id }
        };

        var taskSaveDataList = DataManager.GetTaskSaveData(taskSaveSearchParameters);

        taskSaveDataList.ForEach(taskSaveData =>
        {
            var taskSaveElementData = new TaskSaveElementData()
            {
                Id = taskSaveData.Id
            };

            taskSaveElementData.Remove(dataRequest);
        });
    }

    private static void RemoveInteractionSaveData(SaveElementData elementData, DataRequest dataRequest)
    {
        var interactionSaveSearchParameters = new Search.InteractionSave()
        {
            saveId = new List<int>() { elementData.Id }
        };

        var interactionSaveDataList = DataManager.GetInteractionSaveData(interactionSaveSearchParameters);

        interactionSaveDataList.ForEach(interactionSaveData =>
        {
            var interactionSaveElementData = new InteractionSaveElementData()
            {
                Id = interactionSaveData.Id
            };

            interactionSaveElementData.Remove(dataRequest);
        });
    }

    public static void RemoveIndex(SaveElementData elementData, DataRequest dataRequest)
    {
        var saveSearchParameters = new Search.Save()
        {
            //Game id
        };

        var saveDataList = DataManager.GetSaveData(saveSearchParameters);

        saveDataList.Where(x => x.Index > elementData.Index).ToList().ForEach(saveData =>
        {
            var saveElementData = new SaveElementData()
            {
                Id = saveData.Id,
                Index = saveData.Index
            };

            saveElementData.SetOriginalValues();

            saveElementData.Index--;

            saveElementData.UpdateIndex(dataRequest);
        });
    }
}
