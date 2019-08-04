using UnityEngine;
using System.Collections.Generic;

public class SpoilSegment : MonoBehaviour, ISegment
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

        if (!SegmentController.loaded && !SegmentController.editorController.PathController.loaded)
        {
            var searchParameters = new Search.Item();
            searchParameters.type = new List<int>() { (int)Enums.ItemType.Spoils };

            SegmentController.DataController.DataList = SegmentController.DataController.GetData(new[] { searchParameters });
        }
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
