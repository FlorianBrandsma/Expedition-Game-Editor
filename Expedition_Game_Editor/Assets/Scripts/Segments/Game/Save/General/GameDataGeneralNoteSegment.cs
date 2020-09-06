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
        var chapterSaveData = (ChapterSaveElementData)DataEditor.ElementData;

        if (noteType == NoteType.Public)
            notes = chapterSaveData.PublicNotes;
        else
            notes = chapterSaveData.PrivateNotes;
    }

    private void InitializePhaseSaveData()
    {
        var phaseSaveData = (PhaseSaveElementData)DataEditor.ElementData;

        if (noteType == NoteType.Public)
            notes = phaseSaveData.PublicNotes;
        else
            notes = phaseSaveData.PrivateNotes;
    }

    private void InitializeQuestSaveData()
    {
        var questSaveData = (QuestSaveElementData)DataEditor.ElementData;

        if (noteType == NoteType.Public)
            notes = questSaveData.PublicNotes;
        else
            notes = questSaveData.PrivateNotes;
    }

    private void InitializeObjectiveSaveData()
    {
        var objectiveSaveData = (ObjectiveSaveElementData)DataEditor.ElementData;

        if (noteType == NoteType.Public)
            notes = objectiveSaveData.PublicNotes;
        else
            notes = objectiveSaveData.PrivateNotes;
    }

    private void InitializeTaskSaveData()
    {
        var taskSaveData = (TaskSaveElementData)DataEditor.ElementData;

        if (noteType == NoteType.Public)
            notes = taskSaveData.PublicNotes;
        else
            notes = taskSaveData.PrivateNotes;
    }

    private void InitializeInteractionSaveData()
    {
        var interactionSaveData = (InteractionSaveElementData)DataEditor.ElementData;

        if (noteType == NoteType.Public)
            notes = interactionSaveData.PublicNotes;
        else
            notes = interactionSaveData.PrivateNotes;
    }

    public void OpenSegment()
    {
        notesText.text = notes;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
