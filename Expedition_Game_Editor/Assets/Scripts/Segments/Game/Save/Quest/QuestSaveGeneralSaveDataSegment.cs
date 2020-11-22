using UnityEngine;

public class QuestSaveGeneralSaveDataSegment : MonoBehaviour, ISegment
{
    public ExToggle completeToggle;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public QuestSaveEditor QuestSaveEditor      { get { return (QuestSaveEditor)DataEditor; } }
    
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;
    }

    public void InitializeData() { }

    public void InitializeSegment() { }
    
    public void OpenSegment()
    {
        completeToggle.Toggle.isOn = QuestSaveEditor.Complete;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateComplete()
    {
        QuestSaveEditor.Complete = completeToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
