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

    public InputField inputField;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public string Notes
    {
        get { return noteType == NoteType.Public ? PublicNotes : PrivateNotes; }
        set
        {
            if (noteType == NoteType.Public)
                PublicNotes = value;
            else
                PrivateNotes = value;
        }
    }

    #region Data properties
    public string PublicNotes
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Atmosphere:
                    return ((AtmosphereEditor)DataEditor).PublicNotes;

                case Enums.DataType.Chapter:
                    return ((ChapterEditor)DataEditor).PublicNotes;

                case Enums.DataType.Phase:
                    return ((PhaseEditor)DataEditor).PublicNotes;

                case Enums.DataType.Quest:
                    return ((QuestEditor)DataEditor).PublicNotes;

                case Enums.DataType.Objective:
                    return ((ObjectiveEditor)DataEditor).PublicNotes;

                case Enums.DataType.Task:
                    return ((TaskEditor)DataEditor).PublicNotes;

                case Enums.DataType.Interaction:
                    return ((InteractionEditor)DataEditor).PublicNotes;

                case Enums.DataType.Outcome:
                    return ((OutcomeEditor)DataEditor).PublicNotes;

                case Enums.DataType.Scene:
                    return ((SceneEditor)DataEditor).PublicNotes;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Atmosphere:

                    var atmosphereEditor = (AtmosphereEditor)DataEditor;
                    atmosphereEditor.PublicNotes = value;

                    break;

                case Enums.DataType.Chapter:

                    var chapterEditor = (ChapterEditor)DataEditor;
                    chapterEditor.PublicNotes = value;

                    break;

                case Enums.DataType.Phase:

                    var phaseEditor = (PhaseEditor)DataEditor;
                    phaseEditor.PublicNotes = value;

                    break;

                case Enums.DataType.Quest:

                    var questEditor = (QuestEditor)DataEditor;
                    questEditor.PublicNotes = value;

                    break;

                case Enums.DataType.Objective:

                    var objectiveEditor = (ObjectiveEditor)DataEditor;
                    objectiveEditor.PublicNotes = value;

                    break;

                case Enums.DataType.Task:

                    var taskEditor = (TaskEditor)DataEditor;
                    taskEditor.PublicNotes = value;

                    break;

                case Enums.DataType.Interaction:

                    var interactionEditor = (InteractionEditor)DataEditor;
                    interactionEditor.PublicNotes = value;

                    break;

                case Enums.DataType.Outcome:

                    var outcomeEditor = (OutcomeEditor)DataEditor;
                    outcomeEditor.PublicNotes = value;

                    break;

                case Enums.DataType.Scene:

                    var sceneEditor = (SceneEditor)DataEditor;
                    sceneEditor.PublicNotes = value;

                    break;

                default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
            }
        }
    }

    public string PrivateNotes
    {
        get
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Atmosphere:
                    return ((AtmosphereEditor)DataEditor).PrivateNotes;

                case Enums.DataType.Chapter:
                    return ((ChapterEditor)DataEditor).PrivateNotes;

                case Enums.DataType.Phase:
                    return ((PhaseEditor)DataEditor).PrivateNotes;

                case Enums.DataType.Quest:
                    return ((QuestEditor)DataEditor).PrivateNotes;

                case Enums.DataType.Objective:
                    return ((ObjectiveEditor)DataEditor).PrivateNotes;

                case Enums.DataType.Task:
                    return ((TaskEditor)DataEditor).PrivateNotes;

                case Enums.DataType.Interaction:
                    return ((InteractionEditor)DataEditor).PrivateNotes;

                case Enums.DataType.Outcome:
                    return ((OutcomeEditor)DataEditor).PrivateNotes;

                case Enums.DataType.Scene:
                    return ((SceneEditor)DataEditor).PrivateNotes;

                default: { Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); return ""; }
            }
        }
        set
        {
            switch (DataEditor.Data.dataController.DataType)
            {
                case Enums.DataType.Atmosphere:

                    var atmosphereEditor = (AtmosphereEditor)DataEditor;
                    atmosphereEditor.PrivateNotes = value;

                    break;

                case Enums.DataType.Chapter:

                    var chapterEditor = (ChapterEditor)DataEditor;
                    chapterEditor.PrivateNotes = value;

                    break;

                case Enums.DataType.Phase:

                    var phaseEditor = (PhaseEditor)DataEditor;
                    phaseEditor.PrivateNotes = value;

                    break;

                case Enums.DataType.Quest:

                    var questEditor = (QuestEditor)DataEditor;
                    questEditor.PrivateNotes = value;

                    break;

                case Enums.DataType.Objective:

                    var objectiveEditor = (ObjectiveEditor)DataEditor;
                    objectiveEditor.PrivateNotes = value;

                    break;

                case Enums.DataType.Task:

                    var taskEditor = (TaskEditor)DataEditor;
                    taskEditor.PrivateNotes = value;

                    break;

                case Enums.DataType.Interaction:

                    var interactionEditor = (InteractionEditor)DataEditor;
                    interactionEditor.PrivateNotes = value;

                    break;

                case Enums.DataType.Outcome:

                    var outcomeEditor = (OutcomeEditor)DataEditor;
                    outcomeEditor.PrivateNotes = value;

                    break;

                case Enums.DataType.Scene:

                    var sceneEditor = (SceneEditor)DataEditor;
                    sceneEditor.PrivateNotes = value;

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
