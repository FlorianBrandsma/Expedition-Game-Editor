using UnityEngine;

public class SceneGeneralDurationSegment : MonoBehaviour, ISegment
{
    public ExInputNumber sceneDurationInputNumber;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public SceneEditor SceneEditor              { get { return (SceneEditor)DataEditor; } }

    #region Data properties
    private float SceneDuration
    {
        get { return SceneEditor.SceneDuration; }
        set { SceneEditor.SceneDuration = value; }
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
        sceneDurationInputNumber.Value = SceneDuration;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSceneDuration()
    {
        SceneDuration = sceneDurationInputNumber.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
