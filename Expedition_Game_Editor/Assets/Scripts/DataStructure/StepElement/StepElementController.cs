using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StepElementController : MonoBehaviour//, IDataController
{
    public int temp_id_count;

    public SearchParameters searchParameters;

    private StepElementManager stepElementManager = new StepElementManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.StepElement; } }
    public ICollection DataList                 { get; set; }

    public SearchParameters SearchParameters
    {
        get { return searchParameters; }
        set { searchParameters = value; }
    }

    public void InitializeController()
    {
        //GetData(new List<int>());
    }

    public void GetData(SearchParameters searchParameters)
    {
        DataList = stepElementManager.GetStepElementDataElements(this);

        var stepElementDataElements = DataList.Cast<StepElementDataElement>();

        //stepElementDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //stepElementDataElements[0].Update();
    }

    public void ReplaceData(IEnumerable dataElement)
    {

    }
}