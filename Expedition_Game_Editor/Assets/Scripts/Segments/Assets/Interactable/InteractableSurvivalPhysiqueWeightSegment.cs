using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteractableSurvivalPhysiqueWeightSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    #region UI
    public ExInputNumber weightInputNumber;
    #endregion

    #region Data Variables
    private float weight;
    #endregion

    #region Properties
    public float Weight
    {
        get { return weight; }
        set
        {
            weight = value;

            var interactableDataList = DataEditor.DataList.Cast<InteractableElementData>().ToList();
            interactableDataList.ForEach(interactableData =>
            {
                interactableData.Weight = value;
            });
        }
    }
    #endregion

    #region Methods
    public void UpdateWeight()
    {
        Weight = weightInputNumber.Value;

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

        weight = interactableData.Weight;
    }

    public void OpenSegment()
    {
        weightInputNumber.Value = Weight;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
    #endregion
}
