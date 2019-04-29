using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StepElementController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type { get { return Enums.DataType.StepElement; } }

    public ICollection dataList { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    StepElementManager stepElementManager = new StepElementManager();

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        dataList = stepElementManager.GetStepElementDataElements(this);

        var stepElementDataElements = dataList.Cast<StepElementDataElement>();

        //stepElementDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //stepElementDataElements[0].Update();
    }
}