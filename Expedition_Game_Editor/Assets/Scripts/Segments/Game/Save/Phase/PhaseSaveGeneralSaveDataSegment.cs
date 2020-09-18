using UnityEngine;

public class PhaseSaveGeneralSaveDataSegment : MonoBehaviour, ISegment
{
    public ExToggle completeToggle;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public PhaseSaveEditor PhaseSaveEditor { get { return (PhaseSaveEditor)DataEditor; } }
    
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;
    }

    public void InitializeData() { }

    public void InitializeSegment() { }
    
    public void OpenSegment()
    {
        completeToggle.Toggle.isOn = PhaseSaveEditor.Complete;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateComplete()
    {
        PhaseSaveEditor.Complete = completeToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void CloseSegment() { }
}
