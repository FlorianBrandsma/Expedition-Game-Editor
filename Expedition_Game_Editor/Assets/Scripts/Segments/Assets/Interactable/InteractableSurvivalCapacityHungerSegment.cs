using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteractableSurvivalCapacityHungerSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public ExInputNumber hungerInputNumber;
    #endregion

    #region Data Variables
    private int hunger;
    #endregion

    #region Properties
    public int Hunger
    {
        get { return hunger; }
        set
        {
            hunger = value;

            var interactableDataList = DataEditor.DataList.Cast<InteractableElementData>().ToList();
            interactableDataList.ForEach(interactableData =>
            {
                interactableData.Hunger = value;
            });
        }
    }
    #endregion

    #region Methods
    public void UpdateHunger()
    {
        Hunger = (int)hungerInputNumber.Value;

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

        var interactableData = (InteractableElementData)DataEditor.ElementData;

        hunger = interactableData.Hunger;
    }

    public void OpenSegment()
    {
        hungerInputNumber.Value = Hunger;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
    #endregion
}
