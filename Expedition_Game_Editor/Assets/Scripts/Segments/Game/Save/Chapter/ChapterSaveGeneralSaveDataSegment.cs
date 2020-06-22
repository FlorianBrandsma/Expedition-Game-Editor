using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterSaveGeneralSaveDataSegment : MonoBehaviour, ISegment
{
    private ChapterSaveDataElement ChapterSaveData { get { return (ChapterSaveDataElement)DataEditor.Data.dataElement; } }

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    //UI
    public ExToggle completeToggle;

    public void UpdateComplete()
    {
        ChapterSaveData.Complete = completeToggle.Toggle.isOn;

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
        completeToggle.Toggle.isOn = ChapterSaveData.Complete;
    }

    public void ApplySegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement selectionElement) { }
}
