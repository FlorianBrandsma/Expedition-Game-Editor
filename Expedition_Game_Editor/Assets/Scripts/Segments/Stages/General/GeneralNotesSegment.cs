using UnityEngine;
using UnityEngine.UI;

public class GeneralNotesSegment : MonoBehaviour, ISegment
{
    public Enums.NoteType noteType;

    public InputField inputField;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public string Notes
    {
        get { return noteType == Enums.NoteType.Editor ? EditorNotes : GameNotes; }
        set
        {
            if (noteType == Enums.NoteType.Editor)
                EditorNotes = value;
            else
                GameNotes = value;
        }
    }

    #region Data properties
    public string EditorNotes
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Atmosphere:
                    return ((AtmosphereEditor)DataEditor).EditorNotes;

                case Enums.DataType.Chapter:
                    return ((ChapterEditor)DataEditor).EditorNotes;

                case Enums.DataType.Phase:
                    return ((PhaseEditor)DataEditor).EditorNotes;

                case Enums.DataType.Quest:
                    return ((QuestEditor)DataEditor).EditorNotes;

                case Enums.DataType.Objective:
                    return ((ObjectiveEditor)DataEditor).EditorNotes;

                case Enums.DataType.Task:
                    return ((TaskEditor)DataEditor).EditorNotes;

                case Enums.DataType.Interaction:
                    return ((InteractionEditor)DataEditor).EditorNotes;

                case Enums.DataType.Outcome:
                    return ((OutcomeEditor)DataEditor).EditorNotes;

                case Enums.DataType.Scene:
                    return ((SceneEditor)DataEditor).EditorNotes;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Atmosphere:

                    var atmosphereEditor = (AtmosphereEditor)DataEditor;
                    atmosphereEditor.EditorNotes = value;

                    break;

                case Enums.DataType.Chapter:

                    var chapterEditor = (ChapterEditor)DataEditor;
                    chapterEditor.EditorNotes = value;

                    break;

                case Enums.DataType.Phase:

                    var phaseEditor = (PhaseEditor)DataEditor;
                    phaseEditor.EditorNotes = value;

                    break;

                case Enums.DataType.Quest:

                    var questEditor = (QuestEditor)DataEditor;
                    questEditor.EditorNotes = value;

                    break;

                case Enums.DataType.Objective:

                    var objectiveEditor = (ObjectiveEditor)DataEditor;
                    objectiveEditor.EditorNotes = value;

                    break;

                case Enums.DataType.Task:

                    var taskEditor = (TaskEditor)DataEditor;
                    taskEditor.EditorNotes = value;

                    break;

                case Enums.DataType.Interaction:

                    var interactionEditor = (InteractionEditor)DataEditor;
                    interactionEditor.EditorNotes = value;

                    break;

                case Enums.DataType.Outcome:

                    var outcomeEditor = (OutcomeEditor)DataEditor;
                    outcomeEditor.EditorNotes = value;

                    break;

                case Enums.DataType.Scene:

                    var sceneEditor = (SceneEditor)DataEditor;
                    sceneEditor.EditorNotes = value;

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }

    public string GameNotes
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Atmosphere:
                    return ((AtmosphereEditor)DataEditor).GameNotes;

                case Enums.DataType.Chapter:
                    return ((ChapterEditor)DataEditor).GameNotes;

                case Enums.DataType.Phase:
                    return ((PhaseEditor)DataEditor).GameNotes;

                case Enums.DataType.Quest:
                    return ((QuestEditor)DataEditor).GameNotes;

                case Enums.DataType.Objective:
                    return ((ObjectiveEditor)DataEditor).GameNotes;

                case Enums.DataType.Task:
                    return ((TaskEditor)DataEditor).GameNotes;

                case Enums.DataType.Interaction:
                    return ((InteractionEditor)DataEditor).GameNotes;

                case Enums.DataType.Outcome:
                    return ((OutcomeEditor)DataEditor).GameNotes;

                case Enums.DataType.Scene:
                    return ((SceneEditor)DataEditor).GameNotes;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Atmosphere:

                    var atmosphereEditor = (AtmosphereEditor)DataEditor;
                    atmosphereEditor.GameNotes = value;

                    break;

                case Enums.DataType.Chapter:

                    var chapterEditor = (ChapterEditor)DataEditor;
                    chapterEditor.GameNotes = value;

                    break;

                case Enums.DataType.Phase:

                    var phaseEditor = (PhaseEditor)DataEditor;
                    phaseEditor.GameNotes = value;

                    break;

                case Enums.DataType.Quest:

                    var questEditor = (QuestEditor)DataEditor;
                    questEditor.GameNotes = value;

                    break;

                case Enums.DataType.Objective:

                    var objectiveEditor = (ObjectiveEditor)DataEditor;
                    objectiveEditor.GameNotes = value;

                    break;

                case Enums.DataType.Task:

                    var taskEditor = (TaskEditor)DataEditor;
                    taskEditor.GameNotes = value;

                    break;

                case Enums.DataType.Interaction:

                    var interactionEditor = (InteractionEditor)DataEditor;
                    interactionEditor.GameNotes = value;

                    break;

                case Enums.DataType.Outcome:

                    var outcomeEditor = (OutcomeEditor)DataEditor;
                    outcomeEditor.GameNotes = value;

                    break;

                case Enums.DataType.Scene:

                    var sceneEditor = (SceneEditor)DataEditor;
                    sceneEditor.GameNotes = value;

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if(!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData()
    {
        InitializeDependencies();

        if (DataEditor.Loaded) return;
    }

    public void InitializeSegment() { }
    
    public void OpenSegment()
    {
        inputField.text = Notes;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateNotes()
    {
        Notes = inputField.text;
        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
