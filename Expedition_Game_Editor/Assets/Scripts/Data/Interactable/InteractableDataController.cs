using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InteractableDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    
    public Data Data                            { get; set; }
    public Enums.DataType DataType              { get { return Enums.DataType.Interactable; } }
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
            dataList = InteractableDataManager.GetData(searchProperties),
            searchProperties = this.searchProperties
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(IElementData searchElementData, IElementData resultElementData)
    {
        var searchInteractableElementData = (InteractableElementData)searchElementData;

        switch (resultElementData.DataType)
        {
            case Enums.DataType.Interactable:

                var resultInteractableElementData = (InteractableElementData)resultElementData;

                searchInteractableElementData.Id            = resultInteractableElementData.Id;
                searchInteractableElementData.ModelIconPath = resultInteractableElementData.ModelIconPath;

                break;

            default: Debug.Log("CASE MISSING: " + resultElementData.DataType); break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}
