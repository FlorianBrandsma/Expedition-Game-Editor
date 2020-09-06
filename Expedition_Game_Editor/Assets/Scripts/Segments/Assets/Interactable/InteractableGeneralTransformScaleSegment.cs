using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InteractableGeneralTransformScaleSegment : MonoBehaviour, ISegment
{
    public Text heightText, widthText, depthText;
    public ExInputNumber inputField;

    private float scale;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public float Scale
    {
        get { return scale; }
        set
        {
            scale = value;

            var interactableDataList = DataEditor.DataList.Cast<InteractableElementData>().ToList();
            interactableDataList.ForEach(interactableData =>
            {
                interactableData.Scale = value;
            });
        }
    }
    
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        var interactableData = (InteractableElementData)DataEditor.ElementData;

        scale = interactableData.Scale;
    }

    public void InitializeSegment()
    {
        SetSizeValues();
    }

    public void SetSizeValues()
    {
        var interactableData = (InteractableElementData)DataEditor.ElementData;

        heightText.text = (interactableData.Height * scale).ToString();
        widthText.text = (interactableData.Width * scale).ToString();
        depthText.text = (interactableData.Depth * scale).ToString();
    }

    public void UpdateScale()
    {
        Scale = inputField.Value;

        SetSizeValues();

        DataEditor.UpdateEditor();
    }

    public void OpenSegment()
    {
        SetSizeValues();

        inputField.Value = Scale;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
