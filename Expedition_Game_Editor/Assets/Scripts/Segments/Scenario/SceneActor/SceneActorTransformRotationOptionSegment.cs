using UnityEngine;
using System.Linq;

public class SceneActorTransformRotationOptionSegment : MonoBehaviour, ISegment
{
    public ExToggle changeRotationToggle;
    public ExToggle faceTargetToggle;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private SceneActorEditor SceneActorEditor   { get { return (SceneActorEditor)DataEditor; } }

    #region Data properties
    private int TargetSceneActorId
    {
        get { return SceneActorEditor.TargetSceneActorId; }
    }

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
        faceTargetToggle.Toggle.isOn = FaceTarget;

        UpdateSegment();

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateChangeRotation()
    {
        ChangeRotation = changeRotationToggle.Toggle.isOn;

        UpdateSegments();

        DataEditor.UpdateEditor();
    }

    public void UpdateFaceTarget()
    {
        FaceTarget = faceTargetToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    private void UpdateSegments()
    {
        DataEditor.EditorSegments.Where(x => x.gameObject.activeInHierarchy).ToList().ForEach(x => x.UpdateSegment());
    }

    public void UpdateSegment()
    {
        faceTargetToggle.EnableElement(!ChangeRotation && TargetSceneActorId > 0);
    }

    public void CloseSegment() { }
}