using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.Phase; } }

    public ICollection dataList { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    PhaseManager phaseManager = new PhaseManager();

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        dataList = phaseManager.GetPhaseDataElements(this);

        var phaseDataElements = dataList.Cast<PhaseDataElement>();

        //phaseDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //phaseDataElements[0].Update();
    }
}