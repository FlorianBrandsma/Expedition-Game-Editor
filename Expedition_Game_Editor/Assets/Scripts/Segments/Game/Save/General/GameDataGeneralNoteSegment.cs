using UnityEngine;
using UnityEngine.UI;

public class GameDataGeneralNoteSegment : MonoBehaviour, ISegment
{
    public enum NoteType
    {
        Public,
        Private
    }

    public NoteType noteType;

    public Text notesText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }
    
    public string Notes
    {
        get { return noteType == NoteType.Public ? PublicNotes : PrivateNotes; }
    }

    #region Data properties
    public string PublicNotes
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.ChapterSave:
                    return ((ChapterSaveEditor)DataEditor).PublicNotes;

                case Enums.DataType.PhaseSave:
                    return ((PhaseSaveEditor)DataEditor).PublicNotes;

                case Enums.DataType.QuestSave:
                    return ((QuestSaveEditor)DataEditor).PublicNotes;

                case Enums.DataType.ObjectiveSave:
                    return ((ObjectiveSaveEditor)DataEditor).PublicNotes;

                case Enums.DataType.TaskSave:
                    return ((TaskSaveEditor)DataEditor).PublicNotes;

                case Enums.DataType.InteractionSave:
                    return ((InteractionSaveEditor)DataEditor).PublicNotes;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
    }

    public string PrivateNotes
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.ChapterSave:
                    return ((ChapterSaveEditor)DataEditor).PrivateNotes;

                case Enums.DataType.PhaseSave:
                    return ((PhaseSaveEditor)DataEditor).PrivateNotes;

                case Enums.DataType.QuestSave:
                    return ((QuestSaveEditor)DataEditor).PrivateNotes;

                case Enums.DataType.ObjectiveSave:
                    return ((ObjectiveSaveEditor)DataEditor).PrivateNotes;

                case Enums.DataType.TaskSave:
                    return ((TaskSaveEditor)DataEditor).PrivateNotes;

                case Enums.DataType.InteractionSave:
                    return ((InteractionSaveEditor)DataEditor).PrivateNotes;

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
