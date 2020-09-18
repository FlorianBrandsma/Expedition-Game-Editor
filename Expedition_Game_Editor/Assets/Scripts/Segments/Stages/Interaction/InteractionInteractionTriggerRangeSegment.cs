using UnityEngine;
using System.Linq;

public class InteractionInteractionTriggerRangeSegment : MonoBehaviour, ISegment
{
    public ExInputNumber interactionRangeInputNumber;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private InteractionEditor InteractionEditor { get { return (InteractionEditor)DataEditor; } }

    #region Data properties
    public float InteractionRange
    {
        get { return InteractionEditor.InteractionRange; }
        set { InteractionEditor.InteractionRange = value; }
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
        InitializeDropdown();
    }

    private void InitializeDropdown()
    {
        interactionRangeInputNumber.Value = InteractionRange;
    }
    
    public void OpenSegment() { }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateInteractionRange()
    {
        InteractionRange = interactionRangeInputNumber.Value;

        DataEditor.UpdateEditor();
    }

    public void CloseSegment() { }
}
