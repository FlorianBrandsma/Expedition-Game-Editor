using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseSaveGeneralSaveDataSegment : MonoBehaviour, ISegment
{
    private PhaseSaveDataElement PhaseSaveData { get { return (PhaseSaveDataElement)DataEditor.Data.dataElement; } }

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    //UI
    public ExToggle completeToggle;

    public void UpdateComplete()
    {
        PhaseSaveData.Complete = completeToggle.Toggle.isOn;

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
        completeToggle.Toggle.isOn = PhaseSaveData.Complete;
    }

    public void ApplySegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
