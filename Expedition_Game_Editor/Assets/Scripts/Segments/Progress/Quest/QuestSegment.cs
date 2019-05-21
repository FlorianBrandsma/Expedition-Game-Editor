using UnityEngine;
using System.Collections.Generic;

public class QuestSegment : MonoBehaviour, ISegment
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

        InitializeQuestData();
    }

    private void InitializeQuestData()
    {
        if (SegmentController.editorController.pathController.loaded) return;

        var searchParameters = new Search.Quest();

        searchParameters.phaseId = new List<int>() { SegmentController.path.FindLastRoute("Phase").GeneralData().id };

        SegmentController.DataController.GetData(new[] { searchParameters });
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
