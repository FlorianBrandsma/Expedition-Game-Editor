using UnityEngine;
using System.Collections;
using System.Linq;

public class StepElementController : MonoBehaviour, IDataController
{
    public IDisplay display { get { return GetComponent<IDisplay>(); } }

    public SegmentController segmentController  { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType data_type             { get { return Enums.DataType.StepElement; } }

    public ICollection data_list { get; set; }

    public bool search_by_id;
    public int temp_id_count;

    StepElementManager stepElementManager = new StepElementManager();

    public void InitializeController()
    {
        GetData();
    }

    public void GetData()
    {
        data_list = stepElementManager.GetStepElementDataElements(this);

        var stepElementDataElements = data_list.Cast<StepElementDataElement>();

        //stepElementDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //stepElementDataElements[0].Update();
    }
}
