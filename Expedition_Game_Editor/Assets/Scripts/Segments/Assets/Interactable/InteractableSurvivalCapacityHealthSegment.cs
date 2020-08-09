using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteractableSurvivalCapacityHealthSegment : MonoBehaviour, ISegment
{
    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    #region UI
    public ExInputNumber healthInputNumber;
    #endregion

    #region Data Variables
    private int health;
    #endregion

    #region Properties
    public int Health
    {
        get { return health; }
        set
        {
            health = value;

            var interactableDataList = DataEditor.DataList.Cast<InteractableElementData>().ToList();
            interactableDataList.ForEach(interactableData =>
            {
                interactableData.Health = value;
            });
        }
    }
    #endregion

    #region Methods
    public void UpdateHealth()
    {
        Health = (int)healthInputNumber.Value;
        
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

        health = interactableData.Health;
    }

    public void OpenSegment()
    {
        healthInputNumber.Value = Health;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
    #endregion
}
