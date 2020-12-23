using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class GameSaveDataManager
{
    private static List<PlayerSaveBaseData> playerSaveDataList;
    private static List<ChapterSaveBaseData> chapterSaveDataList;
    private static List<PhaseSaveBaseData> phaseSaveDataList;
    private static List<QuestSaveBaseData> questSaveDataList;
    private static List<ObjectiveSaveBaseData> objectiveSaveDataList;
    private static List<TaskSaveBaseData> taskSaveDataList;
    private static List<InteractionSaveBaseData> interactionSaveDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.GameSave>().First();

        GetPlayerSaveData(searchParameters);
        
        if (playerSaveDataList.Count == 0) return new List<IElementData>();

        GetChapterSaveData(searchParameters);

        GetPhaseSaveData(searchParameters);
        GetQuestSaveData(searchParameters);
        GetObjectiveSaveData(searchParameters);
        GetTaskSaveData(searchParameters);
        GetInteractionSaveData(searchParameters);

        var gameSaveDataElement = new GameSaveElementData()
        {
            PlayerSaveData = (from playerSaveData in playerSaveDataList
                              select new PlayerSaveElementData
                              {
                                  Id = playerSaveData.Id,

                                  SaveId = playerSaveData.Id,
                                  RegionId = playerSaveData.RegionId,
                                  WorldInteractableId = playerSaveData.WorldInteractableId,

                                  PositionX = playerSaveData.PositionX,
                                  PositionY = playerSaveData.PositionY,
                                  PositionZ = playerSaveData.PositionZ,

                                  GameTime = playerSaveData.GameTime,
                                  PlayedTime = playerSaveData.PlayedTime

                              }).First(),

            ChapterSaveDataList = (from chapterSaveData in chapterSaveDataList
                                   select new ChapterSaveElementData
                                   {
                                       Id = chapterSaveData.Id,

                                       SaveId = chapterSaveData.SaveId,
                                       ChapterId = chapterSaveData.ChapterId,

                                       Complete = chapterSaveData.Complete

                                   }).ToList(),

            PhaseSaveDataList = (from phaseSaveData in phaseSaveDataList
                                 select new PhaseSaveElementData
                                 {
                                     Id = phaseSaveData.Id,

                                     SaveId = phaseSaveData.SaveId,
                                     ChapterSaveId = phaseSaveData.ChapterSaveId,
                                     PhaseId = phaseSaveData.PhaseId,

                                     Complete = phaseSaveData.Complete

                                 }).ToList(),

            QuestSaveDataList = (from questSaveData in questSaveDataList
                                 select new QuestSaveElementData
                                 {
                                     Id = questSaveData.Id,

                                     SaveId = questSaveData.SaveId,
                                     PhaseSaveId = questSaveData.PhaseSaveId,
                                     QuestId = questSaveData.QuestId,

                                     Complete = questSaveData.Complete

                                 }).ToList(),

            ObjectiveSaveDataList = (from objectiveSaveData in objectiveSaveDataList
                                     select new ObjectiveSaveElementData
                                     {
                                         Id = objectiveSaveData.Id,

                                         SaveId = objectiveSaveData.SaveId,
                                         QuestSaveId = objectiveSaveData.QuestSaveId,
                                         ObjectiveId = objectiveSaveData.ObjectiveId,

                                         Complete = objectiveSaveData.Complete

                                     }).ToList(),

            TaskSaveDataList = (from taskSaveData in taskSaveDataList
                                select new TaskSaveElementData
                                {
                                    Id = taskSaveData.Id,

                                    SaveId = taskSaveData.SaveId,
                                    WorldInteractableId = taskSaveData.WorldInteractableId,
                                    ObjectiveSaveId = taskSaveData.ObjectiveSaveId,
                                    TaskId = taskSaveData.TaskId,

                                    Complete = taskSaveData.Complete

                                }).ToList(),

            InteractionSaveDataList = (from interactionSaveData in interactionSaveDataList
                                       select new InteractionSaveElementData
                                       {
                                           Id = interactionSaveData.Id,

                                           SaveId = interactionSaveData.SaveId,
                                           TaskSaveId = interactionSaveData.TaskSaveId,
                                           InteractionId = interactionSaveData.InteractionId,

                                           Complete = interactionSaveData.Complete

                                       }).ToList()
        };

        gameSaveDataElement.SetOriginalValues();

        return new List<IElementData>() { gameSaveDataElement };
    }

    private static void GetPlayerSaveData(Search.GameSave searchData)
    {
        var searchParameters = new Search.PlayerSave();
        searchParameters.saveId = searchData.saveId;

        playerSaveDataList = DataManager.GetPlayerSaveData(searchParameters);
    }

    private static void GetChapterSaveData(Search.GameSave searchData)
    {
        var searchParameters = new Search.ChapterSave();
        searchParameters.saveId = searchData.saveId;

        chapterSaveDataList = DataManager.GetChapterSaveData(searchParameters);
    }

    private static void GetPhaseSaveData(Search.GameSave searchData)
    {
        var searchParameters = new Search.PhaseSave();
        searchParameters.saveId = searchData.saveId;

        phaseSaveDataList = DataManager.GetPhaseSaveData(searchParameters);
    }

    private static void GetQuestSaveData(Search.GameSave searchData)
    {
        var searchParameters = new Search.QuestSave();
        searchParameters.saveId = searchData.saveId;

        questSaveDataList = DataManager.GetQuestSaveData(searchParameters);
    }

    private static void GetObjectiveSaveData(Search.GameSave searchData)
    {
        var searchParameters = new Search.ObjectiveSave();
        searchParameters.saveId = searchData.saveId;

        objectiveSaveDataList = DataManager.GetObjectiveSaveData(searchParameters);
    }

    private static void GetTaskSaveData(Search.GameSave searchData)
    {
        var searchParameters = new Search.TaskSave();
        searchParameters.saveId = searchData.saveId;

        taskSaveDataList = DataManager.GetTaskSaveData(searchParameters);
    }

    private static void GetInteractionSaveData(Search.GameSave searchData)
    {
        var searchParameters = new Search.InteractionSave();
        searchParameters.saveId = searchData.saveId;

        interactionSaveDataList = DataManager.GetInteractionSaveData(searchParameters);
    }

    public static void UpdateData(GameSaveElementData elementData, DataRequest dataRequest)
    {

    }
}