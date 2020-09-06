using UnityEngine;
using System.Linq;

public class InteractionDestinationDestinationVarianceSegment : MonoBehaviour, ISegment
{
    public ExInputNumber positionVarianceInputField;

    private float positionVariance;
    
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public float PositionVariance
    {
        get { return positionVariance; }
        set
        {
            positionVariance = value;

            var interactionDestinationDataList = DataEditor.DataList.Cast<InteractionDestinationElementData>().ToList();
            interactionDestinationDataList.ForEach(interactionDestinationData =>
            {
                interactionDestinationData.PositionVariance = value;
            });
        }
    }

    public void UpdatePositionVariance()
    {
        PositionVariance = positionVarianceInputField.Value;
        
        DataEditor.UpdateEditor();
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

        var interactionDestinationData = (InteractionDestinationElementData)DataEditor.ElementData;

        positionVariance = interactionDestinationData.PositionVariance;
    }

    public void InitializeSegment() { }

    public void OpenSegment()
    {
        positionVarianceInputField.Value = PositionVariance;

        gameObject.SetActive(true);
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
