using UnityEngine;
using System.Collections.Generic;

public class TerrainElementSegment : MonoBehaviour, ISegment
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

        InitializeTerrainElementData();
    }

    private void InitializeTerrainElementData()
    {
        if (SegmentController.editorController.pathController.loaded) return;

        var searchParameters = new Search.TerrainElement();
        searchParameters.requestType = Search.TerrainElement.RequestType.GetQuestAndObjectiveElements;

        SegmentController.DataController.GetData(new[] { searchParameters });
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
