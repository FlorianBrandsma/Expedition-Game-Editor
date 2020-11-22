using UnityEngine;
using UnityEngine.UI;

public class InteractableSaveStatusSurvivalPhysiqueSegment : MonoBehaviour, ISegment
{
    public Text weightText, speedText, staminaText;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public InteractableSaveEditor InteractableSaveEditor { get { return (InteractableSaveEditor)DataEditor; } }

    #region Data properties
    private float Weight
    {
        get { return InteractableSaveEditor.Weight; }
    }
    
    private float Speed
    {
        get { return InteractableSaveEditor.Speed; }
    }

    private float Stamina
    {
        get { return InteractableSaveEditor.Stamina; }
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
        weightText.text = (Weight).ToString();
        speedText.text = (Speed).ToString();
        staminaText.text = (Stamina).ToString();
    }
    
    public void OpenSegment() { }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
