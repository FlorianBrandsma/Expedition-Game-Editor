using UnityEngine;
using UnityEngine.UI;

public class ScenePropSizeScaleSegment : MonoBehaviour, ISegment
{
    public Text heightNumberText, widthNumberText, depthNumberText;
    public ExInputNumber scaleInputNumber;
    
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public ScenePropEditor ScenePropEditor { get { return (ScenePropEditor)DataEditor; } }

    #region Data properties
    private float Scale
    {
        get { return ScenePropEditor.Scale; }
        set { ScenePropEditor.Scale = value; }
    }

    private float Height
    {
        get { return ScenePropEditor.Height; }
    }

    private float Width
    {
        get { return ScenePropEditor.Width; }
    }

    private float Depth
    {
        get { return ScenePropEditor.Depth; }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }
    
    public void InitializeData() { }

    public void InitializeSegment()
    {
        SetSizeValues();
    }

    public void SetSizeValues()
    {
        heightNumberText.text = (Height * Scale).ToString();
        widthNumberText.text  = (Width  * Scale).ToString();
        depthNumberText.text  = (Depth  * Scale).ToString();
    }
    
    public void OpenSegment()
    {
        SetSizeValues();

        scaleInputNumber.Value = Scale;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateScale()
    {
        Scale = scaleInputNumber.Value;

        SetSizeValues();

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
