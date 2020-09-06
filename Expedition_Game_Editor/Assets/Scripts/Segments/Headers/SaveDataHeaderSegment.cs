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

        id = chapterSaveData.Id;
        index = chapterSaveData.Index;
        name = chapterSaveData.Name;
    }

    private void InitializePhaseSaveData()
    {
        var phaseSaveData = (PhaseSaveElementData)DataEditor.ElementData;

        id = phaseSaveData.Id;
        index = phaseSaveData.Index;
        name = phaseSaveData.Name;
    }

    private void InitializeQuestSaveData()
    {
        var questSaveData = (QuestSaveElementData)DataEditor.ElementData;

        id = questSaveData.Id;
        index = questSaveData.Index;
        name = questSaveData.Name;
    }

    private void InitializeObjectiveSaveData()
    {
        var objectiveSaveData = (ObjectiveSaveElementData)DataEditor.ElementData;

        id = objectiveSaveData.Id;
        index = objectiveSaveData.Index;
        name = objectiveSaveData.Name;
    }

    private void InitializeTaskSaveData()
    {
        var taskSaveData = (TaskSaveElementData)DataEditor.ElementData;

        id = taskSaveData.Id;
        index = taskSaveData.Index;
        name = taskSaveData.Name;
    }

    private void InitializeInteractionSaveData()
    {
        var interactionSaveData = (InteractionSaveElementData)DataEditor.ElementData;

        id = interactionSaveData.Id;
        index = interactionSaveData.Index;
        name = interactionSaveData.Default ? "Default" : TimeManager.FormatTime(interactionSaveData.StartTime) + " - " + TimeManager.FormatTime(interactionSaveData.EndTime);
    }

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

    public void SetSearchResult(DataElement dataElement) { }
    #endregion
}
