﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainObjectController : MonoBehaviour, IDataController
{
    public Search.ObjectGraphic searchParameters;

    public TerrainObjectDataManager terrainObjectDataManager = new TerrainObjectDataManager();

    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.TerrainObject; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.ObjectGraphic>().FirstOrDefault(); }
    }

    public void InitializeController()
    {
        terrainObjectDataManager.InitializeManager(this);
    }

    public void GetData(IEnumerable searchParameters)
    {
        DataList = terrainObjectDataManager.GetTerrainObjectDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, Data resultData)
    {
        var searchElementData = (TerrainObjectDataElement)searchElement.route.data.DataElement;

        var terrainObjectDataElement = DataList.Cast<TerrainObjectDataElement>().Where(x => x.id == searchElementData.id).FirstOrDefault();

        switch (resultData.DataController.DataType)
        {
            case Enums.DataType.ObjectGraphic:

                var resultElementData = (ObjectGraphicDataElement)resultData.DataElement;

                terrainObjectDataElement.ObjectGraphicId = resultElementData.id;
                terrainObjectDataElement.objectGraphicName = resultElementData.Name;
                terrainObjectDataElement.objectGraphicIconPath = resultElementData.iconPath;

                break;
        }

        searchElement.route.data.DataElement = terrainObjectDataElement;
    }

    public void ToggleElement(IDataElement dataElement) { }
}