using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuestDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<QuestData> questDataList;

    public QuestDataManager(QuestController questController)
    {
        DataController = questController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Quest>().First();

        GetQuestData(searchParameters);

        if (questDataList.Count == 0) return new List<IElementData>();

        var list = (from questData in questDataList
                    select new QuestElementData()
                    {
                        Id = questData.id,
                        Index = questData.index,

                        PhaseId = questData.phaseId,
                        Name = questData.name,
                        PublicNotes = questData.notes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public void GetQuestData(Search.Quest searchParameters)
    {
        questDataList = new List<QuestData>();

        foreach(Fixtures.Quest quest in Fixtures.questList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(quest.id)) continue;
            if (searchParameters.phaseId.Count  > 0 && !searchParameters.phaseId.Contains(quest.phaseId)) continue;

            var questData = new QuestData();

            questData.id = quest.id;
            questData.index = quest.index;

            questData.phaseId = quest.phaseId;
            questData.name = quest.name;
            questData.notes = quest.publicNotes;

            questDataList.Add(questData);
        }
    }

    internal class QuestData
    {
        public int id;
        public int index;

        public int phaseId;

        public string name;
        public string notes;
    }
}
