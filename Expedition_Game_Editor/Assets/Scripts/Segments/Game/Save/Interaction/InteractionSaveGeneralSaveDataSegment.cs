using UnityEngine;

public class InteractionSaveGeneralSaveDataSegment : MonoBehaviour, ISegment
{
    public ExToggle completeToggle;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public InteractionSaveEditor InteractionSaveEditor { get { return (InteractionSaveEditor)DataEditor; } }
    
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;
    }

    public void InitializeData() { }

    public void InitializeSegment() { }
    
    public void OpenSegment()
    {
        completeToggle.Toggle.isOn = InteractionSaveEditor.Complete;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateComplete()
    {
        InteractionSaveEditor.Complete = completeToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
