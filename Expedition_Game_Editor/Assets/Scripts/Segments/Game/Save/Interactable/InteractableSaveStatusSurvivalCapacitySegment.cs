using UnityEngine;
using UnityEngine.UI;

public class InteractableSaveStatusSurvivalCapacitySegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public Text healthText, hungerText, thirstText;
    #endregion

    #region Data Variables
    private int health;
    private int hunger;
    private int thirst;
    #endregion

    #region Methods
    public void SetSizeValues()
    {
        healthText.text = (health).ToString();
        hungerText.text = (hunger).ToString();
        thirstText.text = (thirst).ToString();
    }
    #endregion

    #region Segment
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment()
    {
        SetSizeValues();
    }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        var interactableSaveData = (InteractableSaveElementData)DataEditor.Data.elementData;

        health = interactableSaveData.health;
        hunger = interactableSaveData.hunger;
        thirst = interactableSaveData.thirst;
    }

    private void SetSearchParameters() { }

    public void OpenSegment() { }
    
    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
    #endregion
}
