﻿using UnityEngine;
using System.Linq;

public class SceneDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Data Data                            { get; set; }
    public Enums.DataType DataType              { get { return Enums.DataType.Scene; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.Navigation; } }

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
            dataList = SceneDataManager.GetData(searchProperties.searchParameters.Cast<Search.Scene>().First()),
            searchProperties = this.searchProperties
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(IElementData searchElementData, IElementData resultElementData) { }

    public void ToggleElement(EditorElement editorElement) { }
}