using UnityEngine;

public class WorldObjectRotationDegreeSegment : MonoBehaviour, ISegment
{
    public ExInputNumber xInputField, yInputField, zInputField;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public WorldObjectEditor WorldObjectEditor  { get { return (WorldObjectEditor)DataEditor; } }

    #region Data properties
    private int RotationX
    {
        get { return WorldObjectEditor.RotationX; }
        set { WorldObjectEditor.RotationX = value; }
    }

    private int RotationY
    {
        get { return WorldObjectEditor.RotationY; }
        set { WorldObjectEditor.RotationY = value; }
    }

    private int RotationZ
    {
        get { return WorldObjectEditor.RotationZ; }
        set { WorldObjectEditor.RotationZ = value; }
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

    public void CloseSegment() { }
}
