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

    public List<IDataElement> GetQuestDataElements(IEnumerable searchParameters)
    {
        var questSearchData = searchParameters.Cast<Search.Quest>().FirstOrDefault();

        GetQuestData(questSearchData);

        var list = (from questData in questDataList
                    select new QuestDataElement()
                    {
                        dataType = Enums.DataType.Quest,

                        id = questData.id,
                        index = questData.index,

                        PhaseId = questData.phaseId,
                        Name = questData.name,
                        Notes = questData.notes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetQuestData(Search.Quest searchParameters)
    {
        questDataList = new List<QuestData>();

        foreach(Fixtures.Quest quest in Fixtures.questList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(quest.id)) continue;
            if (searchParameters.phaseId.Count > 0 && !searchParameters.phaseId.Contains(quest.phaseId)) continue;

            var questData = new QuestData();

            questData.id = quest.id;
            questData.index = quest.index;

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
