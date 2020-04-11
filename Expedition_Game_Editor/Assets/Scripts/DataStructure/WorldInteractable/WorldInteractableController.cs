using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WorldInteractableController : MonoBehaviour, IDataController
{
    public Search.Interactable searchParameters;

    public IDataManager DataManager { get; set; }

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.WorldInteractable; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.Navigation; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Interactable>().FirstOrDefault(); }
    }

    public WorldInteractableController()
    {
        DataManager = new WorldInteractableDataManager(this);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData)
    {
        var worldInteractableData = (WorldInteractableDataElement)searchElement.data.dataElement;

        switch (((GeneralData)resultData).DataType)
        {
            case Enums.DataType.Interactable:

                var resultElementData = (InteractableDataElement)resultData;

                worldInteractableData.InteractableId = resultElementData.Id;
                worldInteractableData.interactableName = resultElementData.Name;
                worldInteractableData.objectGraphicIconPath = resultElementData.objectGraphicIconPath;
                
                break;
        }
    }

    public void ToggleElement(IDataElement dataElement) { }
}
