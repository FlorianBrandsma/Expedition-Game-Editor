using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterInteractableController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public IDataManager DataManager             { get; set; }

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.ChapterInteractable; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public SearchProperties SearchProperties
    {
        get { return searchProperties; }
        set { searchProperties = value; }
    }

    public ChapterInteractableController()
    {
        DataManager = new ChapterInteractableDataManager(this);
    }

    public void InitializeController()
    {
        SearchProperties.Initialize();
    }

    public void SetData(DataElement searchElement, IDataElement resultData)
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

    public void ToggleElement(EditorElement editorElement) { }
}
