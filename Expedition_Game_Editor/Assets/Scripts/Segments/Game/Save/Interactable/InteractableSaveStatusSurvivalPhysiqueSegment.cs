using UnityEngine;
using UnityEngine.UI;

public class InteractableSaveStatusSurvivalPhysiqueSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public Text weightText, speedText, staminaText;
    #endregion

    #region Data Variables
    private float weight;
    private float speed;
    private float stamina;
    #endregion

    #region Methods
    public void SetSizeValues()
    {
        weightText.text = (weight).ToString();
        speedText.text = (speed).ToString();
        staminaText.text = (stamina).ToString();
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

        weight = interactableSaveData.weight;
        speed = interactableSaveData.speed;
        stamina = interactableSaveData.stamina;
    }

    public void OpenSegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
    #endregion
}
