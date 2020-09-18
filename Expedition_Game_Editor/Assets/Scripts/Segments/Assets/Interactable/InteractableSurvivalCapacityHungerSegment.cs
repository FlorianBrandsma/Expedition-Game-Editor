using UnityEngine;

public class InteractableSurvivalCapacityHungerSegment : MonoBehaviour, ISegment
{
    public ExInputNumber hungerInputNumber;

    public SegmentController SegmentController      { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                       { get; set; }

    public InteractableEditor InteractableEditor    { get { return (InteractableEditor)DataEditor; } }

    #region Data properties
    private int Hunger
    {
        get { return InteractableEditor.Hunger; }
        set { InteractableEditor.Hunger = value; }
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
        hungerInputNumber.Value = Hunger;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateHunger()
    {
        Hunger = (int)hungerInputNumber.Value;

        DataEditor.UpdateEditor();
    }
    
    public void CloseSegment() { }
}
