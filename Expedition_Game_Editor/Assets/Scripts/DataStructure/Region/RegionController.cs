using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RegionController : MonoBehaviour//, IDataController
{
    public Enums.RegionType regionType;
    public int temp_id_count;

    public SearchParameters searchParameters;

    private RegionManager regionManager         = new RegionManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Region; } }
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
        DataList = regionManager.GetRegionDataElements(this);

        var regionDataElements = DataList.Cast<RegionDataElement>();

        //regionDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //regionDataElements[0].Update();
    }

    public void ReplaceData(IEnumerable dataElement)
    {

    }
}