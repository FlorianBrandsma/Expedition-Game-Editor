using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ObjectiveGeneralInteractableSegment : MonoBehaviour, ISegment
{
    private DataManager dataManager = new DataManager();

    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    private InteractableController ElementController { get { return (InteractableController)SegmentController.DataController; } }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {

    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        var objectiveEditor = (ObjectiveEditor)DataEditor;

        if (objectiveEditor.sceneInteractableDataList.Count > 0) return;

        var objectiveData = (ObjectiveDataElement)DataEditor.Data.dataElement;

        var searchParameters = new Search.SceneInteractable();

        searchParameters.requestType = Search.SceneInteractable.RequestType.Custom;
        searchParameters.objectiveId = new List<int>() { objectiveData.id };

        SegmentController.DataController.DataList = SegmentController.DataController.GetData(new[] { searchParameters });

        var sceneInteractableList = SegmentController.DataController.DataList.Cast<SceneInteractableDataElement>().ToList();

        sceneInteractableList.ForEach(x => objectiveEditor.sceneInteractableDataList.Add(x));
    }

    private void SetSearchParameters()
    {
        //ObjectiveDataElement objectiveData = DataEditor.Data.ElementData.Cast<ObjectiveDataElement>().FirstOrDefault();
        //var searchParameters = SegmentController.DataController.SearchParameters.Cast<Search.Element>().FirstOrDefault();

        ////Out of all the elements, select only those that are not in this list
        //var idList = SegmentController.DataController.DataList.Cast<ObjectiveElementDataElement>().Select(x => x.ElementId).ToList();
        //var list = dataManager.GetElementData().Where(x => !idList.Contains(x.id)).Select(x => x.id).Distinct().ToList();

        //searchParameters.id = list;
    }

    public void OpenSegment()
    {
        SetSearchParameters();

        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {
        var objectiveData = (ObjectiveDataElement)DataEditor.Data.dataElement;

        DataEditor.UpdateEditor();

        SetSearchParameters();
    }
}
