using UnityEngine;

public class SceneActorTransformPositionOptionSegment : MonoBehaviour, ISegment
{
    public ExToggle changePositionToggle;
    public ExToggle freezePositionToggle;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private SceneActorEditor SceneActorEditor   { get { return (SceneActorEditor)DataEditor; } }

    #region Data properties
    private bool ChangePosition
    {
        get { return SceneActorEditor.ChangePosition; }
        set { SceneActorEditor.ChangePosition = value; }
    }

    private bool FreezePosition
    {
        get { return SceneActorEditor.FreezePosition; }
        set { SceneActorEditor.FreezePosition = value; }
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
        changePositionToggle.Toggle.isOn = ChangePosition;
        freezePositionToggle.Toggle.isOn = FreezePosition;

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateChangePosition()
    {
        ChangePosition = changePositionToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateFreezePosition()
    {
        FreezePosition = freezePositionToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void CloseSegment() { }
}
