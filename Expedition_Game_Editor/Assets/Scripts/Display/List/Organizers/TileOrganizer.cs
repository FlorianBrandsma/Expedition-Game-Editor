﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class TileOrganizer : MonoBehaviour, IOrganizer, IList
{
    private Vector2 listSize;

    private TileProperties tileProperties;
    private bool horizontal, vertical;

    private IDataController dataController;
    private List<GeneralData> generalDataList;

    private ListManager ListManager { get { return GetComponent<ListManager>(); } }

    public List<SelectionElement> ElementList { get; set; }
    public Vector2 ElementSize { get; set; }

    public void InitializeOrganizer()
    {
        dataController = ListManager.listProperties.DataController;

        ElementList = new List<SelectionElement>();
    }

    public void InitializeProperties()
    {
        tileProperties = ListManager.listProperties.GetComponent<TileProperties>();

        horizontal = ListManager.listProperties.horizontal;
        vertical = ListManager.listProperties.vertical;

        //switch (dataController.DataType)
        //{
        //    case Enums.DataType.Terrain:    SetTerrainGridSize();   break;
        //    case Enums.DataType.Tile:       SetTileGridSize();      break;
        //    default: Debug.Log(dataController.DataType + " CASE MISSING"); break;
        //}
    }

    //private void SetTerrainGridSize()
    //{
    //    var regionData = (RegionDataElement)ListManager.listProperties.SegmentController.path.FindLastRoute("Region").data.DataElement;

    //    tileProperties.GridSize = new Vector2(regionData.RegionSize, regionData.RegionSize);
    //}

    //private void SetTileGridSize() { }

    public void SetElementSize()
    {
        ElementSize = ListManager.listProperties.elementSize;
    }

    public Vector2 GetListSize(int elementCount, bool exact)
    {
        Vector2 gridSize;

        if(horizontal && vertical)
        {
            gridSize = new Vector2( horizontal  ? tileProperties.GridSize.x * ElementSize.x : ElementSize.x,
                                    vertical    ? tileProperties.GridSize.y * ElementSize.y : ElementSize.y);
        } else {

            int listWidth  = GetListWidth(elementCount);
            int listHeight = GetListHeight(elementCount);

            //No cases where a Tile only has a horizontal slider. Calculation will be added if or when necessary
            gridSize = new Vector2( horizontal  ? 0                                                              : listWidth  * ElementSize.x,
                                    vertical    ? (Mathf.Ceil(elementCount / (float)listWidth) * ElementSize.y)  : listHeight * ElementSize.y);
        }

        if (exact)
            return new Vector2(gridSize.x - ListManager.rectTransform.rect.width, gridSize.y);
        else
            return new Vector2(gridSize.x / ElementSize.x, gridSize.y / ElementSize.y);
    }

    public int GetListWidth(int elementCount)
    {
        int x = 0;

        while (x <= elementCount && -(x * ElementSize.x / 2f) + (x * ElementSize.x) < ListManager.rectTransform.rect.max.x)
            x++;

        return x - 1;
    }

    public int GetListHeight(int elementCount)
    {
        int y = 0;

        while (y <= elementCount && -(y * ElementSize.y / 2f) + (y * ElementSize.y) < ListManager.rectTransform.rect.max.y)
            y++;

        return y - 1;
    }

    public void SetData()
    {
        SetData(dataController.DataList);
    }

    public void SetData(List<IDataElement> list)
    {
        generalDataList = list.Cast<GeneralData>().ToList();

        string elementType = Enum.GetName(typeof(Enums.ElementType), tileProperties.elementType);

        SelectionElement elementPrefab = Resources.Load<SelectionElement>("UI/" + elementType);

        listSize = GetListSize(list.Count, false);

        foreach (IDataElement data in list)
        {
            SelectionElement element = SelectionElementManager.SpawnElement(elementPrefab, tileProperties.elementType, 
                                                                            ListManager, ListManager.selectionType, ListManager.selectionProperty, ListManager.listParent);
            ElementList.Add(element);

            data.SelectionElement = element;
            element.route.data = new Data(dataController, data);

            //Debugging
            GeneralData generalData = (GeneralData)data;
            element.name = generalData.table + generalData.id;
            //

            SetElement(element);
        }
    }

    public void UpdateData()
    {
        ResetData(dataController.DataList);

        SelectionManager.SelectElements();
    }

    public void ResetData(List<IDataElement> filter)
    {
        CloseList();
        SetData(filter);
    }

    void SetElement(SelectionElement element)
    {
        RectTransform rect = element.GetComponent<RectTransform>();

        int index = generalDataList.FindIndex(x => x.id == element.GeneralData().id);

        rect.sizeDelta = new Vector2(ElementSize.x, ElementSize.y);
        
        rect.transform.localPosition = new Vector2( -((ElementSize.x * 0.5f) * (listSize.x - 1)) + (index % listSize.x * ElementSize.x),
                                                     -(ElementSize.y * 0.5f) + (ListManager.listParent.sizeDelta.y / 2f) - (Mathf.Floor(index / listSize.x) * ElementSize.y));

        rect.gameObject.SetActive(true);

        element.SetElement();
    }

    public void CloseList()
    {
        SelectionElementManager.CloseElement(ElementList);
    }

    public void ClearOrganizer() { }

    public void CloseOrganizer()
    {
        CloseList();

        DestroyImmediate(this);
    }
}
