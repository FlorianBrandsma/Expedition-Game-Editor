using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataController : IDataController
{
    public IDataManager DataManager { get; set; }

    private List<IElementData> dataList = new List<IElementData>();

    public SegmentController SegmentController { get; set; }
    
    public List<IElementData> DataList
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

    public void SetData(DataElement searchElement, IElementData resultElementData)
    {
        switch(DataType)
        {
            case Enums.DataType.WorldInteractable:  SetWorldInteractableData(searchElement, resultElementData); break;
            case Enums.DataType.WorldObject:        SetWorldObjectData(searchElement, resultElementData);       break;

            default: Debug.Log("CASE MISSING: " + DataType); break;
        }
    }

    public void SetWorldInteractableData(DataElement searchElement, IElementData resultElementData)
    {
        var worldInteractableData = (WorldInteractableElementData)searchElement.data.elementData;

        switch (resultElementData.DataType)
        {
            case Enums.DataType.Interactable:

                var interactableData = (InteractableElementData)resultElementData;

                worldInteractableData.InteractableId = interactableData.Id;

                worldInteractableData.objectGraphicId = interactableData.ObjectGraphicId;

                worldInteractableData.objectGraphicPath = interactableData.objectGraphicPath;

                worldInteractableData.interactableName = interactableData.Name;
                worldInteractableData.objectGraphicIconPath = interactableData.objectGraphicIconPath;
                worldInteractableData.objectGraphicPath = interactableData.objectGraphicPath;

                break;
        }
    }

    public void SetWorldObjectData(DataElement searchElement, IElementData resultElementData)
    {
        var worldObjectData = (WorldObjectElementData)searchElement.data.elementData;

        switch (resultElementData.DataType)
        {
            case Enums.DataType.ObjectGraphic:

                var objectGraphicData = (ObjectGraphicElementData)resultElementData;

                worldObjectData.ObjectGraphicId = objectGraphicData.Id;

                worldObjectData.objectGraphicPath = objectGraphicData.Path;

                worldObjectData.objectGraphicName = objectGraphicData.Name;
                worldObjectData.objectGraphicIconPath = objectGraphicData.iconPath;
                
                worldObjectData.height = objectGraphicData.Height;
                worldObjectData.width = objectGraphicData.Width;
                worldObjectData.depth = objectGraphicData.Depth;

                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}
