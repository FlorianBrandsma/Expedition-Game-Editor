using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveSaveGeneralSaveDataSegment : MonoBehaviour, ISegment
{
    private ObjectiveSaveElementData ObjectiveSaveData { get { return (ObjectiveSaveElementData)DataEditor.Data.elementData; } }

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    //UI
    public ExToggle completeToggle;

    public void UpdateComplete()
    {
        ObjectiveSaveData.Complete = completeToggle.Toggle.isOn;

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
        completeToggle.Toggle.isOn = ObjectiveSaveData.Complete;
    }

    public void ApplySegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
