﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RegionController : MonoBehaviour, IDataController
{
    public Enums.RegionType regionType;

    public Search.Region searchParameters;

    private RegionDataManager regionDataManager;
    
    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.Region; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.Navigation; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Region>().FirstOrDefault(); }
    }

    public RegionController()
    {
        regionDataManager = new RegionDataManager(this);
    }

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        searchParameters.Cast<Search.Region>().FirstOrDefault().regionType = regionType;

        return regionDataManager.GetRegionDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData) { }

    public void ToggleElement(IDataElement dataElement) { }
}