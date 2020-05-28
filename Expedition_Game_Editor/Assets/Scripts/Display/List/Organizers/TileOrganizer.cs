using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class TileOrganizer : MonoBehaviour, IOrganizer, IList
{
    private IDisplayManager DisplayManager  { get { return GetComponent<IDisplayManager>(); } }
    private ListManager ListManager         { get { return (ListManager)DisplayManager; } }
    
    private ListProperties ListProperties   { get { return (ListProperties)DisplayManager.Display; } }
    private TileProperties TileProperties   { get { return (TileProperties)DisplayManager.Display.Properties; } }

    private IDataController DataController  { get { return DisplayManager.Display.DataController; } }
    
    public List<SelectionElement> ElementList { get; set; }

    public Vector2 ElementSize { get { return ListProperties.elementSize; } }

    public void InitializeOrganizer()
    {
        ElementList = new List<SelectionElement>();
    }

    public void SelectData()
    {
        SelectionManager.SelectData(DataController.DataList, DisplayManager);
    }
    
    public void SetData()
    {
        SetData(DataController.DataList);
    }

    public void UpdateData()
    {
        ResetData(DataController.DataList);
    }

    public void ResetData(List<IDataElement> filter)
    {
        ClearOrganizer();
        SetData(filter);
    }

    public void SetData(List<IDataElement> list)
    {
        string elementType = Enum.GetName(typeof(Enums.ElementType), TileProperties.elementType);

        var prefab = Resources.Load<ExTile>("Elements/UI/" + elementType);

        foreach (IDataElement data in list)
        {
            var tile = (ExTile)PoolManager.SpawnObject(0, prefab);
            
            SelectionElementManager.InitializeElement(  tile.Element, ListManager.listParent,
                                                        DisplayManager,
                                                        DisplayManager.Display.SelectionType,
                                                        DisplayManager.Display.SelectionProperty);
            ElementList.Add(tile.Element);

            data.SelectionElement = tile.Element;
            tile.Element.data = new SelectionElement.Data(DataController, data);

            //Debugging
            GeneralData generalData = (GeneralData)data;
            tile.name = generalData.DebugName + generalData.Id;
            //

            SetElement(tile.Element);
        }
    }

    private void SetElement(SelectionElement element)
    {
        element.RectTransform.sizeDelta = new Vector2(ElementSize.x, ElementSize.y);

        int index = DataController.DataList.FindIndex(x => x.Id == element.GeneralData.Id);
        element.transform.localPosition = GetElementPosition(index);
        
        element.gameObject.SetActive(true);

        element.SetElement();
        element.SetOverlay();
    }

    public Vector2 GetElementPosition(int index)
    {
        var position = new Vector2(-((ElementSize.x * 0.5f) * (ListManager.listSize.x - 1)) + (index % ListManager.listSize.x * ElementSize.x),
                                    -(ElementSize.y * 0.5f) + (ListManager.listParent.sizeDelta.y / 2f) - (Mathf.Floor(index / ListManager.listSize.x) * ElementSize.y));

        return position;
    }

    public Vector2 GetListSize(int elementCount, bool exact)
    {
        Vector2 gridSize;

        if (ListProperties.horizontal && ListProperties.vertical)
        {
            gridSize = new Vector2( ListProperties.horizontal   ? TileProperties.GridSize.x * ElementSize.x : ElementSize.x,
                                    ListProperties.vertical     ? TileProperties.GridSize.y * ElementSize.y : ElementSize.y);
        } else {

            int listWidth = GetListWidth(elementCount);
            int listHeight = GetListHeight(elementCount);

            //No cases where a Tile only has a horizontal slider. Calculation will be added if or when necessary
            gridSize = new Vector2( ListProperties.horizontal   ? 0                                                             : listWidth * ElementSize.x,
                                    ListProperties.vertical     ? (Mathf.Ceil(elementCount / (float)listWidth) * ElementSize.y) : listHeight * ElementSize.y);
        }

        if (exact)
            return new Vector2(gridSize.x - ListManager.RectTransform.rect.width, gridSize.y);
        else
            return new Vector2(gridSize.x / ElementSize.x, gridSize.y / ElementSize.y);
    }

    public int GetListWidth(int elementCount)
    {
        int x = 0;

        while (x <= elementCount && -(x * ElementSize.x / 2f) + (x * ElementSize.x) < ListManager.RectTransform.rect.max.x)
            x++;

        return x - 1;
    }

    public int GetListHeight(int elementCount)
    {
        int y = 0;

        while (y <= elementCount && -(y * ElementSize.y / 2f) + (y * ElementSize.y) < ListManager.RectTransform.rect.max.y)
            y++;

        return y - 1;
    }
    
    public void ClearOrganizer()
    {
        ElementList.ForEach(x => PoolManager.ClosePoolObject(x.Poolable));
        SelectionElementManager.CloseElement(ElementList);
    }

    private void CancelSelection()
    {
        SelectionManager.CancelSelection(DataController.DataList);
    }

    public void CloseOrganizer()
    {
        ClearOrganizer();

        CancelSelection();

        DestroyImmediate(this);
    }
}
