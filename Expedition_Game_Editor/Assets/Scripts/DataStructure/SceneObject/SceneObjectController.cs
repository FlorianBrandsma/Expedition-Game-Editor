using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SceneObjectController : MonoBehaviour, IDataController
{
    public Search.ObjectGraphic searchParameters;

    public SceneObjectDataManager sceneObjectDataManager;

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.SceneObject; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.ObjectGraphic>().FirstOrDefault(); }
    }

    public SceneObjectController()
    {
        sceneObjectDataManager = new SceneObjectDataManager(this);
    }

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return sceneObjectDataManager.GetSceneObjectDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData)
    {
        var sceneObjectData = (SceneObjectDataElement)searchElement.data.dataElement;
        
        switch (((GeneralData)resultData).DataType)
        {
            case Enums.DataType.ObjectGraphic:

                var resultElementData = (ObjectGraphicDataElement)resultData;

                sceneObjectData.ObjectGraphicId = resultElementData.Id;
                sceneObjectData.objectGraphicName = resultElementData.Name;
                sceneObjectData.objectGraphicIconPath = resultElementData.iconPath;
                sceneObjectData.objectGraphicPath = resultElementData.Path;

                break;
        }
    }

    public void ToggleElement(IDataElement dataElement) { }
}