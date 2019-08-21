using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class SceneInteractableController : MonoBehaviour, IDataController
{
    public Search.Interactable searchParameters;

    public SceneInteractableDataManager sceneInteractableDataManager = new SceneInteractableDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.SceneInteractable; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.Navigation; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Interactable>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        sceneInteractableDataManager.InitializeManager(this);
    }

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return sceneInteractableDataManager.GetSceneInteractableDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, SelectionElement.Data resultData)
    {
        var searchElementData = (SceneInteractableDataElement)searchElement.data.dataElement;

        var sceneInteractableDataElement = DataList.Cast<SceneInteractableDataElement>().Where(x => x.id == searchElementData.id).FirstOrDefault();

        switch (resultData.dataController.DataType)
        {
            case Enums.DataType.Interactable:

                var resultElementData = (InteractableDataElement)resultData.dataElement;

                sceneInteractableDataElement.InteractableId = resultElementData.id;
                sceneInteractableDataElement.interactableName = resultElementData.Name;
                sceneInteractableDataElement.objectGraphicIconPath = resultElementData.objectGraphicIconPath;

                break;
        }

        searchElement.data.dataElement = sceneInteractableDataElement;
    }

    public void ToggleElement(IDataElement dataElement) { }
}
