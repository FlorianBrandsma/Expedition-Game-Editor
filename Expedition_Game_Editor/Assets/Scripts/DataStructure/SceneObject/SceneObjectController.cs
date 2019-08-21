using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SceneObjectController : MonoBehaviour, IDataController
{
    public Search.ObjectGraphic searchParameters;

    public SceneObjectDataManager sceneObjectDataManager = new SceneObjectDataManager();

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

    public void InitializeController()
    {
        sceneObjectDataManager.InitializeManager(this);
    }

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return sceneObjectDataManager.GetSceneObjectDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, SelectionElement.Data resultData)
    {
        var searchElementData = (SceneObjectDataElement)searchElement.data.dataElement;

        var sceneObjectDataElement = DataList.Cast<SceneObjectDataElement>().Where(x => x.id == searchElementData.id).FirstOrDefault();

        switch (resultData.dataController.DataType)
        {
            case Enums.DataType.ObjectGraphic:

                var resultElementData = (ObjectGraphicDataElement)resultData.dataElement;

                sceneObjectDataElement.ObjectGraphicId = resultElementData.id;
                sceneObjectDataElement.objectGraphicName = resultElementData.Name;
                sceneObjectDataElement.objectGraphicIconPath = resultElementData.iconPath;

                break;
        }

        searchElement.data.dataElement = sceneObjectDataElement;
    }

    public void ToggleElement(IDataElement dataElement) { }
}