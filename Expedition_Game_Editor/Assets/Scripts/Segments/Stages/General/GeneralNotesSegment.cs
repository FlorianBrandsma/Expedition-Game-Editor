using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GeneralNotesSegment : MonoBehaviour, ISegment
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
    public InputField inputField;
    
    //Data Variables
    private string notes;

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

                    if (noteType == NoteType.Public)
                        chapterData.PublicNotes = value;
                    else
                        chapterData.PrivateNotes = value;

                    break;

                case Enums.DataType.Phase:

                    var phaseData = (PhaseDataElement)DataEditor.Data.dataElement;

                    if (noteType == NoteType.Public)
                        phaseData.PublicNotes = value;
                    else
                        phaseData.PrivateNotes = value;

                    break;

                case Enums.DataType.Quest:

                    var questData = (QuestDataElement)DataEditor.Data.dataElement;

                    if (noteType == NoteType.Public)
                        questData.PublicNotes = value;
                    else
                        questData.PrivateNotes = value;

                    break;

                case Enums.DataType.Objective:

                    var objectiveData = (ObjectiveDataElement)DataEditor.Data.dataElement;

                    if (noteType == NoteType.Public)
                        objectiveData.PublicNotes = value;
                    else
                        objectiveData.PrivateNotes = value;

                    break;

                case Enums.DataType.Task:

                    var taskData = (TaskDataElement)DataEditor.Data.dataElement;

                    if (noteType == NoteType.Public)
                        taskData.PublicNotes = value;
                    else
                        taskData.PrivateNotes = value;

                    break;

                case Enums.DataType.Interaction:

                    var interactionData = (InteractionDataElement)DataEditor.Data.dataElement;

                    if (noteType == NoteType.Public)
                        interactionData.PublicNotes = value;
                    else
                        interactionData.PrivateNotes = value;

                    break;

                case Enums.DataType.Atmosphere:

                    var atmosphereData = (AtmosphereDataElement)DataEditor.Data.dataElement;

                    if (noteType == NoteType.Public)
                        atmosphereData.PublicNotes = value;
                    else
                        atmosphereData.PrivateNotes = value;

                    break;

                default: Debug.Log("CASE MISSING"); break;
            }
        }
    }


    public void UpdateNotes()
    {
        Notes = inputField.text;
        DataEditor.UpdateEditor();
    }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.DataEditor;

        if(!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment() { }

    public void InitializeData()
    {
        InitializeDependencies();

        if(DataEditor.Loaded) return;

        switch (DataEditor.Data.dataController.DataType)
        {
            case Enums.DataType.Chapter:    InitializeChapterData();    break;
            case Enums.DataType.Phase:      InitializePhaseData();      break;
            case Enums.DataType.Quest:      InitializeQuestData();      break;
            case Enums.DataType.Objective:  InitializeObjectiveData();  break;
            case Enums.DataType.Task:       InitializeTaskData();       break;
            case Enums.DataType.Interaction:InitializeInteractionData();break;
            case Enums.DataType.Atmosphere: InitializeAtmosphereData(); break;

            default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType);  break;
        }
    }

    private void InitializeChapterData()
    {
        var chapterData = (ChapterDataElement)DataEditor.Data.dataElement;

        if (noteType == NoteType.Public)
            notes = chapterData.PublicNotes;
        else
            notes = chapterData.PrivateNotes;
    }

    private void InitializePhaseData()
    {
        var phaseData = (PhaseDataElement)DataEditor.Data.dataElement;

        if (noteType == NoteType.Public)
            notes = phaseData.PublicNotes;
        else
            notes = phaseData.PrivateNotes;
    }

    private void InitializeQuestData()
    {
        var questData = (QuestDataElement)DataEditor.Data.dataElement;

        if (noteType == NoteType.Public)
            notes = questData.PublicNotes;
        else
            notes = questData.PrivateNotes;
    }

    private void InitializeObjectiveData()
    {
        var objectiveData = (ObjectiveDataElement)DataEditor.Data.dataElement;

        if (noteType == NoteType.Public)
            notes = objectiveData.PublicNotes;
        else
            notes = objectiveData.PrivateNotes;
    }

    private void InitializeTaskData()
    {
        var taskData = (TaskDataElement)DataEditor.Data.dataElement;

        if (noteType == NoteType.Public)
            notes = taskData.PublicNotes;
        else
            notes = taskData.PrivateNotes;
    }

    private void InitializeInteractionData()
    {
        var interactionData = (InteractionDataElement)DataEditor.Data.dataElement;

        if (noteType == NoteType.Public)
            notes = interactionData.PublicNotes;
        else
            notes = interactionData.PrivateNotes;
    }

    private void InitializeAtmosphereData()
    {
        var atmosphereData = (AtmosphereDataElement)DataEditor.Data.dataElement;

        if (noteType == NoteType.Public)
            notes = atmosphereData.PublicNotes;
        else
            notes = atmosphereData.PrivateNotes;
    }

    public void OpenSegment()
    {
        inputField.text = notes;
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
