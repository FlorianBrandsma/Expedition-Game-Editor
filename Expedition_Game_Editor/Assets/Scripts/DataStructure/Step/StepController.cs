using UnityEngine;
using System.Collections;
using System.Linq;

public class StepController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.Step; } }

    public ICollection data_list { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    StepManager stepManager = new StepManager();

    public void InitializeController()
    {
        GetData();
    }

    public void GetData()
    {
        data_list = stepManager.GetStepDataElements(this);

        var stepDataElements = data_list.Cast<StepDataElement>();

        //stepDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //stepDataElements[0].Update();
    }
}
