using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveElementController : MonoBehaviour, IDataController
{
    public Search.ObjectiveElement searchParameters;

    private ObjectiveElementDataManager objectiveElementDataManager = new ObjectiveElementDataManager();

    public IDisplay Display { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType { get { return Enums.DataType.ObjectiveElement; } }
    public ICollection DataList { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.ObjectiveElement>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        objectiveElementDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = objectiveElementDataManager.GetObjectiveElementDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {

    }
}