using UnityEngine;
using System.Linq;

public class InteractionDestinationArrivalBehaviourPatienceSegment : MonoBehaviour, ISegment
{
    public ExInputNumber patienceInputField;
    
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public InteractionDestinationEditor InteractionDestinationEditor { get { return (InteractionDestinationEditor)DataEditor; } }

    #region Data properties
    private float Patience
    {
        get { return InteractionDestinationEditor.Patience; }
        set { InteractionDestinationEditor.Patience = value; }
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
        patienceInputField.Value = Patience;

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdatePatience()
    {
        Patience = patienceInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}

