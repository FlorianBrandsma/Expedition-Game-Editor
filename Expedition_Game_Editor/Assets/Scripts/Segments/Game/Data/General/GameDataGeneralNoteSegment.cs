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
            case Enums.DataType.ChapterSave:    InitializeChapterSaveData();    break;
            //case Enums.DataType.PhaseSave:      InitializePhaseSaveData();      break;
            //case Enums.DataType.QuestSave:      InitializeQuestSaveData();      break;
            //case Enums.DataType.ObjectiveSave:  InitializeObjectiveSaveData();  break;
            //case Enums.DataType.TaskSave:       InitializeTaskSaveData();       break;
            //case Enums.DataType.InteractionSave:InitializeInteractionSaveData();break;

            default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
        }
    }

    private void InitializeChapterSaveData()
    {
        var chapterData = (ChapterSaveDataElement)DataEditor.Data.dataElement;

        if (noteType == NoteType.Public)
            notes = chapterData.publicNotes;
        else
            notes = chapterData.privateNotes;
    }

    //private void InitializePhaseSaveData()
    //{
    //    var phaseData = (PhaseSaveDataElement)DataEditor.Data.dataElement;

    //    if (noteType == NoteType.Public)
    //        notes = phaseData.PublicNotes;
    //    else
    //        notes = phaseData.PrivateNotes;
    //}

    //private void InitializeQuestSaveData()
    //{
    //    var questData = (QuestSaveDataElement)DataEditor.Data.dataElement;

    //    if (noteType == NoteType.Public)
    //        notes = questData.PublicNotes;
    //    else
    //        notes = questData.PrivateNotes;
    //}

    //private void InitializeObjectiveSaveData()
    //{
    //    var objectiveData = (ObjectiveSaveDataElement)DataEditor.Data.dataElement;

    //    if (noteType == NoteType.Public)
    //        notes = objectiveData.PublicNotes;
    //    else
    //        notes = objectiveData.PrivateNotes;
    //}

    //private void InitializeTaskSaveData()
    //{
    //    var taskData = (TaskSaveDataElement)DataEditor.Data.dataElement;

    //    if (noteType == NoteType.Public)
    //        notes = taskData.PublicNotes;
    //    else
    //        notes = taskData.PrivateNotes;
    //}

    //private void InitializeInteractionSaveData()
    //{
    //    var interactionData = (InteractionSaveDataElement)DataEditor.Data.dataElement;

    //    if (noteType == NoteType.Public)
    //        notes = interactionData.PublicNotes;
    //    else
    //        notes = interactionData.PrivateNotes;
    //}

    public void OpenSegment()
    {
        notesText.text = notes;
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
