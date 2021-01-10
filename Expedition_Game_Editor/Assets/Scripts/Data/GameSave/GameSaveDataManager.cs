using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class GameSaveDataManager
{
    private static List<PlayerSaveBaseData> playerSaveDataList;

    private static List<ChapterSaveBaseData> chapterSaveDataList;
    private static List<ChapterBaseData> chapterDataList;

    private static List<PhaseSaveBaseData> phaseSaveDataList;
    private static List<PhaseBaseData> phaseDataList;

    private static List<QuestSaveBaseData> questSaveDataList;
    private static List<QuestBaseData> questDataList;

    private static List<ObjectiveSaveBaseData> objectiveSaveDataList;
    private static List<ObjectiveBaseData> objectiveDataList;

    private static List<TaskSaveBaseData> taskSaveDataList;
    private static List<TaskBaseData> taskDataList;

    private static List<InteractionSaveBaseData> interactionSaveDataList;

    public static List<IElementData> GetData(Search.GameSave searchParameters)
    {
        GetPlayerSaveData(searchParameters);
        
        if (playerSaveDataList.Count == 0) return new List<IElementData>();

        GetChapterSaveData(searchParameters);
        GetChapterData();

        GetPhaseSaveData(searchParameters);
        GetPhaseData();

        GetQuestSaveData(searchParameters);
        GetQuestData();

        GetObjectiveSaveData(searchParameters);
        GetObjectiveData();

        GetTaskSaveData(searchParameters);
        GetTaskData();

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
                                   join chapterData     in chapterDataList on chapterSaveData.ChapterId equals chapterData.Id
                                   select new ChapterSaveElementData
                                   {
                                       Id = chapterSaveData.Id,

                                       SaveId = chapterSaveData.SaveId,
                                       ChapterId = chapterSaveData.ChapterId,

                                       Complete = chapterSaveData.Complete,

                                       Index = chapterData.Index

                                   }).ToList(),

            PhaseSaveDataList = (from phaseSaveData in phaseSaveDataList
                                 join phaseData     in phaseDataList on phaseSaveData.PhaseId equals phaseData.Id
                                 select new PhaseSaveElementData
                                 {
                                     Id = phaseSaveData.Id,

                                     SaveId = phaseSaveData.SaveId,
                                     PhaseId = phaseSaveData.PhaseId,

                                     Complete = phaseSaveData.Complete,

                                     ChapterId = phaseData.ChapterId,

                                     Index = phaseData.Index
                                     
                                 }).ToList(),

            QuestSaveDataList = (from questSaveData in questSaveDataList
                                 join questData     in questDataList on questSaveData.QuestId equals questData.Id
                                 select new QuestSaveElementData
                                 {
                                     Id = questSaveData.Id,

                                     SaveId = questSaveData.SaveId,
                                     QuestId = questSaveData.QuestId,

                                     Complete = questSaveData.Complete,

                                     PhaseId = questData.PhaseId,

                                     Index = questData.Index
                                     
                                 }).ToList(),

            ObjectiveSaveDataList = (from objectiveSaveData in objectiveSaveDataList
                                     join objectiveData     in objectiveDataList on objectiveSaveData.ObjectiveId equals objectiveData.Id
                                     select new ObjectiveSaveElementData
                                     {
                                         Id = objectiveSaveData.Id,

                                         SaveId = objectiveSaveData.SaveId,
                                         ObjectiveId = objectiveSaveData.ObjectiveId,

                                         Complete = objectiveSaveData.Complete,

                                         QuestId = objectiveData.QuestId,

                                         Index = objectiveData.Index
                                         
                                     }).ToList(),

            TaskSaveDataList = (from taskSaveData   in taskSaveDataList
                                join taskData       in taskDataList on taskSaveData.TaskId equals taskData.Id
                                select new TaskSaveElementData
                                {
                                    Id = taskSaveData.Id,

                                    SaveId = taskSaveData.SaveId,
                                    TaskId = taskSaveData.TaskId,
                                    WorldInteractableId = taskSaveData.WorldInteractableId,

                                    Complete = taskSaveData.Complete,

                                    Index = taskData.Index

                                }).ToList(),

            InteractionSaveDataList = (from interactionSaveData in interactionSaveDataList
                                       select new InteractionSaveElementData
                                       {
                                           Id = interactionSaveData.Id,

                                           SaveId = interactionSaveData.SaveId,
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

    private static void GetChapterData()
    {
        var searchParameters = new Search.Chapter();
        searchParameters.id = chapterSaveDataList.Select(x => x.ChapterId).ToList();

        chapterDataList = DataManager.GetChapterData(searchParameters);
    }

    private static void GetPhaseSaveData(Search.GameSave searchData)
    {
        var searchParameters = new Search.PhaseSave();
        searchParameters.saveId = searchData.saveId;

        phaseSaveDataList = DataManager.GetPhaseSaveData(searchParameters);
    }

    private static void GetPhaseData()
    {
        var searchParameters = new Search.Phase();
        searchParameters.id = phaseSaveDataList.Select(x => x.PhaseId).ToList();

        phaseDataList = DataManager.GetPhaseData(searchParameters);
    }

    private static void GetQuestSaveData(Search.GameSave searchData)
    {
        var searchParameters = new Search.QuestSave();
        searchParameters.saveId = searchData.saveId;

        questSaveDataList = DataManager.GetQuestSaveData(searchParameters);
    }

    private static void GetQuestData()
    {
        var searchParameters = new Search.Quest();
        searchParameters.id = questSaveDataList.Select(x => x.QuestId).ToList();

        questDataList = DataManager.GetQuestData(searchParameters);
    }

    private static void GetObjectiveSaveData(Search.GameSave searchData)
    {
        var searchParameters = new Search.ObjectiveSave();
        searchParameters.saveId = searchData.saveId;

        objectiveSaveDataList = DataManager.GetObjectiveSaveData(searchParameters);
    }

    private static void GetObjectiveData()
    {
        var searchParameters = new Search.Objective();
        searchParameters.id = objectiveSaveDataList.Select(x => x.ObjectiveId).ToList();

        objectiveDataList = DataManager.GetObjectiveData(searchParameters);
    }

    private static void GetTaskSaveData(Search.GameSave searchData)
    {
        var searchParameters = new Search.TaskSave();
        searchParameters.saveId = searchData.saveId;

        taskSaveDataList = DataManager.GetTaskSaveData(searchParameters);
    }

    private static void GetTaskData()
    {
        var searchParameters = new Search.Task();
        searchParameters.id = taskSaveDataList.Select(x => x.TaskId).ToList();

        taskDataList = DataManager.GetTaskData(searchParameters);
    }

    private static void GetInteractionSaveData(Search.GameSave searchData)
    {
        var searchParameters = new Search.InteractionSave();
        searchParameters.saveId = searchData.saveId;

        interactionSaveDataList = DataManager.GetInteractionSaveData(searchParameters);
    }
}