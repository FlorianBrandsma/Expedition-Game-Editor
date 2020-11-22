using UnityEngine;
using System.Linq;

public class InteractableSurvivalPhysiqueSpeedSegment : MonoBehaviour, ISegment
{
    public ExInputNumber speedInputNumber;

    public SegmentController SegmentController      { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                       { get; set; }

    public InteractableEditor InteractableEditor    { get { return (InteractableEditor)DataEditor; } }

    #region Data properties
    private float Speed
    {
        get { return InteractableEditor.Speed; }
        set { InteractableEditor.Speed = value; }
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
        speedInputNumber.Value = Speed;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateSpeed()
    {
        Speed = speedInputNumber.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}