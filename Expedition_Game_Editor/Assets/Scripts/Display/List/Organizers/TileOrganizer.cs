using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TileOrganizer : MonoBehaviour, IOrganizer, IList
{
    private ListManager ListManager { get { return GetComponent<ListManager>(); } }

    static public List<SelectionElement> elementList = new List<SelectionElement>();

    public Vector2 ElementSize { get; set; }
    private Vector2 listSize;

    private TileProperties properties;
    private bool horizontal, vertical;

    private IDataController dataController;
    private List<GeneralData> generalDataList;

    public void InitializeOrganizer()
    {
        dataController = ListManager.listProperties.DataController;
    }

    public void SetProperties()
    {
        properties = ListManager.listProperties.GetComponent<TileProperties>();

        horizontal = ListManager.listProperties.horizontal;
        vertical = ListManager.listProperties.vertical;
    }

    public void SetElementSize()
    {
        ElementSize = ListManager.listProperties.elementSize;
    }

    public Vector2 GetListSize(int elementCount, bool exact)
    {
        Vector2 new_size;

        if(horizontal && vertical)
        {
            new_size = new Vector2( horizontal  ? properties.grid_size.x * ElementSize.x : ElementSize.x,
                                    vertical    ? properties.grid_size.y * ElementSize.y : ElementSize.y);
        } else {

            int list_width  = GetListWidth(elementCount);
            int list_height = GetListHeight(elementCount);

            //No cases where a Tile only has a horizontal slider. Calculation will be added if or when necessary
            new_size = new Vector2( horizontal  ? 0                                                                 : list_width  * ElementSize.x,
                                    vertical    ? (Mathf.Ceil(elementCount / (float)list_width) * ElementSize.y)  : list_height * ElementSize.y);
        }

        if (exact)
            return new Vector2(new_size.x - ListManager.rectTransform.rect.width, new_size.y);
        else
            return new Vector2(new_size.x / ElementSize.x, new_size.y / ElementSize.y);
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

        SelectionElement elementPrefab = Resources.Load<SelectionElement>("UI/Tile");

        listSize = GetListSize(list.Count, false);

        foreach (IDataElement data in list)
        {
            SelectionElement element = ListManager.SpawnElement(elementList, elementPrefab, Enums.ElementType.Tile);
            ListManager.elementList.Add(element);

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

        rect.sizeDelta = new Vector2(ElementSize.x, ElementSize.y);
        
        rect.transform.localPosition = new Vector2( -((ElementSize.x * 0.5f) * (listSize.x - 1)) + (index % listSize.x * ElementSize.x),
                                                     -(ElementSize.y * 0.5f) + (ListManager.listParent.sizeDelta.y / 2f) - (Mathf.Floor(index / listSize.x) * ElementSize.y));

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
