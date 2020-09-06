using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteractableModelSegment : MonoBehaviour, ISegment
{
    private ModelElementData modelElementData;

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public void InitializeDependencies() { }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        var searchProperties = new SearchProperties(Enums.DataType.Model);

        var dataType = SegmentController.Path.GetLastRoute().ElementData.DataType;

        switch (dataType)
        {
            case Enums.DataType.InteractableSave: InitializeInteractableSaveData(); break;

            default: Debug.Log("CASE MISSING: " + dataType); break;
        }

#warning Looks bad with new method
        SegmentController.DataController.Data.dataList = new List<IElementData>() { modelElementData };
    }

    private void InitializeInteractableSaveData()
    {
        var interactableSaveElementData = (InteractableSaveElementData)SegmentController.Path.FindLastRoute(Enums.DataType.InteractableSave).ElementData;

        modelElementData = new ModelElementData()
        {
            Id = interactableSaveElementData.ModelId,
            Path = interactableSaveElementData.ModelPath,
            IconPath = interactableSaveElementData.ModelIconPath
        };
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
