using UnityEngine;
using System.Collections.Generic;

public class ObjectiveSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {

    }

    public void InitializeSegment()
    {
        DataEditor = SegmentController.editorController.pathController.dataEditor;

        InitializeObjectiveData();
    }

    private void InitializeObjectiveData()
    {
        if (SegmentController.editorController.pathController.loaded) return;

        var searchParameters = new Search.Objective();

        searchParameters.questId = new List<int>() { SegmentController.path.FindLastRoute("Quest").GeneralData().id };

        SegmentController.DataController.GetData(new[] { searchParameters });
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
