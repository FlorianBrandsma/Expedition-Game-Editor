using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseController : MonoBehaviour, IDataController
{
    public Search.Phase searchParameters;

    private PhaseManager phaseManager           = new PhaseManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Phase; } }
    public ICollection DataList                 { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Phase>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        //GetData(new List<int>());
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = phaseManager.GetPhaseDataElements(searchParameters.Cast<Search.Phase>().FirstOrDefault());

        var phaseDataElements = DataList.Cast<PhaseDataElement>();

        //phaseDataElements.Where(x => x.changed).ToList().ForEach(x => x.Update());
        //phaseDataElements[0].Update();
    }

    public void ReplaceData(SelectionElement searchElement, Data resultData)
    {

    }
}