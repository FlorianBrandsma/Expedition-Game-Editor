using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSaveGeneralSaveDataSegment : MonoBehaviour, ISegment
{
    private QuestSaveDataElement QuestSaveData { get { return (QuestSaveDataElement)DataEditor.Data.dataElement; } }

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    //UI
    public ExToggle completeToggle;

    public void UpdateComplete()
    {
        QuestSaveData.Complete = completeToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.DataEditor;
    }

    public void InitializeSegment() { }

    public void InitializeData() { }

    public void OpenSegment()
    {
        completeToggle.Toggle.isOn = QuestSaveData.Complete;
    }

    public void ApplySegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
