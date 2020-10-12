using UnityEngine;
using UnityEngine.UI;
using System;

public class InteractionInteractableBehaviourArrivalSegment : MonoBehaviour, ISegment
{
    public ExDropdown arrivalTypeDropdown;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    private InteractionEditor InteractionEditor { get { return (InteractionEditor)DataEditor; } }

    #region Data properties
    public int ArrivalType
    {
        get { return InteractionEditor.ArrivalType; }
        set { InteractionEditor.ArrivalType = value; }
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
        arrivalTypeDropdown.Dropdown.captionText.text = Enum.GetName(typeof(Enums.ArrivalType), ArrivalType);

        foreach (var display in Enum.GetValues(typeof(Enums.ArrivalType)))
        {
            arrivalTypeDropdown.Dropdown.options.Add(new Dropdown.OptionData(display.ToString()));
        }
    }

    public void OpenSegment()
    {
        arrivalTypeDropdown.Dropdown.value = ArrivalType;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateArrivalType()
    {
        ArrivalType = arrivalTypeDropdown.Dropdown.value;

        DataEditor.UpdateEditor();
    }

    public void CloseSegment()
    {
        arrivalTypeDropdown.Dropdown.options.Clear();
    }
}

