using UnityEngine;
using System.Linq;

public class InteractableSurvivalCapacityHealthSegment : MonoBehaviour, ISegment
{
    public ExInputNumber healthInputNumber;

    public SegmentController SegmentController      { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                       { get; set; }

    public InteractableEditor InteractableEditor    { get { return (InteractableEditor)DataEditor; } }

    #region Data properties
    private int Health
    {
        get { return InteractableEditor.Health; }
        set { InteractableEditor.Health = value; }
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
        healthInputNumber.Value = Health;
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateHealth()
    {
        Health = (int)healthInputNumber.Value;

        DataEditor.UpdateEditor();
    }
    
    public void CloseSegment() { }
}
