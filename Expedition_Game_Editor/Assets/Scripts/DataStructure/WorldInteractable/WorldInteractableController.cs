using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WorldInteractableController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public IDataManager DataManager             { get; set; }

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.WorldInteractable; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.Navigation; } }
    public List<IDataElement> DataList          { get; set; }

    public SearchProperties SearchProperties
    {
        get { return searchProperties; }
        set { searchProperties = value; }
    }

    public WorldInteractableController()
    {
        DataManager = new WorldInteractableDataManager(this);
    }

    public void InitializeController()
    {
        SearchProperties.Initialize();
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData)
    {
        var worldInteractableData = (WorldInteractableDataElement)searchElement.data.dataElement;

        switch (resultData.DataType)
        {
            case Enums.DataType.ObjectGraphic:

                var objectGraphicData = (ObjectGraphicDataElement)resultData;

                worldInteractableData.ObjectGraphicId = objectGraphicData.Id;
                worldInteractableData.interactableName = objectGraphicData.Name;
                worldInteractableData.objectGraphicIconPath = objectGraphicData.iconPath;

                break;

            case Enums.DataType.Interactable:

                var interactableData = (InteractableDataElement)resultData;

                worldInteractableData.InteractableId = interactableData.Id;
                worldInteractableData.ObjectGraphicId = interactableData.ObjectGraphicId;
                worldInteractableData.interactableName = interactableData.Name;
                worldInteractableData.objectGraphicIconPath = interactableData.objectGraphicIconPath;
                
                break;

            default: Debug.Log("CASE MISSING: " + resultData.DataType); break;
        }
    }

    public void ToggleElement(IDataElement dataElement) { }
}
