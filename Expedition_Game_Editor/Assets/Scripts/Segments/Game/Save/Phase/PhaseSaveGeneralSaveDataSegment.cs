using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseSaveGeneralSaveDataSegment : MonoBehaviour, ISegment
{
    private PhaseSaveElementData PhaseSaveData { get { return (PhaseSaveElementData)DataEditor.ElementData; } }

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
        DataEditor = SegmentController.EditorController.PathController.DataEditor;
    }

    public void InitializeSegment() { }

    public void InitializeData() { }

    public void OpenSegment()
    {
        completeToggle.Toggle.isOn = PhaseSaveData.Complete;
    }

    public void ApplySegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
