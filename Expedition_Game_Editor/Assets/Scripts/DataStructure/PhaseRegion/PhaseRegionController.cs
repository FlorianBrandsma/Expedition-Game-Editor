using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseRegionController : MonoBehaviour, IDataController
{
    public Enums.RegionType regionType;

    public Search.PhaseRegion searchParameters;

    private PhaseRegionDataManager phaseRegionDataManager = new PhaseRegionDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.PhaseRegion; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.PhaseRegion>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        phaseRegionDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = phaseRegionDataManager.GetPhaseRegionDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {

    }

    public void ToggleElement(IDataElement dataElement)
    {

    }
}