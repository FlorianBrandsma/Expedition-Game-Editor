using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SceneController : MonoBehaviour, IDataController
{
    public Search.Region searchParameters;

    private SceneDataManager sceneDataManager   = new SceneDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Scene; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Region>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        sceneDataManager.InitializeManager(this);
    }

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return sceneDataManager.GetSceneDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData)
    {
        switch (((GeneralData)searchElement.data.dataElement).dataType)
        { 
            case Enums.DataType.SceneInteractable:  SetSceneInteractableData(searchElement, resultData);    break;
            case Enums.DataType.SceneObject:        SetSceneObjectData(searchElement, resultData);          break;
        }
    }

    private void SetSceneInteractableData(SelectionElement searchElement, IDataElement resultData)
    {
        var sceneInteractableData = (SceneInteractableDataElement)searchElement.data.dataElement;

        switch (((GeneralData)resultData).dataType)
        {
            case Enums.DataType.Interactable:

                var resultElementData = (InteractableDataElement)resultData;

                sceneInteractableData.objectGraphicId = resultElementData.id;
                sceneInteractableData.objectGraphicIconPath = resultElementData.objectGraphicIconPath;
                sceneInteractableData.objectGraphicPath = resultElementData.objectGraphicPath;

                break;
        }
    }

    private void SetSceneObjectData(SelectionElement searchElement, IDataElement resultData)
    {
        var sceneObjectData = (SceneObjectDataElement)searchElement.data.dataElement;

        switch (((GeneralData)resultData).dataType)
        {
            case Enums.DataType.ObjectGraphic:
                
                var resultElementData = (ObjectGraphicDataElement)resultData;

                sceneObjectData.ObjectGraphicId = resultElementData.id;
                sceneObjectData.objectGraphicName = resultElementData.Name;
                sceneObjectData.objectGraphicIconPath = resultElementData.iconPath;
                sceneObjectData.objectGraphicPath = resultElementData.Path;

                break;
        }
    }

    public void ToggleElement(IDataElement dataElement) { }
}
