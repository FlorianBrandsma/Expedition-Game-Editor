﻿using UnityEngine;
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
            dataList = ChapterInteractableDataManager.GetData(searchProperties.searchParameters.Cast<Search.ChapterInteractable>().First()),
            searchProperties = this.searchProperties
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(IElementData searchElementData, IElementData resultElementData)
    {
        var searchChapterInteractableElementData = (ChapterInteractableElementData)searchElementData;

        switch (resultElementData.DataType)
        {
            case Enums.DataType.Interactable:

                var resultInteractableElementData = (InteractableElementData)resultElementData;

                searchChapterInteractableElementData.InteractableId     = resultInteractableElementData.Id;
                searchChapterInteractableElementData.InteractableName   = resultInteractableElementData.Name;
                searchChapterInteractableElementData.ModelIconPath      = resultInteractableElementData.ModelIconPath;

                break;

            default: Debug.Log("CASE MISSING: " + resultElementData.DataType); break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}
