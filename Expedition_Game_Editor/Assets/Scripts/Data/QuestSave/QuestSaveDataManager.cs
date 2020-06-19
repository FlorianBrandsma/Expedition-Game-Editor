using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuestSaveDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<QuestSaveData> questSaveDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.QuestData> questDataList;

    public QuestSaveDataManager(QuestSaveController questController)
    {
        DataController = questController;
    }

    public List<IDataElement> GetDataElements(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.QuestSave>().First();

        GetQuestSaveData(searchParameters);

        if (questSaveDataList.Count == 0) return new List<IDataElement>();

        GetQuestData();

        var list = (from questSaveData  in questSaveDataList
                    join questData      in questDataList on questSaveData.questId equals questData.Id
                    select new QuestSaveDataElement()
                    {
                        Id = questSaveData.Id,

                        PhaseSaveId = questSaveData.phaseSaveId,
                        QuestId = questSaveData.questId,

                        Complete = questSaveData.complete,

                        name = questData.name,

                        publicNotes = questData.publicNotes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetQuestSaveData(Search.QuestSave searchParameters)
    {
        questSaveDataList = new List<QuestSaveData>();

        foreach (Fixtures.QuestSave questSave in Fixtures.questSaveList)
        {
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(questSave.Id))                     continue;
            if (searchParameters.phaseSaveId.Count  > 0 && !searchParameters.phaseSaveId.Contains(questSave.phaseSaveId))   continue;

            var questSaveData = new QuestSaveData();

            questSaveData.Id = questSave.Id;

            questSaveData.phaseSaveId = questSave.phaseSaveId;
            questSaveData.questId = questSave.questId;

            questSaveData.complete = questSave.complete;

            questSaveDataList.Add(questSaveData);
        }
    }

    internal void GetQuestData()
    {
        var questSearchParameters = new Search.Quest();
        questSearchParameters.id = questSaveDataList.Select(x => x.questId).Distinct().ToList();

        questDataList = dataManager.GetQuestData(questSearchParameters);
    }

    internal class QuestSaveData : GeneralData
    {
        public int phaseSaveId;
        public int questId;

        public bool complete;
    }
}
