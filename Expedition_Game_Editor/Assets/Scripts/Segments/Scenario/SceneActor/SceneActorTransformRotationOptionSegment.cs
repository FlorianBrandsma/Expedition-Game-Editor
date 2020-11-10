using UnityEngine;

public class SceneActorTransformRotationOptionSegment : MonoBehaviour, ISegment
{
    public ExToggle changeRotationToggle;
    public ExToggle FaceTargetToggle;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private SceneActorEditor SceneActorEditor   { get { return (SceneActorEditor)DataEditor; } }

    #region Data properties
    private bool ChangeRotation
    {
        get { return SceneActorEditor.ChangeRotation; }
        set { SceneActorEditor.ChangeRotation = value; }
    }

    private bool FaceTarget
    {
        get { return SceneActorEditor.FaceTarget; }
        set { SceneActorEditor.FaceTarget = value; }
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
        changeRotationToggle.Toggle.isOn = ChangeRotation;
        FaceTargetToggle.Toggle.isOn = FaceTarget;

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateChangeRotation()
    {
        ChangeRotation = changeRotationToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateFaceTarget()
    {
        FaceTarget = FaceTargetToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void CloseSegment() { }
}
