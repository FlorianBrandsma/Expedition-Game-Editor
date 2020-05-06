using UnityEngine;
using UnityEngine.UI;

public class SaveDataHeaderSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public Text idText;
    public Text headerText;
    #endregion

    #region Data Variables
    private int id;
    private int index;
    private new string name;
    #endregion

    #region Segment
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
            //case Enums.DataType.PhaseSave:          InitializePhaseSaveData();          break;
            //case Enums.DataType.QuestSave:          InitializeQuestSaveData();          break;
            //case Enums.DataType.ObjectiveSave:      InitializeObjectiveSaveData();      break;
            //case Enums.DataType.TaskSave:           InitializeTaskSaveData();           break;
            //case Enums.DataType.InteractionSave:    InitializeInteractionSaveData();    break;
            

            default: Debug.Log("CASE MISSING: " + DataEditor.Data.dataController.DataType); break;
        }
    }

    private void InitializeChapterSaveData()
    {
        var chapterData = (ChapterSaveDataElement)DataEditor.Data.dataElement;

        id = chapterData.Id;
        index = chapterData.Index;
        name = chapterData.name;
    }

    //private void InitializePhaseSaveData()
    //{
    //    var phaseData = (PhaseDataElement)DataEditor.Data.dataElement;

    //    id = phaseData.Id;
    //    index = phaseData.Index;
    //    name = phaseData.Name;
    //}

    //private void InitializeQuestSaveData()
    //{
    //    var questData = (QuestDataElement)DataEditor.Data.dataElement;

    //    id = questData.Id;
    //    index = questData.Index;
    //    name = questData.Name;
    //}

    //private void InitializeObjectiveSaveData()
    //{
    //    var objectiveData = (ObjectiveDataElement)DataEditor.Data.dataElement;

    //    id = objectiveData.Id;
    //    index = objectiveData.Index;
    //    name = objectiveData.Name;
    //}

    //private void InitializeTaskSaveData()
    //{
    //    var objectiveData = (TaskDataElement)DataEditor.Data.dataElement;

    //    id = objectiveData.Id;
    //    index = objectiveData.Index;
    //    name = objectiveData.Name;
    //}

    //private void InitializeInteractionSaveData()
    //{
    //    var objectiveData = (TaskDataElement)DataEditor.Data.dataElement;

    //    id = objectiveData.Id;
    //    index = objectiveData.Index;
    //    name = objectiveData.Name;
    //}

    public void OpenSegment()
    {
        idText.text = id.ToString();
        headerText.text = name;

        gameObject.SetActive(true);
    }

    public void CloseSegment()
    {
        gameObject.SetActive(false);
    }

    public void SetSearchResult(SelectionElement selectionElement) { }
    #endregion
}
