using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveController : MonoBehaviour, IDataController
{
    public Search.Objective searchParameters;

    private ObjectiveDataManager objectiveDataManager;

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Objective; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.Navigation; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Objective>().FirstOrDefault(); }
    }

    public ObjectiveController()
    {
        objectiveDataManager = new ObjectiveDataManager(this);
    }

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return objectiveDataManager.GetObjectiveDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData) { }

    public void ToggleElement(IDataElement dataElement) { }
}