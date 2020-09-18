using UnityEngine;
using System.Linq;

public class InteractionInteractionDelayCancelSegment : MonoBehaviour, ISegment
{
    public ExToggle cancelDelayOnInputToggle;
    public ExToggle cancelDelayOnMovementToggle;
    public ExToggle cancelDelayOnHitToggle;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private InteractionEditor InteractionEditor { get { return (InteractionEditor)DataEditor; } }

    #region Data properties
    public bool CancelDelayOnInput
    {
        get { return InteractionEditor.CancelDelayOnInput; }
        set { InteractionEditor.CancelDelayOnInput = value; }
    }

    public bool CancelDelayOnMovement
    {
        get { return InteractionEditor.CancelDelayOnMovement; }
        set { InteractionEditor.CancelDelayOnMovement = value; }
    }

    public bool CancelDelayOnHit
    {
        get { return InteractionEditor.CancelDelayOnHit; }
        set { InteractionEditor.CancelDelayOnHit = value; }
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
        cancelDelayOnInputToggle.Toggle.isOn = CancelDelayOnInput;
        cancelDelayOnMovementToggle.Toggle.isOn = CancelDelayOnMovement;
        cancelDelayOnHitToggle.Toggle.isOn = CancelDelayOnHit;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateCancelDelayOnInput()
    {
        CancelDelayOnInput = cancelDelayOnInputToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateCancelDelayOnMovement()
    {
        CancelDelayOnMovement = cancelDelayOnMovementToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateCancelDelayOnHit()
    {
        CancelDelayOnHit = cancelDelayOnHitToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }
    
    public void CloseSegment() { }
}
