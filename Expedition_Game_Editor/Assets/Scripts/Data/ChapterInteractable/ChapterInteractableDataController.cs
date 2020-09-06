using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterInteractableDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    
    public Data Data                            { get; set; }
    public Enums.DataType DataType              { get { return Enums.DataType.ChapterInteractable; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    
    public SearchProperties SearchProperties
    {
        get { return searchProperties; }
        set { searchProperties = value; }
    }
    
    public void InitializeController()
    {
        SearchProperties.Initialize();
    }

    public void GetData()
    {
        GetData(searchProperties);
    }

    public void GetData(SearchProperties searchProperties)
    {
        Data = new Data()
        {
            dataController = this,
            dataList = ChapterInteractableDataManager.GetData(searchProperties)
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(DataElement searchElement, IElementData resultData)
    {
        var chapterInteractableData = (ChapterInteractableElementData)searchElement.ElementData;

        switch (resultData.DataType)
        {
            case Enums.DataType.Interactable:

                var resultElementData = (InteractableElementData)resultData;

                chapterInteractableData.InteractableId = resultElementData.Id;
                chapterInteractableData.InteractableName = resultElementData.Name;
                chapterInteractableData.ModelIconPath = resultElementData.ModelIconPath;

                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}
