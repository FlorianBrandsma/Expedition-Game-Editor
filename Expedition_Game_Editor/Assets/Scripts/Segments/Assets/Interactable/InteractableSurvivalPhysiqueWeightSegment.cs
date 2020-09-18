using UnityEngine;
using System.Linq;

public class InteractableSurvivalPhysiqueWeightSegment : MonoBehaviour, ISegment
{
    public ExInputNumber weightInputNumber;

    public SegmentController SegmentController      { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                       { get; set; }

    public InteractableEditor InteractableEditor    { get { return (InteractableEditor)DataEditor; } }

    #region Data properties
    private float Weight
    {
        get { return InteractableEditor.Weight; }
        set { InteractableEditor.Weight = value; }
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
        weightInputNumber.Value = Weight;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateWeight()
    {
        Weight = weightInputNumber.Value;

        DataEditor.UpdateEditor();
    }
    
    public void CloseSegment() { }
}
