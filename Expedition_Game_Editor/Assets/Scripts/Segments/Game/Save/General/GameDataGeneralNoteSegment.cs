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

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    //UI
    public Text notesText;
    
    //Data Variables
    private string notes;

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.DataEditor;

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
        var chapterSaveData = (ChapterSaveDataElement)DataEditor.Data.dataElement;

        if (noteType == NoteType.Public)
            notes = chapterSaveData.publicNotes;
        else
            notes = chapterSaveData.privateNotes;
    }

    private void InitializePhaseSaveData()
    {
        var phaseSaveData = (PhaseSaveDataElement)DataEditor.Data.dataElement;

        if (noteType == NoteType.Public)
            notes = phaseSaveData.publicNotes;
        else
            notes = phaseSaveData.privateNotes;
    }

    private void InitializeQuestSaveData()
    {
        var questSaveData = (QuestSaveDataElement)DataEditor.Data.dataElement;

        if (noteType == NoteType.Public)
            notes = questSaveData.publicNotes;
        else
            notes = questSaveData.privateNotes;
    }

    private void InitializeObjectiveSaveData()
    {
        var objectiveSaveData = (ObjectiveSaveDataElement)DataEditor.Data.dataElement;

        if (noteType == NoteType.Public)
            notes = objectiveSaveData.publicNotes;
        else
            notes = objectiveSaveData.privateNotes;
    }

    private void InitializeTaskSaveData()
    {
        var taskSaveData = (TaskSaveDataElement)DataEditor.Data.dataElement;

        if (noteType == NoteType.Public)
            notes = taskSaveData.publicNotes;
        else
            notes = taskSaveData.privateNotes;
    }

    private void InitializeInteractionSaveData()
    {
        var interactionSaveData = (InteractionSaveDataElement)DataEditor.Data.dataElement;

        if (noteType == NoteType.Public)
            notes = interactionSaveData.publicNotes;
        else
            notes = interactionSaveData.privateNotes;
    }

    public void OpenSegment()
    {
        notesText.text = notes;
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
