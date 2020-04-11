using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WorldController : MonoBehaviour, IDataController
{
    public Search.Region searchParameters;

    public IDataManager DataManager { get; set; }
    
    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.World; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Region>().FirstOrDefault(); }
    }

    public WorldController()
    {
        DataManager = new WorldDataManager(this);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData)
    {
        switch (((GeneralData)searchElement.data.dataElement).DataType)
        { 
            case Enums.DataType.WorldInteractable:  SetWorldInteractableData(searchElement, resultData);    break;
            case Enums.DataType.WorldObject:        SetWorldObjectData(searchElement, resultData);          break;
        }
    }

    private void SetWorldInteractableData(SelectionElement searchElement, IDataElement resultData)
    {
        var worldInteractableData = (WorldInteractableDataElement)searchElement.data.dataElement;

        switch (((GeneralData)resultData).DataType)
        {
            case Enums.DataType.Interactable:

                var resultElementData = (InteractableDataElement)resultData;

                worldInteractableData.objectGraphicId = resultElementData.ObjectGraphicId;
                worldInteractableData.objectGraphicIconPath = resultElementData.objectGraphicIconPath;
                worldInteractableData.objectGraphicPath = resultElementData.objectGraphicPath;

                break;
        }
    }

    private void SetWorldObjectData(SelectionElement searchElement, IDataElement resultData)
    {
        var worldObjectData = (WorldObjectDataElement)searchElement.data.dataElement;

        switch (((GeneralData)resultData).DataType)
        {
            case Enums.DataType.ObjectGraphic:
                
                var resultElementData = (ObjectGraphicDataElement)resultData;

                worldObjectData.ObjectGraphicId = resultElementData.Id;
                worldObjectData.objectGraphicName = resultElementData.Name;
                worldObjectData.objectGraphicIconPath = resultElementData.iconPath;
                worldObjectData.objectGraphicPath = resultElementData.Path;

                break;
        }
    }

    public void ToggleElement(IDataElement dataElement) { }
}
