using UnityEngine;

public class SceneActorTransformRotationDegreeSegment : MonoBehaviour, ISegment
{
    public ExInputNumber xInputField, yInputField, zInputField;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public SceneActorEditor SceneActorEditor    { get { return (SceneActorEditor)DataEditor; } }

    #region Data properties
    private bool ChangeRotation
    {
        get { return SceneActorEditor.ChangeRotation; }
    }

    private int RotationX
    {
        get { return SceneActorEditor.RotationX; }
        set { SceneActorEditor.RotationX = value; }
    }

    private int RotationY
    {
        get { return SceneActorEditor.RotationY; }
        set { SceneActorEditor.RotationY = value; }
    }

    private int RotationZ
    {
        get { return SceneActorEditor.RotationZ; }
        set { SceneActorEditor.RotationZ = value; }
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
        xInputField.Value = RotationX;
        yInputField.Value = RotationY;
        zInputField.Value = RotationZ;

        UpdateSegment();

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateRotationX()
    {
        RotationX = (int)xInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdateRotationY()
    {
        RotationY = (int)yInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdateRotationZ()
    {
        RotationZ = (int)zInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment()
    {
        EnableInputFields(ChangeRotation);
    }

    private void EnableInputFields(bool enable)
    {
        xInputField.EnableElement(enable);
        yInputField.EnableElement(enable);
        zInputField.EnableElement(enable);
    }

    public void CloseSegment() { }
}
