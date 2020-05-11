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

        var interactableDataElement = (InteractableDataElement)SegmentController.Path.FindLastRoute(Enums.DataType.Interactable).data.dataElement;

        var objectGraphicDataElement = new ObjectGraphicDataElement()
        {
            Id = interactableDataElement.ObjectGraphicId,
            Path = interactableDataElement.objectGraphicPath,
            iconPath = interactableDataElement.objectGraphicIconPath
        };

        SegmentController.DataController.DataList = new List<IDataElement>() { objectGraphicDataElement };
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = SegmentController.DataController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement) { }
}
