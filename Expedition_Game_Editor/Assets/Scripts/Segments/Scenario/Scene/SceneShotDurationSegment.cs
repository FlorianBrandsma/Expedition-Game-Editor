using UnityEngine;

public class SceneShotDurationSegment : MonoBehaviour, ISegment
{
    public ExInputNumber shotDurationInputNumber;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public SceneEditor SceneEditor              { get { return (SceneEditor)DataEditor; } }

    #region Data properties
    private float ShotDuration
    {
        get { return SceneEditor.ShotDuration; }
        set { SceneEditor.ShotDuration = value; }
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
        shotDurationInputNumber.Value = ShotDuration;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateShotDuration()
    {
        ShotDuration = shotDurationInputNumber.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
