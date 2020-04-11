﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InteractionController : MonoBehaviour, IDataController
{
    public Search.Interaction searchParameters;

    public IDataManager DataManager             { get; set; }
    
    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Interaction; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.Navigation; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Interaction>().FirstOrDefault(); }
    }

    public InteractionController()
    {
        DataManager = new InteractionDataManager(this);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData) { }

    public void ToggleElement(IDataElement dataElement) { }
}