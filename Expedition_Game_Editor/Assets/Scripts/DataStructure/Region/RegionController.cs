using UnityEngine;
using System.Collections;
using System.Linq;

public class RegionController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.Region; } }

    public ICollection data_list { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    RegionManager regionManager = new RegionManager();

    public void InitializeController()
    {
        GetData();
    }

    public void GetData()
    {
        data_list = regionManager.GetRegionDataElements(this);

        var regionDataElements = data_list.Cast<RegionDataElement>();

        //regionDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //regionDataElements[0].Update();
    }
}
