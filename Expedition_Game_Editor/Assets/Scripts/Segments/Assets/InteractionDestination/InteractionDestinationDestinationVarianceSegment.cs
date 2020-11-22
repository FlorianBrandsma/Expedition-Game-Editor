using UnityEngine;
using System.Linq;

public class InteractionDestinationDestinationVarianceSegment : MonoBehaviour, ISegment
{
    public ExInputNumber positionVarianceInputField;

    private float positionVariance;
    
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public InteractionDestinationEditor InteractionDestinationEditor { get { return (InteractionDestinationEditor)DataEditor; } }

    #region Data properties
    private float PositionVariance
    {
        get { return InteractionDestinationEditor.PositionVariance; }
        set { InteractionDestinationEditor.PositionVariance = value; }
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
        positionVarianceInputField.Value = PositionVariance;

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdatePositionVariance()
    {
        PositionVariance = positionVarianceInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
