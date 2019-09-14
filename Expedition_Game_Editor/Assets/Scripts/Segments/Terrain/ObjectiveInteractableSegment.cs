using UnityEngine;
using System.Collections.Generic;

public class ObjectiveInteractableSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

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
        if (SegmentController.editorController.PathController.loaded) return;

        var searchParameters = new Search.SceneInteractable();

        searchParameters.requestType = Search.SceneInteractable.RequestType.GetQuestAndObjectiveInteractables;

        searchParameters.questId     = new List<int>() { SegmentController.Path.FindLastRoute(Enums.DataType.Quest).GeneralData.id };
        searchParameters.objectiveId = new List<int>() { SegmentController.Path.FindLastRoute(Enums.DataType.Objective).GeneralData.id };
        
        SegmentController.DataController.DataList = SegmentController.DataController.GetData(new[] { searchParameters });
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
