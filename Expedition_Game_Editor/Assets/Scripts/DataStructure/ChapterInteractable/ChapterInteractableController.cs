using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterInteractableController : MonoBehaviour, IDataController
{
    public Search.Interactable searchParameters;

    public IDataManager DataManager             { get; set; }

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.ChapterInteractable; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Interactable>().FirstOrDefault(); }
    }

    public ChapterInteractableController()
    {
        DataManager = new ChapterInteractableDataManager(this);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData)
    {
        var chapterInteractableData = (ChapterInteractableDataElement)searchElement.data.dataElement;

        switch (((GeneralData)resultData).DataType)
        {
            case Enums.DataType.Interactable:

                var resultElementData = (InteractableDataElement)resultData;

                chapterInteractableData.InteractableId = resultElementData.Id;
                chapterInteractableData.interactableName = resultElementData.Name;
                chapterInteractableData.objectGraphicIconPath = resultElementData.objectGraphicIconPath;

                break;
        }
    }

    public void ToggleElement(IDataElement dataElement) { }
}
