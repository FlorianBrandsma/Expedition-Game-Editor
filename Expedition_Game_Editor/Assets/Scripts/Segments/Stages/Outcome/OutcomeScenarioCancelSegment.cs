using UnityEngine;
using UnityEngine.UI;
using System;

public class OutcomeScenarioCancelSegment : MonoBehaviour, ISegment
{
    public ExDropdown cancelScenarioTypeDropdown;
    public ExToggle cancelScenarioOnInteractionToggle;
    public ExToggle cancelScenarioOnInputToggle;
    public ExToggle cancelScenarioOnRangeToggle;
    public ExToggle cancelScenarioOnHitToggle;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private OutcomeEditor OutcomeEditor { get { return (OutcomeEditor)DataEditor; } }

    #region Data properties
    public int CancelScenarioType
    {
        get { return OutcomeEditor.CancelScenarioType; }
        set { OutcomeEditor.CancelScenarioType = value; }
    }

    public bool CancelScenarioOnInteraction
    {
        get { return OutcomeEditor.CancelScenarioOnInteraction; }
        set { OutcomeEditor.CancelScenarioOnInteraction = value; }
    }

    public bool CancelScenarioOnInput
    {
        get { return OutcomeEditor.CancelScenarioOnInput; }
        set { OutcomeEditor.CancelScenarioOnInput = value; }
    }

    public bool CancelScenarioOnRange
    {
        get { return OutcomeEditor.CancelScenarioOnRange; }
        set { OutcomeEditor.CancelScenarioOnRange = value; }
    }

    public bool CancelScenarioOnHit
    {
        get { return OutcomeEditor.CancelScenarioOnHit; }
        set { OutcomeEditor.CancelScenarioOnHit = value; }
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
        cancelScenarioTypeDropdown.Dropdown.captionText.text = Enum.GetName(typeof(Enums.CancelScenarioType), CancelScenarioType);

        foreach (var type in Enum.GetValues(typeof(Enums.CancelScenarioType)))
        {
            cancelScenarioTypeDropdown.Dropdown.options.Add(new Dropdown.OptionData(type.ToString()));
        }
    }
    
    public void OpenSegment()
    {
        cancelScenarioTypeDropdown.Dropdown.value = CancelScenarioType;
        cancelScenarioOnInteractionToggle.Toggle.isOn = CancelScenarioOnInteraction;
        cancelScenarioOnInputToggle.Toggle.isOn = CancelScenarioOnInput;
        cancelScenarioOnRangeToggle.Toggle.isOn = CancelScenarioOnRange;
        cancelScenarioOnHitToggle.Toggle.isOn = CancelScenarioOnHit;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateCancelScenarioType()
    {
        CancelScenarioType = cancelScenarioTypeDropdown.Dropdown.value;

        UpdateSegment();

        DataEditor.UpdateEditor();
    }

    public void UpdateCancelScenarioOnInteraction()
    {
        CancelScenarioOnInteraction = cancelScenarioOnInteractionToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateCancelScenarioOnInput()
    {
        CancelScenarioOnInput = cancelScenarioOnInputToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateCancelScenarioOnRange()
    {
        CancelScenarioOnRange = cancelScenarioOnRangeToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateCancelScenarioOnHit()
    {
        CancelScenarioOnHit = cancelScenarioOnHitToggle.Toggle.isOn;

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment()
    {
        EnableToggles((Enums.CancelScenarioType)CancelScenarioType == Enums.CancelScenarioType.Cancel);
    }

    private void EnableToggles(bool enable)
    {
        cancelScenarioOnInteractionToggle.EnableElement(enable);
        cancelScenarioOnInputToggle.EnableElement(enable);
        cancelScenarioOnRangeToggle.EnableElement(enable);
        cancelScenarioOnHitToggle.EnableElement(enable);
    }

    public void CloseSegment()
    {
        cancelScenarioTypeDropdown.Dropdown.options.Clear();
    }
}
