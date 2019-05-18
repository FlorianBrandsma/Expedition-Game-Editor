using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuestController : MonoBehaviour, IDataController
{
    public Search.Quest searchParameters;

    private QuestDataManager questDataManager   = new QuestDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Quest; } }
    public ICollection DataList                 { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Quest>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        questDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = questDataManager.GetQuestDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {

    }
}