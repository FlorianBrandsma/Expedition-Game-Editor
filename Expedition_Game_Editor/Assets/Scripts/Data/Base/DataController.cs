using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataController : IDataController
{
    public IDataManager DataManager { get; set; }

    private List<IDataElement> dataList = new List<IDataElement>();

    public SegmentController SegmentController { get; set; }
    
    public List<IDataElement> DataList
    {
        get { return dataList; }
        set { dataList = value; }
    }

    public Enums.DataType DataType { get; set; }

    public Enums.DataCategory DataCategory { get; set; }

    public SearchProperties SearchProperties { get; set; }

    public DataController(Enums.DataType dataType)
    {
        DataType = dataType;

        switch(DataType)
        {
            case Enums.DataType.WorldInteractable:  DataManager = new WorldInteractableDataManager(this);   break;
            case Enums.DataType.Interaction:        DataManager = new InteractionDataManager(this);         break;
            case Enums.DataType.WorldObject:        DataManager = new WorldObjectDataManager(this);         break;
        }
    }

    public void InitializeController() { }

    public void SetData(SelectionElement searchElement, IDataElement resultDataElement)
    {
        switch(DataType)
        {
            case Enums.DataType.WorldInteractable:  SetWorldInteractableData(searchElement, resultDataElement); break;
            case Enums.DataType.WorldObject:        SetWorldObjectData(searchElement, resultDataElement);       break;

            default: Debug.Log("CASE MISSING: " + DataType); break;
        }
    }

    public void SetWorldInteractableData(SelectionElement searchElement, IDataElement resultDataElement)
    {
        var worldInteractableData = (WorldInteractableDataElement)searchElement.data.dataElement;

        switch (resultDataElement.DataType)
        {
            case Enums.DataType.Interactable:

                var interactableData = (InteractableDataElement)resultDataElement;

                worldInteractableData.InteractableId = interactableData.Id;

                worldInteractableData.interactableName = interactableData.Name;
                worldInteractableData.objectGraphicIconPath = interactableData.objectGraphicIconPath;
                worldInteractableData.objectGraphicPath = interactableData.objectGraphicPath;

                break;
        }
    }

    public void SetWorldObjectData(SelectionElement searchElement, IDataElement resultDataElement)
    {
        var worldObjectData = (WorldObjectDataElement)searchElement.data.dataElement;

        switch (resultDataElement.DataType)
        {
            case Enums.DataType.ObjectGraphic:

                var objectGraphicData = (ObjectGraphicDataElement)resultDataElement;

                worldObjectData.ObjectGraphicId = objectGraphicData.Id;

                worldObjectData.objectGraphicName = objectGraphicData.Name;
                worldObjectData.objectGraphicIconPath = objectGraphicData.iconPath;
                worldObjectData.objectGraphicPath = objectGraphicData.Path;

                worldObjectData.height = objectGraphicData.Height;
                worldObjectData.width = objectGraphicData.Width;
                worldObjectData.depth = objectGraphicData.Depth;

                break;
        }
    }

    public void ToggleElement(IDataElement dataElement) { }
}
