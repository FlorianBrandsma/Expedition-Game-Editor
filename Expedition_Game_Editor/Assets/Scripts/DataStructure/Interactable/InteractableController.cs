﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InteractableController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public IDataManager DataManager             { get; set; }
    
    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Interactable; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public SearchProperties SearchProperties
    {
        get { return searchProperties; }
        set { searchProperties = value; }
    }

    public InteractableController()
    {
        DataManager = new InteractableDataManager(this);
    }

    public void InitializeController()
    {
        SearchProperties.Initialize();
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData)
    {
        var interactableData = (InteractableDataElement)searchElement.data.dataElement;

        switch (((GeneralData)resultData).DataType)
        {
            case Enums.DataType.Interactable:

                var resultElementData = (InteractableDataElement)resultData;

                interactableData.Id = resultElementData.Id;
                interactableData.objectGraphicIconPath = resultElementData.objectGraphicIconPath;

                break;
        }
    }

    public void ToggleElement(IDataElement dataElement) { }
}
