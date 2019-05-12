using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TileOrganizer : MonoBehaviour, IOrganizer, IList
{
    private ListManager listManager { get { return GetComponent<ListManager>(); } }

    static public List<SelectionElement> elementList = new List<SelectionElement>();

    public Vector2 ElementSize { get; set; }
    private Vector2 list_size;

    private TileProperties properties;
    private bool horizontal, vertical;

    private IDataController dataController;
    private List<GeneralData> generalData_list;

    public void InitializeOrganizer()
    {
        dataController = listManager.listProperties.DataController;
    }

    public void SetProperties()
    {
        properties = listManager.listProperties.GetComponent<TileProperties>();

        horizontal = listManager.listProperties.horizontal;
        vertical = listManager.listProperties.vertical;
    }

    public void SetElementSize()
    {
        ElementSize = listManager.listProperties.elementSize;
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
            return new Vector2(new_size.x - listManager.rectTransform.rect.width, new_size.y);
        else
            return new Vector2(new_size.x / ElementSize.x, new_size.y / ElementSize.y);
    }

    public int GetListWidth(int elementCount)
    {
        int x = 0;

        while (x <= elementCount && -(x * ElementSize.x / 2f) + (x * ElementSize.x) < listManager.rectTransform.rect.max.x)
            x++;

        return x - 1;
    }

    public int GetListHeight(int elementCount)
    {
        int y = 0;

        while (y <= elementCount && -(y * ElementSize.y / 2f) + (y * ElementSize.y) < listManager.rectTransform.rect.max.y)
            y++;

        return y - 1;
    }

    public void SetData()
    {
        SetData(dataController.DataList);
    }

    public void SetData(ICollection list)
    {
        generalData_list = list.Cast<GeneralData>().ToList();

        SelectionElement element_prefab = Resources.Load<SelectionElement>("UI/Tile");

        list_size = GetListSize(list.Count, false);

        foreach (var data in list)
        {
            SelectionElement element = listManager.SpawnElement(elementList, element_prefab, Enums.ElementType.Tile);
            listManager.elementList.Add(element);

            element.route.data = new Data(dataController, new[] { data });

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

        SelectionManager.ResetSelection(listManager);
    }

    public void ResetData(ICollection filter)
    {
        CloseList();
        SetData(filter);
    }

    void SetElement(SelectionElement element)
    {
        element.SetElement();

        RectTransform rect = element.GetComponent<RectTransform>();

        int index = generalData_list.FindIndex(x => x.id == element.GeneralData().id);

        rect.sizeDelta = new Vector2(ElementSize.x, ElementSize.y);
        
        rect.transform.localPosition = new Vector2( -((ElementSize.x * 0.5f) * (list_size.x - 1)) + (index % list_size.x * ElementSize.x),
                                                     -(ElementSize.y * 0.5f) + (listManager.listParent.sizeDelta.y / 2f) - (Mathf.Floor(index / list_size.x) * ElementSize.y));

        rect.gameObject.SetActive(true);
    }

    public SelectionElement GetElement(int index)
    {
        return listManager.elementList[index];
    }

    public void CloseList()
    {
        listManager.ResetElement();
    }

    public void ClearOrganizer() { }

    public void CloseOrganizer()
    {
        CloseList();

        DestroyImmediate(this);
    }
}
