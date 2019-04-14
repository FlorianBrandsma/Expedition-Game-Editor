using UnityEngine;
using System.Collections;
using System.Linq;

public class PhaseController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController  { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type             { get { return Enums.DataType.Phase; } }

    public ICollection data_list { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    PhaseManager phaseManager = new PhaseManager();

    public void InitializeController()
    {
        GetData();
    }

    public void GetData()
    {
        data_list = phaseManager.GetPhaseDataElements(this);

        var phaseDataElements = data_list.Cast<PhaseDataElement>();

        phaseDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());

        //chapterDataElements[0].Update();
    }
}
