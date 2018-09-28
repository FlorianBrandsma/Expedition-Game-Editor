using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class GridOrganizer : MonoBehaviour, IOrganizer
{
    private List<ElementData> local_data_list;

    static public List<SelectionElement> element_list = new List<SelectionElement>();
    private List<SelectionElement> element_list_local = new List<SelectionElement>();

    //private Enums.SelectionProperty selectionProperty;
    //private SelectionManager.Type selectionType;

    private float base_size;

    private Vector2 list_size;

    private bool fit_axis;

    //private bool visible_only;

    private bool horizontal, vertical;

    private Vector2 grid_size;

    private ListManager listManager;

    public void InitializeOrganizer()
    {
        listManager = GetComponent<ListManager>();
    }

    public void SetProperties(ListProperties listProperties)
    {
        //selectionProperty = listProperties.selectionProperty;
        //selectionType = listProperties.selectionType;

        //visible_only = listProperties.visible_only;
        
        fit_axis = listProperties.fit_axis;

        grid_size = listProperties.grid_size;

        horizontal = listProperties.horizontal;
        vertical = listProperties.vertical;
    }

    public void SetListSize(float new_size)
    {
        base_size = new_size;
    }

    public Vector2 GetListSize(List<ElementData> data_list, bool exact)
    {
        Vector2 new_size;

        if (fit_axis)
        {
            int list_width  = GetListWidth();
            int list_height = GetListHeight();

            if (list_width > data_list.Count)
                list_width = data_list.Count;

            if (list_height > data_list.Count)
                list_height = data_list.Count;

            new_size = new Vector2( horizontal  ? ((data_list.Count + (data_list.Count % list_height)) * base_size) / list_height : list_width * base_size, 
                                    vertical    ? ((data_list.Count + (data_list.Count % list_width))  * base_size) / list_width  : list_height * base_size);
        } else {

            new_size = new Vector2( horizontal  ? grid_size.x * base_size : base_size,
                                    vertical    ? grid_size.y * base_size : base_size);
        }

        if (exact)
            return new Vector2(new_size.x - listManager.main_list.rect.width, new_size.y);
        else
            return new_size / base_size;
    }

    public int GetListWidth()
    {
        int x = 0;

        while (-(x * base_size / 2f) + (x * base_size) < listManager.main_list.rect.max.x)
            x++;

        return x - 1;
    }

    public int GetListHeight()
    {
        int y = 0;

        while (-(y * base_size / 2f) + (y * base_size) < listManager.main_list.rect.max.y)
            y++;

        return y - 1;
    }

    public void SetRows(List<ElementData> data_list)
    {
        local_data_list = data_list;

        SelectionElement element_prefab = Resources.Load<SelectionElement>("Editor/Organizer/Grid/Grid_Prefab");

        int i = 0;

        list_size = GetListSize(local_data_list, false);

        for (int y = 0; y < list_size.y; y++)
        {
            for (int x = 0; x < list_size.x; x++)
            {
                SelectionElement element = listManager.SpawnElement(element_list, element_prefab, local_data_list[i]);
                element_list_local.Add(element);

                listManager.element_list.Add(element);

                //Debugging
                element.name = listManager.listData.data.table + " " + i;

                SetElement(element);

                i++;

                if (i == listManager.listData.list.Count)
                    break;
            }
        }
    }

    public void ResetRows(List<ElementData> filter)
    {
        CloseList();
        SetRows(filter);
    }

    void SetElement(SelectionElement element)
    {
        RectTransform rect = element.GetComponent<RectTransform>();

        int index = local_data_list.IndexOf(element.data);

        rect.sizeDelta = new Vector2(base_size, base_size);
        
        rect.transform.localPosition = new Vector2( -((base_size * 0.5f) * (list_size.x - 1)) + (index % list_size.x * base_size),
                                                     -(base_size * 0.5f) + (listManager.list_parent.sizeDelta.y / 2f) - (Mathf.Floor(index / list_size.x) * base_size));

        rect.gameObject.SetActive(true);
    }

    public SelectionElement GetElement(int index)
    {
        return element_list_local[index];
    }

    public void CloseList()
    {
        listManager.ResetElement(element_list_local);

        DestroyImmediate(this);
    } 
}
