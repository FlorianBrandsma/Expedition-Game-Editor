﻿using UnityEngine;
using System.Collections.Generic;

public class PanelTileOrganizer : MonoBehaviour, IOrganizer, IList
{
    private IDisplayManager DisplayManager          { get { return GetComponent<IDisplayManager>(); } }
    private ListManager ListManager                 { get { return (ListManager)DisplayManager; } }
    
    private ListProperties ListProperties           { get { return (ListProperties)DisplayManager.Display; } }
    private PanelTileProperties PanelTileProperties { get { return (PanelTileProperties)DisplayManager.Display.Properties; } }

    private IDataController DataController          { get { return DisplayManager.Display.DataController; } }

    public List<EditorElement> ElementList          { get; set; }

    public Vector2 ElementSize                      { get { return ListProperties.elementSize; } }

    public void InitializeOrganizer()
    {
        ElementList = new List<EditorElement>();
    }

    public void SelectData()
    {
        SelectionManager.SelectData(DataController.Data.dataList, DisplayManager);
    }

    public void UpdateData()
    {
        ResetData(null);
    }

    public void ResetData(List<IElementData> filter)
    {
        ClearOrganizer();
        SetData(filter);
    }

    public void SetData()
    {
        SetData(DataController.Data.dataList);
    }

    public void SetData(List<IElementData> list)
    {
        var prefab = Resources.Load<ExPanelTile>("Elements/UI/PanelTile");

        foreach (IElementData elementData in list)
        {
            var elementPosition = ListManager.List.GetElementPosition(list.IndexOf(elementData));

            if (ListManager.ElementBelowMin(elementPosition, true))
                continue;

            if (ListManager.ElementAboveMax(elementPosition, true))
                break;
            
            var panelTile = (ExPanelTile)PoolManager.SpawnObject(prefab);
            panelTile.transform.SetParent(ListManager.listParent);

            var dataElement = panelTile.EditorElement.DataElement;

            elementData.DataElement = dataElement;

            dataElement.Data    = DataController.Data;
            dataElement.Id      = elementData.Id;
            dataElement.Path    = DisplayManager.Display.DataController.SegmentController.Path;

            dataElement.InitializeDisplayManager(DisplayManager);
            
            dataElement.InitializeDisplayProperties(DisplayManager.Display.SelectionType,
                                                    DisplayManager.Display.SelectionProperty,
                                                    DisplayManager.Display.AddProperty,
                                                    DisplayManager.Display.UniqueSelection);

            ElementList.Add(panelTile.EditorElement);

            panelTile.GetComponent<ExPanelTile>().InitializeChildElement();

            //Debugging
            panelTile.name = elementData.DebugName + elementData.Id;

            SetElement(panelTile.EditorElement, elementPosition);
        }
    }
    
    private void SetElement(EditorElement element, Vector2 elementPosition)
    {
        element.RectTransform.sizeDelta = new Vector2(ElementSize.x, ElementSize.y);

        int index = DataController.Data.dataList.FindIndex(x => x.Id == element.DataElement.ElementData.Id);
        element.transform.localPosition = elementPosition;
        
        element.gameObject.SetActive(true);

        element.DataElement.SetElement();
        element.SetOverlay();
    }

    public Vector2 GetElementPosition(int index)
    {
        var position = new Vector2(-((ElementSize.x * 0.5f) * (ListManager.listSize.x - 1)) + Mathf.Floor(index / ListManager.listSize.y) * ElementSize.x,
                                     (ElementSize.y * 0.5f) - (index % ListManager.listSize.y * ElementSize.y));

        return position;
    }

    public Vector2 GetListSize(bool exact)
    {
        var elementCount = DataController.Data.dataList.Count;

        if (elementCount == 0)
            return Vector2.zero;

        var listWidth  = GetListWidth();
        var listHeight = GetListHeight();

        if (listWidth > elementCount)
            listWidth = elementCount;

        if (listHeight > elementCount)
            listHeight = elementCount;

        //No cases where a PanelTile only has a vertical slider. Calculation will be added if or when necessary
        var newSize = new Vector2(  ListProperties.horizontal  ? ((elementCount + (elementCount % listHeight)) * ElementSize.x) / listHeight   : listWidth  * ElementSize.y,
                                    ListProperties.vertical    ? 0                                                                             : listHeight * ElementSize.y);

        if (exact)
            return new Vector2(newSize.x - ListManager.RectTransform.rect.width, newSize.y);
        else
            return new Vector2(newSize.x / ElementSize.x, newSize.y / ElementSize.y);
    }

    public int GetListWidth()
    {
        int x = 0;

        while (-(x * ElementSize.x / 2f) + (x * ElementSize.x) < ListManager.RectTransform.rect.max.x)
            x++;

        return x - 1;
    }

    public int GetListHeight()
    {
        int y = 0;

        while (-(y * ElementSize.y / 2f) + (y * ElementSize.y) < ListManager.RectTransform.rect.max.y)
            y++;

        return y - 1;
    }
    
    public void ClearOrganizer()
    {
        ElementList.ForEach(x => PoolManager.ClosePoolObject(x.DataElement.Poolable));
        SelectionElementManager.CloseElement(ElementList);
    }

    private void CancelSelection()
    {
        SelectionManager.CancelSelection(DataController.Data.dataList);
    }

    public void CloseOrganizer()
    {
        CancelSelection();

        ClearOrganizer();
        
        DestroyImmediate(this);
    }
}
