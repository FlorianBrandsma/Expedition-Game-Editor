using UnityEngine;
using System.Linq;

public class InteractionInteractionDelayMethodSegment : MonoBehaviour, ISegment
{
    public ExDropdown delayMethodDropdown;
    public ExInputNumber delayDurationInputNumber;
    public ExToggle hideDelayIndicatorToggle;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private InteractionEditor InteractionEditor { get { return (InteractionEditor)DataEditor; } }

    #region Data properties
    public int DelayMethod
    {
        get { return InteractionEditor.DelayMethod; }
        set { InteractionEditor.DelayMethod = value; }
    }

    public int DelayDuration
    {
        get { return InteractionEditor.DelayDuration; }
        set { InteractionEditor.DelayDuration = value; }
    }

    public bool HideDelayIndicator
    {
        get { return InteractionEditor.HideDelayIndicator; }
        set { InteractionEditor.HideDelayIndicator = value; }
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
        delayMethodDropdown.Dropdown.value = DelayMethod;
        delayDurationInputNumber.Value = DelayDuration;
        hideDelayIndicatorToggle.Toggle.isOn = HideDelayIndicator;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateDelayMethod()
    {
        DelayMethod = delayMethodDropdown.Dropdown.value;

        DataEditor.UpdateEditor();
    }

    public void UpdateDelayDuration()
    {
        DelayDuration = (int)delayDurationInputNumber.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdateHideDelayIndicator()
    {
        HideDelayIndicator = hideDelayIndicatorToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void CloseSegment() { }
}
