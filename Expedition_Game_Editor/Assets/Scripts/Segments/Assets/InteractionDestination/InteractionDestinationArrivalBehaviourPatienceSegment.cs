using UnityEngine;
using System.Linq;

public class InteractionDestinationArrivalBehaviourPatienceSegment : MonoBehaviour, ISegment
{
    public ExInputNumber patienceInputField;

    private float patience;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public float Patience
    {
        get { return patience; }
        set
        {
            patience = value;

            var interactionDestinationDataList = DataEditor.DataList.Cast<InteractionDestinationElementData>().ToList();
            interactionDestinationDataList.ForEach(interactionDestinationData =>
            {
                interactionDestinationData.Patience = value;
            });
        }
    }

    public void UpdatePatience()
    {
        Patience = patienceInputField.Value;

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

        var interactionDestinationData = (InteractionDestinationElementData)DataEditor.Data.elementData;

        patience = interactionDestinationData.Patience;
    }

    public void InitializeSegment() { }

    public void OpenSegment()
    {
        patienceInputField.Value = Patience;

        gameObject.SetActive(true);
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}

