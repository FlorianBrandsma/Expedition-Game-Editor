using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InteractionController : MonoBehaviour, IDataController
{
    public Search.Interaction searchParameters;

    private InteractionDataManager interactionDataManager;

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Interaction; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.Navigation; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Interaction>().FirstOrDefault(); }
    }

    public InteractionController()
    {
        interactionDataManager = new InteractionDataManager(this);
    }

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return interactionDataManager.GetInteractionDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData) { }

    public void ToggleElement(IDataElement dataElement) { }
}