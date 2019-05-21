using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuestDataManager
{
    private QuestController questController;
    private List<QuestData> questDataList;

    public void InitializeManager(QuestController questController)
    {
        this.questController = questController;
    }

    public List<QuestDataElement> GetQuestDataElements(IEnumerable searchParameters)
    {
        var questSearchData = searchParameters.Cast<Search.Quest>().FirstOrDefault();

        GetQuestData(questSearchData);

        var list = (from questData in questDataList
                    select new QuestDataElement()
                    {
                        id = questData.id,
                        table = questData.table,

                        Index = questData.index,
                        Name = questData.name,
                        Notes = questData.notes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetQuestData(Search.Quest searchParameters)
    {
        questDataList = new List<QuestData>();

        foreach(Fixtures.Quest quest in Fixtures.questList)
        { 
            var questData = new QuestData();

            questData.id = quest.id;
            questData.table = "Quest";
            questData.index = quest.index;

            if (searchParameters.phaseId.Count > 0 && !searchParameters.phaseId.Contains(quest.phaseId)) continue;

            questData.phaseId = quest.phaseId;
            questData.name = quest.name;
            questData.notes = quest.notes;

            questDataList.Add(questData);
        }
    }

    internal class QuestData : GeneralData
    {
        public int phaseId;
        public string name;
        public string notes;
    }
}
