using UnityEngine;
using System.Collections;

public class SearchResultSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }
    
    public void InitializeDependencies() { }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.DataController == null) return;
        
        var searchParameters = SegmentController.editorController.PathController.route.data.searchParameters;

        SegmentController.DataController.DataList = SegmentController.DataController.GetData(searchParameters);
    }

    public void OpenSegment()
    {
        if (SegmentController.DataController == null) return;

        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
