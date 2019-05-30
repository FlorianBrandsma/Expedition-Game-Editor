using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseElementController : MonoBehaviour, IDataController
{
    public Search.PhaseElement searchParameters;

    private PhaseElementDataManager phaseElementDataManager = new PhaseElementDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.PhaseElement; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.PhaseElement>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        phaseElementDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = phaseElementDataManager.GetQuestElementDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {

    }
}