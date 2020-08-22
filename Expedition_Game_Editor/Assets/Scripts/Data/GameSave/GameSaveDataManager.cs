using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameSaveDataManager : IDataManager
{
    public IDataController DataController { get; set; }
    
    private DataManager dataManager = new DataManager();

    private List<DataManager.PlayerSaveData> playerSaveDataList;
    private List<DataManager.ChapterSaveData> chapterSaveDataList;
    private List<DataManager.PhaseSaveData> phaseSaveDataList;
    private List<DataManager.QuestSaveData> questSaveDataList;
    private List<DataManager.ObjectiveSaveData> objectiveSaveDataList;
    private List<DataManager.TaskSaveData> taskSaveDataList;
    private List<DataManager.InteractionSaveData> interactionSaveDataList;

    public GameSaveDataManager(GameSaveController gameSaveController)
    {
        DataController = gameSaveController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
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
            playerSaveData = (from playerSaveData in playerSaveDataList
                              select new PlayerSaveElementData
                              {
                                  Id = playerSaveData.id,

                                  SaveId = playerSaveData.id,
                                  RegionId = playerSaveData.regionId,
                                  PartyMemberId = playerSaveData.partyMemberId,

                                  PositionX = playerSaveData.positionX,
                                  PositionY = playerSaveData.positionY,
                                  PositionZ = playerSaveData.positionZ,

                                  GameTime = playerSaveData.gameTime,
                                  PlayedTime = playerSaveData.playedSeconds

                              }).First(),

            chapterSaveDataList = (from chapterSaveData in chapterSaveDataList
                                   select new ChapterSaveElementData
                                   {
                                       Id = chapterSaveData.id,

                                       SaveId = chapterSaveData.saveId,
                                       ChapterId = chapterSaveData.chapterId,

                                       Complete = chapterSaveData.complete

                                   }).ToList(),

            phaseSaveDataList = (from phaseSaveData in phaseSaveDataList
                                 select new PhaseSaveElementData
                                 {
                                     Id = phaseSaveData.id,

                                     SaveId = phaseSaveData.saveId,
                                     ChapterSaveId = phaseSaveData.chapterSaveId,
                                     PhaseId = phaseSaveData.phaseId,

                                     Complete = phaseSaveData.complete

                                 }).ToList(),

            questSaveDataList = (from questSaveData in questSaveDataList
                                 select new QuestSaveElementData
                                 {
                                     Id = questSaveData.id,

                                     SaveId = questSaveData.saveId,
                                     PhaseSaveId = questSaveData.phaseSaveId,
                                     QuestId = questSaveData.questId,

                                     Complete = questSaveData.complete

                                 }).ToList(),

            objectiveSaveDataList = (from objectiveSaveData in objectiveSaveDataList
                                     select new ObjectiveSaveElementData
                                     {
                                         Id = objectiveSaveData.id,

                                         SaveId = objectiveSaveData.saveId,
                                         QuestSaveId = objectiveSaveData.questSaveId,
                                         ObjectiveId = objectiveSaveData.objectiveId,

                                         Complete = objectiveSaveData.complete

                                     }).ToList(),

            taskSaveDataList = (from taskSaveData in taskSaveDataList
                                select new TaskSaveElementData
                                {
                                    Id = taskSaveData.id,

                                    SaveId = taskSaveData.saveId,
                                    WorldInteractableId = taskSaveData.worldInteractableId,
                                    ObjectiveSaveId = taskSaveData.objectiveSaveId,
                                    TaskId = taskSaveData.taskId,

                                    Complete = taskSaveData.complete

                                }).ToList(),

            interactionSaveDataList = (from interactionSaveData in interactionSaveDataList
                                       select new InteractionSaveElementData
                                       {
                                           Id = interactionSaveData.id,

                                           SaveId = interactionSaveData.saveId,
                                           TaskSaveId = interactionSaveData.taskSaveId,
                                           InteractionId = interactionSaveData.interactionId,

                                           Complete = interactionSaveData.complete

                                       }).ToList()
        };

        gameSaveDataElement.SetOriginalValues();

        return new List<IElementData>() { gameSaveDataElement };
    }

    internal void GetPlayerSaveData(Search.GameSave searchData)
    {
        var searchParameters = new Search.PlayerSave();
        searchParameters.saveId = searchData.saveId;

        playerSaveDataList = dataManager.GetPlayerSaveData(searchParameters);
    }

    internal void GetChapterSaveData(Search.GameSave searchData)
    {
        var searchParameters = new Search.ChapterSave();
        searchParameters.saveId = searchData.saveId;

        chapterSaveDataList = dataManager.GetChapterSaveData(searchParameters);
    }

    internal void GetPhaseSaveData(Search.GameSave searchData)
    {
        var searchParameters = new Search.PhaseSave();
        searchParameters.saveId = searchData.saveId;

        phaseSaveDataList = dataManager.GetPhaseSaveData(searchParameters);
    }

    internal void GetQuestSaveData(Search.GameSave searchData)
    {
        var searchParameters = new Search.QuestSave();
        searchParameters.saveId = searchData.saveId;

        questSaveDataList = dataManager.GetQuestSaveData(searchParameters);
    }

    internal void GetObjectiveSaveData(Search.GameSave searchData)
    {
        var searchParameters = new Search.ObjectiveSave();
        searchParameters.saveId = searchData.saveId;

        objectiveSaveDataList = dataManager.GetObjectiveSaveData(searchParameters);
    }

    internal void GetTaskSaveData(Search.GameSave searchData)
    {
        var searchParameters = new Search.TaskSave();
        searchParameters.saveId = searchData.saveId;

        taskSaveDataList = dataManager.GetTaskSaveData(searchParameters);
    }

    internal void GetInteractionSaveData(Search.GameSave searchData)
    {
        var searchParameters = new Search.InteractionSave();
        searchParameters.saveId = searchData.saveId;

        interactionSaveDataList = dataManager.GetInteractionSaveData(searchParameters);
    }
}