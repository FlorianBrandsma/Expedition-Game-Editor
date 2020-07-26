using UnityEngine;
using System.Linq;

public class InteractionInteractionTriggerRangeSegment : MonoBehaviour, ISegment
{
    public ExInputNumber interactionRangeInputNumber;

    private float interactionRange;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public float InteractionRange
    {
        get { return interactionRange; }
        set
        {
            interactionRange = value;

            var interactionDataList = DataEditor.DataList.Cast<InteractionElementData>().ToList();
            interactionDataList.ForEach(interactionData =>
            {
                interactionData.InteractionRange = interactionRange;
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

        interactionRange = interactionData.InteractionRange;
    }

    public void InitializeSegment()
    {
        InitializeDropdown();
    }

    private void InitializeDropdown()
    {
        interactionRangeInputNumber.Value = interactionRange;
    }

    public void UpdateInteractionRange()
    {
        InteractionRange = interactionRangeInputNumber.Value;

        DataEditor.UpdateEditor();
    }

    private void SetSearchParameters() { }

    public void OpenSegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
