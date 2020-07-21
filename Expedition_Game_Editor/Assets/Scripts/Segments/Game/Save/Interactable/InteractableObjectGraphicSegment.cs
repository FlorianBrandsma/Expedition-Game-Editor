using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class InteractableObjectGraphicSegment : MonoBehaviour, ISegment
{
    private ObjectGraphicElementData objectGraphicElementData;

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

        var searchProperties = new SearchProperties(Enums.DataType.ObjectGraphic);

        var dataType = SegmentController.Path.GetLastRoute().GeneralData.DataType;

        switch (dataType)
        {
            case Enums.DataType.InteractableSave: InitializeInteractableSaveData(); break;

            default: Debug.Log("CASE MISSING: " + dataType); break;
        }
        
        SegmentController.DataController.DataList = new List<IElementData>() { objectGraphicElementData };
    }

    private void InitializeInteractableSaveData()
    {
        var interactableSaveElementData = (InteractableSaveElementData)SegmentController.Path.FindLastRoute(Enums.DataType.InteractableSave).data.elementData;

        objectGraphicElementData = new ObjectGraphicElementData()
        {
            Id = interactableSaveElementData.objectGraphicId,
            Path = interactableSaveElementData.objectGraphicPath,
            iconPath = interactableSaveElementData.objectGraphicIconPath
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
