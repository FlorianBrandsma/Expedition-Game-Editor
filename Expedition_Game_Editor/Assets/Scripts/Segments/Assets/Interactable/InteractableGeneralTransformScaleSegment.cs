using UnityEngine;
using UnityEngine.UI;

public class InteractableGeneralTransformScaleSegment : MonoBehaviour, ISegment
{
    public Text heightText, widthText, depthText;
    public ExInputNumber inputField;

    public SegmentController SegmentController      { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                       { get; set; }

    public InteractableEditor InteractableEditor    { get { return (InteractableEditor)DataEditor; } }

    #region Data properties
    private float Scale
    {
        get { return InteractableEditor.Scale; }
        set { InteractableEditor.Scale = value; }
    }

    private float Height
    {
        get { return InteractableEditor.Height; }
        set { InteractableEditor.Height = value; }
    }

    private float Width
    {
        get { return InteractableEditor.Width; }
        set { InteractableEditor.Width = value; }
    }

    private float Depth
    {
        get { return InteractableEditor.Depth; }
        set { InteractableEditor.Depth = value; }
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
