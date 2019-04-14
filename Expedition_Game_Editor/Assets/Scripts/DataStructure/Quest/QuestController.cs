using UnityEngine;
using System.Collections;
using System.Linq;

public class QuestController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController  { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type             { get { return Enums.DataType.Quest; } }

    public ICollection data_list { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    QuestManager questManager = new QuestManager();

    public void InitializeController()
    {
        GetData();
    }

    public void GetData()
    {
        data_list = questManager.GetQuestDataElements(this);

        var questDataElements = data_list.Cast<QuestDataElement>();

        //chapterDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //chapterDataElements[0].Update();
    }
}
