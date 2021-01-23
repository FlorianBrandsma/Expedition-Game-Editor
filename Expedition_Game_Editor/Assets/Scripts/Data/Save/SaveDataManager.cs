using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class SaveDataManager
{
    private static List<SaveBaseData> saveDataList;

    private static List<PlayerSaveBaseData> playerSaveDataList;

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

        GetPlayerSaveData();

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

                    join leftJoin in (from playerSaveData           in playerSaveDataList
                                      join worldInteractableData    in worldInteractableDataList    on playerSaveData.WorldInteractableId   equals worldInteractableData.Id
                                      join interactableData         in interactableDataList         on worldInteractableData.InteractableId equals interactableData.Id
                                      join modelData                in modelDataList                on interactableData.ModelId             equals modelData.Id
                                      join iconData                 in iconDataList                 on modelData.IconId                     equals iconData.Id

                                      join regionData               in regionDataList               on playerSaveData.RegionId              equals regionData.Id
                                      join tileSetData              in tileSetDataList              on regionData.TileSetId                 equals tileSetData.Id

                                      join phaseData                in phaseDataList                on regionData.PhaseId                   equals phaseData.Id
                                      join chapterData              in chapterDataList              on phaseData.ChapterId                  equals chapterData.Id
                                      select new { playerSaveData, iconData, chapterData, tileSetData, regionData }) on saveData.Id equals leftJoin.playerSaveData.SaveId into playerSaveData

                    select new SaveElementData()
                    {
                        Id = saveData.Id,

                        GameId = saveData.GameId,

                        ModelIconPath = playerSaveData.FirstOrDefault() != null ? playerSaveData.FirstOrDefault().iconData.Path : "",

                        Name = playerSaveData.FirstOrDefault() != null ? "Ch. " + (playerSaveData.FirstOrDefault().chapterData.Index + 1) + ": " + playerSaveData.FirstOrDefault().chapterData.Name : "",
                        LocationName = playerSaveData.FirstOrDefault() != null ? RegionManager.LocationName(playerSaveData.FirstOrDefault().playerSaveData.PositionX, 
                                                                                                            playerSaveData.FirstOrDefault().playerSaveData.PositionZ, 
                                                                                                            playerSaveData.FirstOrDefault().tileSetData.TileSize, 
                                                                                                            playerSaveData.FirstOrDefault().regionData, 
                                                                                                            terrainDataList) : "",

                        Time = playerSaveData.FirstOrDefault() != null ? TimeManager.TimeFromSeconds(playerSaveData.FirstOrDefault().playerSaveData.PlayedTime) : ""

                    }).OrderBy(x => x.Id).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static SaveElementData DefaultData(int gameId)
    {
        return new SaveElementData()
        {
            Id = -1,

            GameId = gameId
        };
    }

    public static void SetDefaultAddValues(List<SaveElementData> list)
    {
        var addElementData = list.Where(x => x.Id == -1).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;
    }

    private static void GetSaveData(Search.Save searchParameters)
    {
        saveDataList = DataManager.GetSaveData(searchParameters);
    }

    private static void GetPlayerSaveData()
    {
        var searchParameters = new Search.PlayerSave();
        searchParameters.saveId = saveDataList.Select(x => x.Id).Distinct().ToList();

        playerSaveDataList = DataManager.GetPlayerSaveData(searchParameters);
    }

    private static void GetWorldInteractableData()
    {
        var searchParameters = new Search.WorldInteractable();
        searchParameters.id = playerSaveDataList.Select(x => x.WorldInteractableId).Distinct().ToList();

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
        searchParameters.id = playerSaveDataList.Select(x => x.RegionId).Distinct().ToList();

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
        AddPlayerSaveData(elementData, dataRequest);
        AddInteractableSaveData(elementData, dataRequest);
        AddChapterSaveData(elementData, dataRequest);
    }

    private static void AddPlayerSaveData(SaveElementData elementData, DataRequest dataRequest)
    {
        var playerSaveElementData = PlayerSaveDataManager.DefaultData(elementData.Id, elementData.GameId);
        playerSaveElementData.Add(dataRequest);
    }

    private static void AddInteractableSaveData(SaveElementData elementData, DataRequest dataRequest)
    {
        //Interactable
        var interactableSearchParameters = new Search.Interactable()
        {
            //gameId
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
        //Chapter
        var chapterSearchParameters = new Search.Chapter()
        {
            //gameId
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
    }
}
