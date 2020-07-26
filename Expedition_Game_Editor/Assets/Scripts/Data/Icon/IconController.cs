﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class IconController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public IDataManager DataManager             { get; set; }
    
    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Icon; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IElementData> DataList          { get; set; }

    public SearchProperties SearchProperties
    {
        get { return searchProperties; }
        set { searchProperties = value; }
    }

    public IconController()
    {
        DataManager = new IconDataManager(this);
    }

    public void InitializeController()
    {
        SearchProperties.Initialize();
    }

    public void SetData(DataElement searchElement, IElementData resultData)
    {
        var iconData = (IconElementData)searchElement.data.elementData;

        switch (((GeneralData)resultData).DataType)
        {
            case Enums.DataType.Icon:

                var resultElementData = (IconElementData)resultData;

                iconData.Id = resultElementData.Id;
                iconData.Path = resultElementData.Path;

                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}