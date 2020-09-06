using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSaveGeneralSaveDataSegment : MonoBehaviour, ISegment
{
    private InteractionSaveElementData InteractionSaveData { get { return (InteractionSaveElementData)DataEditor.ElementData; } }

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    //UI
    public ExToggle completeToggle;

    public void UpdateComplete()
    {
        InteractionSaveData.Complete = completeToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;
    }

    public void InitializeData() { }

    public void InitializeSegment() { }
    
    public void OpenSegment()
    {
        completeToggle.Toggle.isOn = InteractionSaveData.Complete;
    }

    public void ApplySegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
