using UnityEngine;
using System.Collections;

public class SegmentPlaceholder : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.dataEditor;
    }

    public void InitializeSegment()
    {
        
    }

    public void InitializeData()
    {

    }

    public void OpenSegment()
    {

    }

    public void Activate(EditorController new_controller)
    {

    }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {

    }

    public void SetSearchResult(SelectionElement selectionElement)
    {

    }
}
