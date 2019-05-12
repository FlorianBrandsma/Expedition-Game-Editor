using UnityEngine;
using System.Collections;

public class SearchResultSegment : MonoBehaviour, ISegment
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

        InitializeData();
    }

    private void InitializeData()
    {
        if (SegmentController.DataController == null) return;
        
        var searchParameters = SegmentController.editorController.pathController.route.data.SearchParameters;

        SegmentController.DataController.GetData(searchParameters);
    }

    public void OpenSegment()
    {
        if (SegmentController.DataController == null) return;

        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
