using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class WorldObjectSizeScaleSegment : MonoBehaviour, ISegment
{
    public ExInputNumber inputField;
    public Text heightText, widthText, depthText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public WorldObjectEditor WorldObjectEditor  { get { return (WorldObjectEditor)DataEditor; } }

    #region Data properties
    private float Scale
    {
        get { return WorldObjectEditor.Scale; }
        set { WorldObjectEditor.Scale = value; }
    }

    private float Height
    {
        get { return WorldObjectEditor.Height; }
    }

    private float Width
    {
        get { return WorldObjectEditor.Width; }
    }

    private float Depth
    {
        get { return WorldObjectEditor.Depth; }
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
        heightText.text = (Height * Scale).ToString();
        widthText.text  = (Width * Scale).ToString();
        depthText.text  = (Depth * Scale).ToString();
    }
    
    public void OpenSegment()
    {
        SetSizeValues();

        inputField.Value = Scale;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateScale()
    {
        Scale = inputField.Value;

        SetSizeValues();

        DataEditor.UpdateEditor();
    }

    public void CloseSegment() { }
}
