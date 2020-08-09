using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class InteractableGeneralTransformScaleSegment : MonoBehaviour, ISegment
{
    public Text heightText, widthText, depthText;
    public ExInputNumber inputField;

    private float height;
    private float width;
    private float depth;
    private float scaleMultiplier;

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public float ScaleMultiplier
    {
        get { return scaleMultiplier; }
        set
        {
            scaleMultiplier = value;

            var interactableDataList = DataEditor.DataList.Cast<InteractableElementData>().ToList();
            interactableDataList.ForEach(interactableData =>
            {
                interactableData.ScaleMultiplier = value;
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

        var interactableData = (InteractableElementData)DataEditor.Data.elementData;

        height = interactableData.height;
        width = interactableData.width;
        depth = interactableData.depth;

        scaleMultiplier = interactableData.ScaleMultiplier;
    }

    public void InitializeSegment()
    {
        SetSizeValues();
    }

    public void SetSizeValues()
    {
        heightText.text = (height * scaleMultiplier).ToString();
        widthText.text = (width * scaleMultiplier).ToString();
        depthText.text = (depth * scaleMultiplier).ToString();
    }

    public void UpdateScaleMultiplier()
    {
        ScaleMultiplier = inputField.Value;

        SetSizeValues();

        DataEditor.UpdateEditor();
    }

    public void OpenSegment()
    {
        SetSizeValues();

        inputField.Value = ScaleMultiplier;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
