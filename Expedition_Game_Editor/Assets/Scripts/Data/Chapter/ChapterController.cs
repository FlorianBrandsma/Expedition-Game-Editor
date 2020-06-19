﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ChapterController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public IDataManager DataManager             { get; set; }
    
    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Chapter; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.Navigation; } }
    public List<IDataElement> DataList          { get; set; }

    public SearchProperties SearchProperties
    {
        get { return searchProperties; }
        set { searchProperties = value; }
    }

    public ChapterController()
    {
        DataManager = new ChapterDataManager(this);
    }

    public void InitializeController()
    {
        SearchProperties.Initialize();
    }

    public void SetData(SelectionElement searchElement, IDataElement resultDataElement) { }

    public void ToggleElement(IDataElement dataElement) { }
}
