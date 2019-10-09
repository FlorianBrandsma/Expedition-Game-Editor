using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseController : MonoBehaviour, IDataController
{
    public Search.Phase searchParameters;

    private PhaseDataManager phaseDataManager;

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
        phaseDataManager = new PhaseDataManager(this);
    }

    private void CreatePhaseElements()
    {

    }

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return phaseDataManager.GetPhaseDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData)
    {
        CreatePhaseElements();
    }

    public void ToggleElement(IDataElement dataElement) { }
}