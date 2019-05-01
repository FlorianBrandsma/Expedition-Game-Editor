using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RegionController : MonoBehaviour, IDataController
{
    public Enums.RegionType regionType;
    public bool searchById;
    public int temp_id_count;

    private RegionManager regionManager         = new RegionManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType DataType              { get { return Enums.DataType.Region; } }
    public ICollection DataList                 { get; set; }

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        DataList = regionManager.GetRegionDataElements(this);

        var regionDataElements = DataList.Cast<RegionDataElement>();

        //regionDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //regionDataElements[0].Update();
    }

    public void GetData(SearchData searchData)
    {

    }
}