using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuestController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.Quest; } }

    public ICollection dataList { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    QuestManager questManager = new QuestManager();

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        dataList = questManager.GetQuestDataElements(this);

        var questDataElements = dataList.Cast<QuestDataElement>();

        //questDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //questDataElements[0].Update();
    }
}