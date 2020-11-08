using UnityEngine;

public class InteractionInteractionTriggerMethodSegment : MonoBehaviour, ISegment
{
    public ExToggle triggerAutomaticallyToggle;
    public ExToggle beNearDestinationToggle;
    public ExToggle faceInteractableToggle;
    public ExToggle faceControllableToggle;
    public ExToggle hideInteractionIndicatorToggle;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private InteractionEditor InteractionEditor { get { return (InteractionEditor)DataEditor; } }

    #region Data properties
    private bool TriggerAutomatically
    {
        get { return InteractionEditor.TriggerAutomatically; }
        set { InteractionEditor.TriggerAutomatically = value; }
    }

    private bool BeNearDestination
    {
        get { return InteractionEditor.BeNearDestination; }
        set { InteractionEditor.BeNearDestination = value; }
    }

    private bool FaceInteractable
    {
        get { return InteractionEditor.FaceInteractable; }
        set { InteractionEditor.FaceInteractable = value; }
    }

    private bool FaceControllable
    {
        get { return InteractionEditor.FaceControllable; }
        set { InteractionEditor.FaceControllable = value; }
    }

    private bool HideInteractionIndicator
    {
        get { return InteractionEditor.HideInteractionIndicator; }
        set { InteractionEditor.HideInteractionIndicator = value; }
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
        triggerAutomaticallyToggle.Toggle.isOn = TriggerAutomatically;
        beNearDestinationToggle.Toggle.isOn = BeNearDestination;
        faceInteractableToggle.Toggle.isOn = FaceInteractable;
        faceControllableToggle.Toggle.isOn = FaceControllable;
        hideInteractionIndicatorToggle.Toggle.isOn = HideInteractionIndicator;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateTriggerAutomatically()
    {
        TriggerAutomatically = triggerAutomaticallyToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateBeNearDestination()
    {
        BeNearDestination = beNearDestinationToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateFaceInteractable()
    {
        FaceInteractable = faceInteractableToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateFaceControllable()
    {
        FaceControllable = faceControllableToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateHideInteractionIndicator()
    {
        HideInteractionIndicator = hideInteractionIndicatorToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void CloseSegment() { }
}
