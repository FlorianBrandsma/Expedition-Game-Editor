using UnityEngine;

public class ChapterSaveGeneralSaveDataSegment : MonoBehaviour, ISegment
{
    public ExToggle completeToggle;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public ChapterSaveEditor ChapterSaveEditor { get { return (ChapterSaveEditor)DataEditor; } }
    
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;
    }

    public void InitializeData() { }

    public void InitializeSegment() { }
    
    public void OpenSegment()
    {
        completeToggle.Toggle.isOn = ChapterSaveEditor.Complete;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateComplete()
    {
        ChapterSaveEditor.Complete = completeToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
