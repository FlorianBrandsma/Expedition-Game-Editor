using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuestController : MonoBehaviour//, IDataController
{
    public int temp_id_count;

    public SearchParameters searchParameters;

    private QuestManager questManager           = new QuestManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Quest; } }
    public ICollection DataList                 { get; set; }

    public SearchParameters SearchParameters
    {
        get { return searchParameters; }
        set { searchParameters = value; }
    }

    public void InitializeController()
    {
        //GetData(new List<int>());
    }

    public void GetData(SearchParameters searchParameters)
    {
        DataList = questManager.GetQuestDataElements(this);

        var questDataElements = DataList.Cast<QuestDataElement>();

        //questDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //questDataElements[0].Update();
    }

    public void ReplaceData(IEnumerable dataElement)
    {

    }
}