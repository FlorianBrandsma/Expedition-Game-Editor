using UnityEngine;

public class SceneGeneralOptionSegment : MonoBehaviour, ISegment
{
    public ExToggle freezeTimeToggle;
    public ExToggle freezeMovementToggle;
    public ExToggle autoContinueToggle;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private SceneEditor SceneEditor             { get { return (SceneEditor)DataEditor; } }

    #region Data properties
    private bool FreezeTime
    {
        get { return SceneEditor.FreezeTime; }
        set { SceneEditor.FreezeTime = value; }
    }

    private bool FreezeMovement
    {
        get { return SceneEditor.FreezeMovement; }
        set { SceneEditor.FreezeMovement = value; }
    }

    private bool AutoContinue
    {
        get { return SceneEditor.AutoContinue; }
        set { SceneEditor.AutoContinue = value; }
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
        freezeTimeToggle.Toggle.isOn = FreezeTime;
        freezeMovementToggle.Toggle.isOn = FreezeMovement;
        autoContinueToggle.Toggle.isOn = AutoContinue;

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateFreezeTime()
    {
        FreezeTime = freezeTimeToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateFreezeMovement()
    {
        FreezeMovement = freezeMovementToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateAutoContinue()
    {
        AutoContinue = autoContinueToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void CloseSegment() { }
}
