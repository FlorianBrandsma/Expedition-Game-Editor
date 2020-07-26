using UnityEngine;
using System.Linq;

public class InteractionInteractionDelayMethodSegment : MonoBehaviour, ISegment
{
    public ExDropdown delayMethodDropdown;
    public ExInputNumber delayDurationInputNumber;
    public ExToggle hideDelayIndicatorToggle;

    private int delayMethod;
    private int delayDuration;
    private bool hideDelayIndicator;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public int DelayMethod
    {
        get { return delayMethod; }
        set
        {
            delayMethod = value;

            var interactionDataList = DataEditor.DataList.Cast<InteractionElementData>().ToList();
            interactionDataList.ForEach(interactionData =>
            {
                interactionData.DelayMethod = delayMethod;
            });
        }
    }

    public int DelayDuration
    {
        get { return delayDuration; }
        set
        {
            delayDuration = value;

            var interactionDataList = DataEditor.DataList.Cast<InteractionElementData>().ToList();
            interactionDataList.ForEach(interactionData =>
            {
                interactionData.DelayDuration = delayDuration;
            });
        }
    }

    public bool HideDelayIndicator
    {
        get { return hideDelayIndicator; }
        set
        {
            hideDelayIndicator = value;

            var interactionDataList = DataEditor.DataList.Cast<InteractionElementData>().ToList();
            interactionDataList.ForEach(interactionData =>
            {
                interactionData.HideDelayIndicator = hideDelayIndicator;
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

        var interactionData = (InteractionElementData)DataEditor.Data.elementData;

        delayMethod = interactionData.DelayMethod;
        delayDuration = interactionData.DelayDuration;
        hideDelayIndicator = interactionData.HideDelayIndicator;
    }

    public void InitializeSegment() { }

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

    private void SetSearchParameters() { }

    public void OpenSegment()
    {
        delayMethodDropdown.Dropdown.value = delayMethod;
        delayDurationInputNumber.Value = delayDuration;
        hideDelayIndicatorToggle.Toggle.isOn = hideDelayIndicator;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
