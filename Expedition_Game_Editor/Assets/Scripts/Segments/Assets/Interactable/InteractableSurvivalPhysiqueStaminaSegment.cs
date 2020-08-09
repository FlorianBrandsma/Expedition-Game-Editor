using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteractableSurvivalPhysiqueStaminaSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public ExInputNumber staminaInputNumber;
    #endregion

    #region Data Variables
    private float stamina;
    #endregion

    #region Properties
    public float Stamina
    {
        get { return stamina; }
        set
        {
            stamina = value;

            var interactableDataList = DataEditor.DataList.Cast<InteractableElementData>().ToList();
            interactableDataList.ForEach(interactableData =>
            {
                interactableData.Stamina = value;
            });
        }
    }
    #endregion

    #region Methods
    public void UpdateStamina()
    {
        Stamina = staminaInputNumber.Value;

        DataEditor.UpdateEditor();
    }
    #endregion

    #region Segment
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }

    public void InitializeSegment() { }

    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        var interactableData = (InteractableElementData)DataEditor.Data.elementData;

        stamina = interactableData.Stamina;
    }

    public void OpenSegment()
    {
        staminaInputNumber.Value = Stamina;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
    #endregion
}