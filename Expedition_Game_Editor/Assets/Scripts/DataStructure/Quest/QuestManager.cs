using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class QuestManager
{
    private QuestController dataController;
    private List<QuestData> questData_list;

    public List<QuestDataElement> GetQuestDataElements(QuestController dataController)
    {
        this.dataController = dataController;

        GetQuestData();
        //GetIconData()?

        var list = (from oCore in questData_list
                    select new QuestDataElement()
                    {
                        id = oCore.id,
                        table = oCore.table,
                        type = oCore.type,
                        index = oCore.index,
                        name = oCore.name

                    }).OrderBy(x => x.index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetQuestData()
    {
        questData_list = new List<QuestData>();

        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var questData = new QuestData();

            questData.id = (i + 1);
            questData.table = "Quest";
            questData.index = i;

            questData.name = "Quest " + (i + 1);

            questData_list.Add(questData);
        }
    }

    internal class QuestData : GeneralData
    {
        public int index;
        public string name;
    }
}
