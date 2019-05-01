using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StepController : MonoBehaviour, IDataController
{
    public bool searchById;
    public int temp_id_count;

    private StepManager stepManager             = new StepManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public Enums.DataType DataType              { get { return Enums.DataType.Step; } }
    public ICollection DataList                 { get; set; }

    public void InitializeController()
    {
        GetData(new List<int>());
    }

    public void GetData(List<int> id_list)
    {
        DataList = stepManager.GetStepDataElements(this);

        var stepDataElements = DataList.Cast<StepDataElement>();

        //stepDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //stepDataElements[0].Update();
    }

    public void GetData(SearchData searchData)
    {

    }
}