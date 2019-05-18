using UnityEngine;
using System.Collections.Generic;

public class PhaseSegment : MonoBehaviour, ISegment
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

        InitializePhaseData();
    }

    private void InitializePhaseData()
    {
        if (SegmentController.editorController.pathController.loaded) return;

        var searchParameters = new Search.Phase();

        searchParameters.chapterId = new List<int>() { SegmentController.path.FindLastRoute("Chapter").GeneralData().id };
        searchParameters.temp_id_count = 15;
        
        SegmentController.DataController.GetData(new[] { searchParameters });
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {

    }
}
