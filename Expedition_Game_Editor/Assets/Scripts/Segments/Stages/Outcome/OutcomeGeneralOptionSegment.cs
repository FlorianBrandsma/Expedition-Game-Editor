using UnityEngine;

public class OutcomeGeneralOptionSegment : MonoBehaviour, ISegment
{
    public ExToggle completeTaskToggle;
    public ExToggle resetObjectiveToggle;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private OutcomeEditor OutcomeEditor         { get { return (OutcomeEditor)DataEditor; } }

    #region Data properties
    private bool CompleteTask
    {
        get { return OutcomeEditor.CompleteTask; }
        set { OutcomeEditor.CompleteTask = value; }
    }

    private bool ResetObjective
    {
        get { return OutcomeEditor.ResetObjective; }
        set { OutcomeEditor.ResetObjective = value; }
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
        completeTaskToggle.Toggle.isOn = CompleteTask;
        resetObjectiveToggle.Toggle.isOn = ResetObjective;

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateCompleteTask()
    {
        CompleteTask = completeTaskToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateResetObjective()
    {
        ResetObjective = resetObjectiveToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void CloseSegment() { }
}
