using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuestElementDataManager
{
    private QuestElementController questElementController;
    private List<QuestElementData> questElementDataList;

    public void InitializeManager(QuestElementController questElementController)
    {
        this.questElementController = questElementController;
    }

    public List<QuestElementDataElement> GetQuestElementDataElements(IEnumerable searchParameters)
    {
        var questElementSearchData = searchParameters.Cast<Search.QuestElement>().FirstOrDefault();

        GetQuestElementData(questElementSearchData);

        var list = (from questElementData in questElementDataList
                    select new QuestElementDataElement()
                    {
                        id = questElementData.id,
                        table = questElementData.table,

                        Index = questElementData.index,
                        Name = questElementData.name

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetQuestElementData(Search.QuestElement searchParameters)
    {
        questElementDataList = new List<QuestElementData>();

        for (int i = 0; i < searchParameters.temp_id_count; i++)
        {
            var questElementData = new QuestElementData();

            questElementData.id = (i + 1);
            questElementData.table = "QuestElement";
            questElementData.index = i;

            questElementData.name = "QuestElement " + (i + 1);

            questElementDataList.Add(questElementData);
        }
    }

    internal class QuestElementData : GeneralData
    {
        public int index;
        public string name;
    }
}
