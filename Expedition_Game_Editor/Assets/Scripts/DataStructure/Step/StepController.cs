using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StepController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.Step; } }

    public ICollection dataList { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    StepManager stepManager = new StepManager();

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        dataList = stepManager.GetStepDataElements(this);

        var stepDataElements = dataList.Cast<StepDataElement>();

        //stepDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //stepDataElements[0].Update();
    }
}