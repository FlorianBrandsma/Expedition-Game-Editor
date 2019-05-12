using UnityEngine;
using System.Collections;

public class GearSegment : MonoBehaviour, ISegment
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

        InitializeChapterData();
    }

    private void InitializeChapterData()
    {
        if (SegmentController.editorController.pathController.loaded) return;

        SegmentController.DataController.InitializeController();
    }

    public void OpenSegment()
    {
        //if (GetComponent<IDisplay>() != null)
        //    GetComponent<IDisplay>().SetDisplay();
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {

    }
}
