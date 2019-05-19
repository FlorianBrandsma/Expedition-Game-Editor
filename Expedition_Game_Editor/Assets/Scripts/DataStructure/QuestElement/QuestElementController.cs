using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class QuestElementController : MonoBehaviour, IDataController
{
    public Search.QuestElement searchParameters;

    private QuestElementDataManager questElementDataManager = new QuestElementDataManager();

    public IDisplay Display { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType { get { return Enums.DataType.QuestElement; } }
    public ICollection DataList { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.QuestElement>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        questElementDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = questElementDataManager.GetQuestElementDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {

    }
}