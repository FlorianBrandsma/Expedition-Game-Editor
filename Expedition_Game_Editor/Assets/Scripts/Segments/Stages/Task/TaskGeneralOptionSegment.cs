using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class TaskGeneralOptionSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public ExToggle completeObjectiveToggle;
    public ExToggle repeatableToggle;
    #endregion

    #region Data Variables
    private bool completeObjective;
    private bool repeatable;
    #endregion

    #region Properties
    public bool CompleteObjective
    {
        get { return completeObjective; }
        set
        {
            completeObjective = value;

            var taskDataList = DataEditor.DataList.Cast<TaskElementData>().ToList();
            taskDataList.ForEach(taskData =>
            {
                taskData.CompleteObjective = completeObjective;
            });
        }
    }

    public bool Repeatable
    {
        get { return repeatable; }
        set
        {
            repeatable = value;

            var taskDataList = DataEditor.DataList.Cast<TaskElementData>().ToList();
            taskDataList.ForEach(taskData =>
            {
                taskData.Repeatable = repeatable;
            });
        }
    }
    #endregion

    #region Methods
    public void UpdateCompleteObjective()
    {
        CompleteObjective = completeObjectiveToggle.Toggle.isOn;
        
        DataEditor.UpdateEditor();
    }

    public void UpdateRepeatable()
    {
        Repeatable = repeatableToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }
    #endregion

    #region Segment
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        var taskData = (TaskElementData)DataEditor.ElementData;

        completeObjective = taskData.CompleteObjective;
        repeatable = taskData.Repeatable;
    }

    public void InitializeSegment() { }

    public void OpenSegment()
    {
        completeObjectiveToggle.Toggle.isOn = CompleteObjective;
        repeatableToggle.Toggle.isOn = Repeatable;
        
        gameObject.SetActive(true);
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
    #endregion
}
