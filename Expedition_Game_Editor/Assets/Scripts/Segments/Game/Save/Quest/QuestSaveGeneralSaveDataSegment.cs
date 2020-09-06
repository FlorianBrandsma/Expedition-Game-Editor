﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestSaveGeneralSaveDataSegment : MonoBehaviour, ISegment
{
    private QuestSaveElementData QuestSaveData { get { return (QuestSaveElementData)DataEditor.ElementData; } }

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
        DataEditor = SegmentController.EditorController.PathController.DataEditor;
    }

    public void InitializeSegment() { }

    public void InitializeData() { }

    public void OpenSegment()
    {
        completeToggle.Toggle.isOn = QuestSaveData.Complete;
    }

    public void ApplySegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
