using UnityEngine;

public class SceneShotRotationDegreeSegment : MonoBehaviour, ISegment
{
    public ExToggle changeRotationToggle;
    public ExInputNumber xInputField, yInputField, zInputField;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public SceneShotEditor SceneShotEditor      { get { return (SceneShotEditor)DataEditor; } }

    #region Data properties
    private bool ChangeRotation
    {
        get { return SceneShotEditor.ChangeRotation; }
        set { SceneShotEditor.ChangeRotation = value; }
    }

    private int RotationX
    {
        get { return SceneShotEditor.RotationX; }
        set { SceneShotEditor.RotationX = value; }
    }

    private int RotationY
    {
        get { return SceneShotEditor.RotationY; }
        set { SceneShotEditor.RotationY = value; }
    }

    private int RotationZ
    {
        get { return SceneShotEditor.RotationZ; }
        set { SceneShotEditor.RotationZ = value; }
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

        xInputField.Value = RotationX;
        yInputField.Value = RotationY;
        zInputField.Value = RotationZ;

        UpdateSegment();

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData elementData) { }

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

    public void UpdateChangeRotation()
    {
        ChangeRotation = changeRotationToggle.Toggle.isOn;

        UpdateSegment();

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
