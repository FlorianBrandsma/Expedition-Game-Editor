using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObjectGraphicSegment : MonoBehaviour, ISegment
{
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

        var interactableElementData = (InteractableElementData)SegmentController.Path.FindLastRoute(Enums.DataType.Interactable).data.elementData;

        var objectGraphicElementData = new ObjectGraphicElementData()
        {
            Id = interactableElementData.ObjectGraphicId,
            Path = interactableElementData.objectGraphicPath,
            iconPath = interactableElementData.objectGraphicIconPath
        };

        SegmentController.DataController.DataList = new List<IElementData>() { objectGraphicElementData };
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(DataElement dataElement) { }
}
