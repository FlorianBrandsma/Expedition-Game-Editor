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

    public IEnumerable SearchParameters { get; set; }

    public DataController(Enums.DataType dataType)
    {
        DataType = dataType;

        switch(DataType)
        {
            case Enums.DataType.SceneInteractable:  DataManager = new SceneInteractableDataManager(this);   break;
            case Enums.DataType.Interaction:        DataManager = new InteractionDataManager(this);         break;
            case Enums.DataType.SceneObject:        DataManager = new SceneObjectDataManager(this);         break;
        }
    }

    public void SetData(SelectionElement searchElement, IDataElement resultDataElement)
    {
        switch(DataType)
        {
            case Enums.DataType.SceneInteractable:  SetSceneInteractableData(searchElement, resultDataElement); break;
            case Enums.DataType.SceneObject:        SetSceneObjectData(searchElement, resultDataElement);       break;

            default: Debug.Log("CASE MISSING: " + DataType); break;
        }
    }

    public void SetSceneInteractableData(SelectionElement searchElement, IDataElement resultDataElement)
    {
        var sceneInteractableData = (SceneInteractableDataElement)searchElement.data.dataElement;

        switch (resultDataElement.DataType)
        {
            case Enums.DataType.Interactable:

                var interactableData = (InteractableDataElement)resultDataElement;

                sceneInteractableData.InteractableId = interactableData.Id;
                sceneInteractableData.objectGraphicId = interactableData.ObjectGraphicId;
                sceneInteractableData.interactableName = interactableData.Name;
                sceneInteractableData.objectGraphicIconPath = interactableData.objectGraphicIconPath;
                sceneInteractableData.objectGraphicPath = interactableData.objectGraphicPath;

                break;
        }
    }

    public void SetSceneObjectData(SelectionElement searchElement, IDataElement resultDataElement)
    {
        var sceneObjectData = (SceneObjectDataElement)searchElement.data.dataElement;

        switch (resultDataElement.DataType)
        {
            case Enums.DataType.ObjectGraphic:

                var objectGraphicData = (ObjectGraphicDataElement)resultDataElement;

                sceneObjectData.ObjectGraphicId = objectGraphicData.Id;
                sceneObjectData.objectGraphicName = objectGraphicData.Name;
                sceneObjectData.objectGraphicIconPath = objectGraphicData.iconPath;
                sceneObjectData.objectGraphicPath = objectGraphicData.Path;

                sceneObjectData.height = objectGraphicData.Height;
                sceneObjectData.width = objectGraphicData.Width;
                sceneObjectData.depth = objectGraphicData.Depth;

                break;
        }
    }

    public void ToggleElement(IDataElement dataElement) { }
}
