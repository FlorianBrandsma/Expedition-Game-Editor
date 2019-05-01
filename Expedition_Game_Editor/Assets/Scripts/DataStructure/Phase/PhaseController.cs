using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseController : MonoBehaviour, IDataController
{
    public bool searchById;
    public int temp_id_count;

    private PhaseManager phaseManager           = new PhaseManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType DataType              { get { return Enums.DataType.Phase; } }
    public ICollection DataList                 { get; set; }

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        DataList = phaseManager.GetPhaseDataElements(this);

        var phaseDataElements = DataList.Cast<PhaseDataElement>();

        //phaseDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //phaseDataElements[0].Update();
    }

    public void GetData(SearchData searchData)
    {

    }
}