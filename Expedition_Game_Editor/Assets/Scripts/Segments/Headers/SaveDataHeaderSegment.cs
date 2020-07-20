﻿using UnityEngine;
using UnityEngine.UI;

public class SaveDataHeaderSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public Text idText;
    public Text headerText;
    #endregion

    #region Data Variables
    private int id;
    private int index;
    private new string name;
    #endregion

    #region Segment
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment() { }

    public void InitializeData()
    {
        InitializeDependencies();

        if (DataEditor.Loaded) return;

        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.ChapterSave:        InitializeChapterSaveData();        break;
            case Enums.DataType.PhaseSave:          InitializePhaseSaveData();          break;
            case Enums.DataType.QuestSave:          InitializeQuestSaveData();          break;
            case Enums.DataType.ObjectiveSave:      InitializeObjectiveSaveData();      break;
            case Enums.DataType.TaskSave:           InitializeTaskSaveData();           break;
            case Enums.DataType.InteractionSave:    InitializeInteractionSaveData();    break;
            

            default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
        }
    }

    private void InitializeChapterSaveData()
    {
        var chapterSaveData = (ChapterSaveElementData)DataEditor.Data.elementData;

        id = chapterSaveData.Id;
        index = chapterSaveData.Index;
        name = chapterSaveData.name;
    }

    private void InitializePhaseSaveData()
    {
        var phaseSaveData = (PhaseSaveElementData)DataEditor.Data.elementData;

        id = phaseSaveData.Id;
        index = phaseSaveData.Index;
        name = phaseSaveData.name;
    }

    private void InitializeQuestSaveData()
    {
        var questSaveData = (QuestSaveElementData)DataEditor.Data.elementData;

        id = questSaveData.Id;
        index = questSaveData.Index;
        name = questSaveData.name;
    }

    private void InitializeObjectiveSaveData()
    {
        var objectiveSaveData = (ObjectiveSaveElementData)DataEditor.Data.elementData;

        id = objectiveSaveData.Id;
        index = objectiveSaveData.Index;
        name = objectiveSaveData.name;
    }

    private void InitializeTaskSaveData()
    {
        var taskSaveData = (TaskSaveElementData)DataEditor.Data.elementData;

        id = taskSaveData.Id;
        index = taskSaveData.Index;
        name = taskSaveData.name;
    }

    private void InitializeInteractionSaveData()
    {
        var interactionSaveData = (InteractionSaveElementData)DataEditor.Data.elementData;

        id = interactionSaveData.Id;
        index = interactionSaveData.Index;
        name = interactionSaveData.isDefault ? "Default" : TimeManager.FormatTime(interactionSaveData.startTime) + " - " + TimeManager.FormatTime(interactionSaveData.endTime);
    }

    public void OpenSegment()
    {
        idText.text = id.ToString();
        headerText.text = name;

        gameObject.SetActive(true);
    }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }

    public void SetSearchResult(DataElement dataElement) { }
    #endregion
}
