using UnityEngine;
using System.Collections;

public class SearchResultSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }
    
    public void InitializeDependencies() { }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.DataController == null) return;
        
        var searchProperties = SegmentController.EditorController.PathController.route.data.searchProperties;

        SegmentController.DataController.DataList = RenderManager.GetData(SegmentController.DataController, searchProperties);
    }

    public void OpenSegment()
    {
        if (SegmentController.DataController == null) return;

        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement selectionElement) { }
}
