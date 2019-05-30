using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PhaseSegment : MonoBehaviour, ISegment
{
    ChapterDataElement chapterData;
    private DataManager dataManager = new DataManager();
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

        chapterData = (ChapterDataElement)SegmentController.editorController.pathController.route.data.DataElement;

        InitializePhaseData();
    }

    private void InitializePhaseData()
    {
        if (SegmentController.editorController.pathController.loaded) return;

        var searchParameters = new Search.Phase();

        searchParameters.chapterId = new List<int>() { chapterData.id };

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
