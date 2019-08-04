﻿using UnityEngine;
using UnityEngine.UI;

public class GeneralNotesSegment : MonoBehaviour, ISegment
{
    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    #region UI

    public InputField inputField;

    #endregion

    #region Data Variables

    private int id;
    private int index;
    private string notes;
    private string icon;

    #endregion

    #region Data Properties
    public string Notes
    {
        get { return notes; }
        set
        {
            notes = value;

            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Chapter:

                    var chapterData = (ChapterDataElement)DataEditor.Data.dataElement;
                    chapterData.Notes = value;

                    break;

                case Enums.DataType.Phase:

                    var phaseData = (PhaseDataElement)DataEditor.Data.dataElement;
                    phaseData.Notes = value;

                    break;

                case Enums.DataType.Quest:

                    var questData = (QuestDataElement)DataEditor.Data.dataElement;
                    questData.Notes = value;

                    break;

                case Enums.DataType.Objective:

                    var objectiveData = (ObjectiveDataElement)DataEditor.Data.dataElement;
                    objectiveData.Notes = value;

                    break;

                default: Debug.Log("CASE MISSING"); break;
            }
        }
    }
    #endregion

    #region Data Methods
    public void UpdateNotes()
    {
        Notes = inputField.text;
        DataEditor.UpdateEditor();
    }
    #endregion

    #region Segment

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeDependencies();

        InitializeData();
    }

    public void InitializeData()
    {
        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Chapter:    InitializeChapterData();    break;
            case Enums.DataType.Phase:      InitializePhaseData();      break;
            case Enums.DataType.Quest:      InitializeQuestData();      break;
            case Enums.DataType.Objective:  InitializeObjectiveData();  break;
            default:                        Debug.Log("CASE MISSING");  break;
        }
    }

    private void InitializeChapterData()
    {
        var chapterData = (ChapterDataElement)DataEditor.Data.dataElement;

        notes = chapterData.Notes;
    }

    private void InitializePhaseData()
    {
        var phaseData = (PhaseDataElement)DataEditor.Data.dataElement;

        notes = phaseData.Notes;
    }

    private void InitializeQuestData()
    {
        var questData = (QuestDataElement)DataEditor.Data.dataElement;

        notes = questData.Notes;
    }

    private void InitializeObjectiveData()
    {
        var objectiveData = (ObjectiveDataElement)DataEditor.Data.dataElement;

        notes = objectiveData.Notes;
    }

    public void OpenSegment()
    {
        inputField.text = notes;

        gameObject.SetActive(true);
    }

    public void ApplySegment()
    {

    }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }

    public void SetSearchResult(SelectionElement selectionElement)
    {

    }
    #endregion
}
