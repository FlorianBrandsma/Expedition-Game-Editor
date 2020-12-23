using UnityEngine;
using UnityEngine.UI;

public class InteractableSaveStatusSurvivalCapacitySegment : MonoBehaviour, ISegment
{
    public Text healthText, hungerText, thirstText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public InteractableSaveEditor InteractableSaveEditor { get { return (InteractableSaveEditor)DataEditor; } }

    #region Data properties
    private int Health
    {
        get { return InteractableSaveEditor.Health; }
    }

    private int Hunger
    {
        get { return InteractableSaveEditor.Hunger; }
    }

    private int Thirst
    {
        get { return InteractableSaveEditor.Thirst; }
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
        SetSizeValues();
    }

    public void SetSizeValues()
    {
        healthText.text = (Health).ToString();
        hungerText.text = (Hunger).ToString();
        thirstText.text = (Thirst).ToString();
    }
    
    public void OpenSegment() { }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
