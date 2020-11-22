using UnityEngine;
using System.Collections.Generic;
using System;

public class TileOrganizer : MonoBehaviour, IOrganizer, IList
{
    private IDisplayManager DisplayManager  { get { return GetComponent<IDisplayManager>(); } }
    private ListManager ListManager         { get { return (ListManager)DisplayManager; } }
    
    private ListProperties ListProperties   { get { return (ListProperties)DisplayManager.Display; } }
    private TileProperties TileProperties   { get { return (TileProperties)DisplayManager.Display.Properties; } }

    private IDataController DataController  { get { return DisplayManager.Display.DataController; } }
    
    public List<EditorElement> ElementList { get; set; }

    public Vector2 ElementSize { get { return ListProperties.elementSize; } }

    public void InitializeOrganizer()
    {
        ElementList = new List<EditorElement>();
    }

    public void SelectData()
    {
        SelectionManager.SelectData(DataController.Data.dataList, DisplayManager);
    }

    public void SetData()
    {
        SetData(DataController.Data.dataList);
    }

    public void UpdateData()
    {
        ResetData(DataController.Data.dataList);
    }

    public void ResetData(List<IElementData> filter)
    {
        ClearOrganizer();
        SetData(filter);
    }

    public void SetData(List<IElementData> list)
    {
        string elementType = Enum.GetName(typeof(Enums.ElementType), TileProperties.elementType);

        var prefab = Resources.Load<ExTile>("Elements/UI/" + elementType);

        foreach (IElementData elementData in list)
        {
            var tile = (ExTile)PoolManager.SpawnObject(prefab);
            
            SelectionElementManager.InitializeElement(  tile.EditorElement.DataElement, ListManager.listParent,
                                                        DisplayManager,
                                                        DisplayManager.Display.SelectionType,
                                                        DisplayManager.Display.SelectionProperty,
                                                        DisplayManager.Display.UniqueSelection);
            ElementList.Add(tile.EditorElement);

            elementData.DataElement = tile.EditorElement.DataElement;

            tile.EditorElement.DataElement.Data = DataController.Data;
            tile.EditorElement.DataElement.Id = elementData.Id;

            tile.EditorElement.DataElement.Path = DisplayManager.Display.DataController.SegmentController.Path;

            //Debugging
            tile.name = elementData.DebugName + elementData.Id;

            SetElement(tile.EditorElement);
        }
    }

    private void SetElement(EditorElement element)
    {
        element.RectTransform.sizeDelta = new Vector2(ElementSize.x, ElementSize.y);

        int index = DataController.Data.dataList.FindIndex(x => x.Id == element.DataElement.ElementData.Id);
        element.transform.localPosition = GetElementPosition(index);
        
        element.gameObject.SetActive(true);

        element.DataElement.SetElement();
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
