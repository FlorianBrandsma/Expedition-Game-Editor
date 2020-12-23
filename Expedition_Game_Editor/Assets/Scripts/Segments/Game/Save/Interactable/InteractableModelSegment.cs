using UnityEngine;
using System.Collections.Generic;

public class InteractableModelSegment : MonoBehaviour, ISegment
{
    private ModelElementData modelElementData;
    
    public SegmentController SegmentController      { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                       { get; set; }

    private ModelDataController ModelDataController { get { return GetComponent<ModelDataController>(); } }

    public void InitializeDependencies() { }

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

        ModelDataController.Data = new Data()
        {
            dataController = ModelDataController,
            dataList = new List<IElementData>() { modelElementData }
        };
        
    }

    public void InitializeSegment()
    {
        InitializeData();
    }
    
    private void InitializeInteractableSaveData()
    {
        var interactableSaveElementData = (InteractableSaveElementData)SegmentController.Path.FindLastRoute(Enums.DataType.InteractableSave).ElementData;

        modelElementData = new ModelElementData()
        {
            Id          = interactableSaveElementData.ModelId,
            Path        = interactableSaveElementData.ModelPath,
            IconPath    = interactableSaveElementData.ModelIconPath
        };
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
