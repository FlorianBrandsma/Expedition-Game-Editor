using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseController : MonoBehaviour, IDataController
{
    public Search.Phase searchParameters;

    public IDataManager DataManager { get; set; }

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Phase; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.Navigation; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Phase>().FirstOrDefault(); }
    }

    public PhaseController()
    {
        DataManager = new PhaseDataManager(this);
    }

    private void CreatePhaseElements()
    {

    }

    public void SetData(SelectionElement searchElement, IDataElement resultData)
    {
        CreatePhaseElements();
    }

    public void ToggleElement(IDataElement dataElement) { }
}