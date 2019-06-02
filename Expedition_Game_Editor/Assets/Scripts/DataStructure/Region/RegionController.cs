using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RegionController : MonoBehaviour, IDataController
{
    public Enums.RegionType regionType;

    public Search.Region searchParameters;

    private RegionDataManager regionDataManager = new RegionDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Region; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Region>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        regionDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = regionDataManager.GetRegionDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {
        
    }

    public void ToggleElement(IDataElement dataElement)
    {

    }
}