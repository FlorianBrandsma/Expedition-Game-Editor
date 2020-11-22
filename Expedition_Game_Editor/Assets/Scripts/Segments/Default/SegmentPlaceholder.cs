using UnityEngine;
using System.Collections;

public class SegmentPlaceholder : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;
    }

    public void InitializeSegment() { }

    public void InitializeData() { }

    public void OpenSegment() { }

    public void ApplySegment() { }

    public void CloseSegment() { }

    public void UpdateSegment() { }

    public void SetSearchResult(IElementData elementData) { }
}
