using UnityEngine;
using UnityEngine.UI;

public class GameDataGeneralNoteSegment : MonoBehaviour, ISegment
{
    public Enums.NoteType noteType;

    public Text notesText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }
    
    public string Notes
    {
        get { return noteType == Enums.NoteType.Editor ? EditorNotes : GameNotes; }
    }

    #region Data properties
    public string EditorNotes
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.ChapterSave:
                    return ((ChapterSaveEditor)DataEditor).EditorNotes;

                case Enums.DataType.PhaseSave:
                    return ((PhaseSaveEditor)DataEditor).EditorNotes;

                case Enums.DataType.QuestSave:
                    return ((QuestSaveEditor)DataEditor).EditorNotes;

                case Enums.DataType.ObjectiveSave:
                    return ((ObjectiveSaveEditor)DataEditor).EditorNotes;

                case Enums.DataType.TaskSave:
                    return ((TaskSaveEditor)DataEditor).EditorNotes;

                case Enums.DataType.InteractionSave:
                    return ((InteractionSaveEditor)DataEditor).EditorNotes;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
    }

    public string GameNotes
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.ChapterSave:
                    return ((ChapterSaveEditor)DataEditor).GameNotes;

                case Enums.DataType.PhaseSave:
                    return ((PhaseSaveEditor)DataEditor).GameNotes;

                case Enums.DataType.QuestSave:
                    return ((QuestSaveEditor)DataEditor).GameNotes;

                case Enums.DataType.ObjectiveSave:
                    return ((ObjectiveSaveEditor)DataEditor).GameNotes;

                case Enums.DataType.TaskSave:
                    return ((TaskSaveEditor)DataEditor).GameNotes;

                case Enums.DataType.InteractionSave:
                    return ((InteractionSaveEditor)DataEditor).GameNotes;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData()
    {
        InitializeDependencies();
    }

    public void InitializeSegment() { }
    
    public void OpenSegment()
    {
        notesText.text = Notes;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
