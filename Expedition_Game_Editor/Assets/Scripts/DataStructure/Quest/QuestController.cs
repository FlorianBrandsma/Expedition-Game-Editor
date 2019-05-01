using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuestController : MonoBehaviour, IDataController
{
    public bool searchById;
    public int temp_id_count;

    private QuestManager questManager           = new QuestManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType DataType              { get { return Enums.DataType.Quest; } }
    public ICollection DataList                 { get; set; }

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        DataList = questManager.GetQuestDataElements(this);

        var questDataElements = DataList.Cast<QuestDataElement>();

        //questDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //questDataElements[0].Update();
    }

    public void GetData(SearchData searchData)
    {

    }
}