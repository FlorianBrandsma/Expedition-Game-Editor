using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class TileOrganizer : MonoBehaviour, IOrganizer, IList
{
    private IDisplayManager DisplayManager  { get { return GetComponent<IDisplayManager>(); } }
    private ListManager ListManager         { get { return (ListManager)DisplayManager; } }
    
    private ListProperties ListProperties   { get { return (ListProperties)DisplayManager.Display; } }
    private TileProperties TileProperties   { get { return (TileProperties)DisplayManager.Display.Properties; } }

    private IDataController DataController  { get { return DisplayManager.Display.DataController; } }
    
    public List<EditorElement> ElementList  { get; set; }

    public Vector2 ElementSize              { get { return ListProperties.elementSize; } }
    
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

        var tilePrefab = Resources.Load<ExTile>("Elements/UI/" + elementType);

        foreach (IElementData elementData in list.Where(elementData => !(elementData is PlaceholderElementData)))
        {
            var tile = (ExTile)PoolManager.SpawnObject(tilePrefab);
            tile.transform.SetParent(ListManager.listParent);

            var dataElement = tile.EditorElement.DataElement;

            dataElement.InitializeDisplayManager(DisplayManager);

            dataElement.InitializeDisplayProperties(DisplayManager.Display.SelectionType,
                                                    DisplayManager.Display.SelectionProperty,
                                                    DisplayManager.Display.AddProperty,
                                                    DisplayManager.Display.UniqueSelection);

            InitializeElement(elementData, tile.EditorElement);
        }

        if (!ListProperties.enablePlaceholders) return;

        var placeholderPrefab = Resources.Load<ExPlaceholder>("Elements/UI/Placeholder");

        foreach (IElementData elementData in list.Where(elementData => (elementData is PlaceholderElementData)))
        {
            var placeholder = (ExPlaceholder)PoolManager.SpawnObject(placeholderPrefab);
            placeholder.transform.SetParent(ListManager.listParent);

            placeholder.RectTransform.anchorMin = ListProperties.anchorMin;
            placeholder.RectTransform.anchorMax = ListProperties.anchorMax;

            InitializeElement(elementData, placeholder.EditorElement);
        }
    }

    private void InitializeElement(IElementData elementData, EditorElement editorElement)
    {
        ElementList.Add(editorElement);

        var dataElement = editorElement.DataElement;

        dataElement = editorElement.DataElement;

        dataElement.Data    = DataController.Data;
        dataElement.Id      = elementData.Id;
        dataElement.Path    = DisplayManager.Display.DataController.SegmentController.Path;

        dataElement.InitializeDisplayManager(DisplayManager);

        //Debugging
        editorElement.name = elementData.DebugName + elementData.Id;

        SetElement(editorElement);
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

    public Vector2 GetListSize(bool exact)
    {
        var elementCount = DataController.Data.dataList.Count;

        Vector2 gridSize;

        if (ListProperties.horizontal && ListProperties.vertical)
        {
            gridSize = new Vector2( ListProperties.horizontal   ? TileProperties.GridSize.x * ElementSize.x : ElementSize.x,
                                    ListProperties.vertical     ? TileProperties.GridSize.y * ElementSize.y : ElementSize.y);
        } else {

            var listWidth   = GetListWidth(elementCount);
            var listHeight  = GetListHeight(elementCount);

            gridSize = new Vector2( ListProperties.horizontal   ? (Mathf.Ceil(elementCount / (float)listHeight) * ElementSize.x) : listWidth * ElementSize.x,
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
