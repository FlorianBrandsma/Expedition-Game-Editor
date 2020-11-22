using UnityEngine;

public class TaskGeneralOptionSegment : MonoBehaviour, ISegment
{
    public ExToggle completeObjectiveToggle;
    public ExToggle repeatableToggle;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private TaskEditor TaskEditor               { get { return (TaskEditor)DataEditor; } }

    #region Data properties
    private bool CompleteObjective
    {
        get { return TaskEditor.CompleteObjective; }
        set { TaskEditor.CompleteObjective = value; }
    }

    private bool Repeatable
    {
        get { return TaskEditor.Repeatable; }
        set { TaskEditor.Repeatable = value; }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData() { }

    public void InitializeSegment() { }

    public void OpenSegment()
    {
        completeObjectiveToggle.Toggle.isOn = CompleteObjective;
        repeatableToggle.Toggle.isOn = Repeatable;
        
        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData elementData) { }

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

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
