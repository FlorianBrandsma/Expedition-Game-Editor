using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public IDataManager DataManager { get; set; }

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Phase; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.Navigation; } }
    public List<IDataElement> DataList          { get; set; }

    public SearchProperties SearchProperties
    {
        get { return searchProperties; }
        set { searchProperties = value; }
    }

    public PhaseController()
    {
        DataManager = new PhaseDataManager(this);
    }

    public void InitializeController()
    {
        SearchProperties.Initialize();
    }

    private void CreatePhaseElements()
    {

    }

    public void SetData(DataElement searchElement, IDataElement resultData)
    {
        CreatePhaseElements();
    }

    public void ToggleElement(EditorElement editorElement) { }
}