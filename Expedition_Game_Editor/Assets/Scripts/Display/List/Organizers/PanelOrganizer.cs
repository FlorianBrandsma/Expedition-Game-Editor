﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class PanelOrganizer : MonoBehaviour, IOrganizer, IList
{
    static public List<SelectionElement> elementList = new List<SelectionElement>();

    private IDataController dataController;
    private List<GeneralData> generalDataList;

    private List<float> rowHeight       = new List<float>(); //Individual heights
    private List<float> rowOffsetMax    = new List<float>(); //Combined heights

    private PanelProperties properties;

    private ListManager ListManager { get { return GetComponent<ListManager>(); } }

    public Vector2 ElementSize { get; set; }

    public void InitializeOrganizer()
    {
        dataController = ListManager.listProperties.DataController;
    }

    public void SetProperties()
    {
        properties = ListManager.listProperties.GetComponent<PanelProperties>();
    }

    public void SetElementSize()
    {
        ElementSize = new Vector2(  ListManager.listProperties.elementSize.x,
                                    properties.constantHeight ? ListManager.listProperties.elementSize.y : 
                                                                ListManager.listProperties.elementSize.y / properties.referenceArea.anchorMax.x);

        SetList();
    }

    public Vector2 GetListSize(int elementCount, bool exact)
    {
        if (exact)
            return new Vector2(0, rowHeight.Sum());
        else
            return new Vector2(0, elementCount);
    }

    public void SetList()
    {
        float positionSum = 0;

        for (int i = 0; i < ListManager.listProperties.DataController.DataList.Count; i++)
        {
            rowHeight.Add(ElementSize.y);

            positionSum += ElementSize.y;
            rowOffsetMax.Add(positionSum - ElementSize.y);
        }
    }

    public void SetData()
    {
        SetData(dataController.DataList);
    }

    public void SetData(List<IDataElement> list)
    {
        generalDataList = list.Cast<GeneralData>().ToList();
        
        string elementType = Enum.GetName(typeof(Enums.ElementType), properties.elementType);

        SelectionElement elementPrefab = Resources.Load<SelectionElement>("UI/" + elementType);

        foreach (IDataElement data in list)
        {
            SelectionElement element = ListManager.SpawnElement(elementList, elementPrefab, properties.elementType);
            ListManager.elementList.Add(element);

            element.selectionProperty = ListManager.listProperties.selectionProperty;
            
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

        SelectionManager.ResetSelection(ListManager);
    }

    public void ResetData(List<IDataElement> filter)
    {
        CloseList();
        SetData(filter);
    }

    void SetElement(SelectionElement element)
    {
        element.SetElement();

        RectTransform rect = element.GetComponent<RectTransform>();

        int index = generalDataList.FindIndex(x => x.id == element.GeneralData().id);

        rect.sizeDelta = new Vector2(rect.sizeDelta.x, ElementSize.y);

        element.transform.localPosition = new Vector2(element.transform.localPosition.x, 
                                                     (ListManager.listParent.sizeDelta.y / 2) + (-ElementSize.y * index) - (ElementSize.y / 2));

        rect.gameObject.SetActive(true);     
    }

    public SelectionElement GetElement(int index)
    {
        return ListManager.elementList[index];
    }

    public void CloseList()
    {
        ListManager.CloseElement();
    }

    public void ClearOrganizer() { }

    public void CloseOrganizer()
    {
        CloseList();

        DestroyImmediate(this);
    }
}
