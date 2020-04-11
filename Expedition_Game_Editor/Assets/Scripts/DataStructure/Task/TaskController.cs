using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TaskController : MonoBehaviour, IDataController
{
    public Search.Task searchParameters;

    public IDataManager DataManager { get; set; }

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Task; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.Navigation; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Task>().FirstOrDefault(); }
    }

    public TaskController()
    {
        DataManager = new TaskDataManager(this);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData) { }

    public void ToggleElement(IDataElement dataElement) { }
}
