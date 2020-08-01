using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteractableSurvivalCapacityThirstSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public ExInputNumber thirstInputNumber;
    #endregion

    #region Data Variables
    private int thirst;
    #endregion

    #region Properties
    public int Thirst
    {
        get { return thirst; }
        set
        {
            thirst = value;

            var interactableDataList = DataEditor.DataList.Cast<InteractableElementData>().ToList();
            interactableDataList.ForEach(interactableData =>
            {
                interactableData.Thirst = value;
            });
        }
    }
    #endregion

    #region Methods
    public void UpdateThirst()
    {
        Thirst = (int)thirstInputNumber.Value;

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

        thirst = interactableData.Thirst;
    }

    private void SetSearchParameters() { }

    public void OpenSegment()
    {
        thirstInputNumber.Value = Thirst;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
    #endregion
}