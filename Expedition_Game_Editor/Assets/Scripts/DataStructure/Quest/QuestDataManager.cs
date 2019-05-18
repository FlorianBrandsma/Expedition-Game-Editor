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
                        Name = questData.name

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetQuestData(Search.Quest searchParameters)
    {
        questDataList = new List<QuestData>();

        for (int i = 0; i < searchParameters.temp_id_count; i++)
        {
            var questData = new QuestData();

            questData.id = (i + 1);
            questData.table = "Quest";
            questData.index = i;

            questData.name = "Quest " + (i + 1);

            questDataList.Add(questData);
        }
    }

    internal class QuestData : GeneralData
    {
        public int index;
        public string name;
    }
}
