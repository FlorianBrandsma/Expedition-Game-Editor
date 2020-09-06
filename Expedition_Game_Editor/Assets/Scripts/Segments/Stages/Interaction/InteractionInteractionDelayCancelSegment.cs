using UnityEngine;
using System.Linq;

public class InteractionInteractionDelayCancelSegment : MonoBehaviour, ISegment
{
    public ExToggle cancelDelayOnInputToggle;
    public ExToggle cancelDelayOnMovementToggle;
    public ExToggle cancelDelayOnHitToggle;

    private bool cancelDelayOnInput;
    private bool cancelDelayOnMovement;
    private bool cancelDelayOnHit;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public bool CancelDelayOnInput
    {
        get { return cancelDelayOnInput; }
        set
        {
            cancelDelayOnInput = value;

            var interactionDataList = DataEditor.DataList.Cast<InteractionElementData>().ToList();
            interactionDataList.ForEach(interactionData =>
            {
                interactionData.CancelDelayOnInput = cancelDelayOnInput;
            });
        }
    }

    public bool CancelDelayOnMovement
    {
        get { return cancelDelayOnMovement; }
        set
        {
            cancelDelayOnMovement = value;

            var interactionDataList = DataEditor.DataList.Cast<InteractionElementData>().ToList();
            interactionDataList.ForEach(interactionData =>
            {
                interactionData.CancelDelayOnMovement = cancelDelayOnMovement;
            });
        }
    }

    public bool CancelDelayOnHit
    {
        get { return cancelDelayOnHit; }
        set
        {
            cancelDelayOnHit = value;

            var interactionDataList = DataEditor.DataList.Cast<InteractionElementData>().ToList();
            interactionDataList.ForEach(interactionData =>
            {
                interactionData.CancelDelayOnHit = cancelDelayOnHit;
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

        var interactionData = (InteractionElementData)DataEditor.ElementData;

        cancelDelayOnInput = interactionData.CancelDelayOnInput;
        cancelDelayOnMovement = interactionData.CancelDelayOnMovement;
        cancelDelayOnHit = interactionData.CancelDelayOnHit;
    }

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

    public void InitializeSegment() { }

    public void OpenSegment()
    {
        cancelDelayOnInputToggle.Toggle.isOn = cancelDelayOnInput;
        cancelDelayOnMovementToggle.Toggle.isOn = cancelDelayOnMovement;
        cancelDelayOnHitToggle.Toggle.isOn = cancelDelayOnHit;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
