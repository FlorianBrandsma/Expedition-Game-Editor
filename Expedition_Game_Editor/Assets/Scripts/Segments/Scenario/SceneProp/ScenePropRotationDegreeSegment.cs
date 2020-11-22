using UnityEngine;

public class ScenePropRotationDegreeSegment : MonoBehaviour, ISegment
{
    public ExInputNumber xInputField, yInputField, zInputField;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public ScenePropEditor ScenePropEditor      { get { return (ScenePropEditor)DataEditor; } }

    #region Data properties
    private int RotationX
    {
        get { return ScenePropEditor.RotationX; }
        set { ScenePropEditor.RotationX = value; }
    }

    private int RotationY
    {
        get { return ScenePropEditor.RotationY; }
        set { ScenePropEditor.RotationY = value; }
    }

    private int RotationZ
    {
        get { return ScenePropEditor.RotationZ; }
        set { ScenePropEditor.RotationZ = value; }
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

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
