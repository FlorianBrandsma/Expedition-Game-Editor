using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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

                    var chapterDataList = DataEditor.DataList.Cast<ChapterDataElement>().ToList();
                    chapterDataList.ForEach(chapterData =>
                    {
                        chapterData.Notes = value;
                    });

                    break;

                case Enums.DataType.Phase:

                    var phaseDataList = DataEditor.DataList.Cast<PhaseDataElement>().ToList();
                    phaseDataList.ForEach(phaseData =>
                    {
                        phaseData.Notes = value;
                    });

                    break;

                case Enums.DataType.Quest:

                    var questDataList = DataEditor.DataList.Cast<QuestDataElement>().ToList();
                    questDataList.ForEach(questData =>
                    {
                        questData.Notes = value;
                    });

                    break;

                case Enums.DataType.Objective:

                    var objectiveDataList = DataEditor.DataList.Cast<ObjectiveDataElement>().ToList();
                    objectiveDataList.ForEach(objectiveData =>
                    {
                        objectiveData.Notes = value;
                    });

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
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
    #endregion
}
