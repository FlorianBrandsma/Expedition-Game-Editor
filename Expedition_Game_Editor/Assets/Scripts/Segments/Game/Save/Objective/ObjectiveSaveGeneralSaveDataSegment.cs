using UnityEngine;

public class ObjectiveSaveGeneralSaveDataSegment : MonoBehaviour, ISegment
{
    public ExToggle completeToggle;

    public SegmentController SegmentController      { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                       { get; set; }

    public ObjectiveSaveEditor ObjectiveSaveEditor  { get { return (ObjectiveSaveEditor)DataEditor; } }
    
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;
    }

    public void InitializeData() { }

    public void InitializeSegment() { }
    
    public void OpenSegment()
    {
        completeToggle.Toggle.isOn = ObjectiveSaveEditor.Complete;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateComplete()
    {
        ObjectiveSaveEditor.Complete = completeToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
