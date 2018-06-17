using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class GridOrganizer : MonoBehaviour, IOrganizer
{
    private List<RectTransform> element_list = new List<RectTransform>();

    public RectTransform element_prefab;
    public RectTransform element_selection;

    private Path edit_path;

    private float base_size;

    private Vector2 list_size;

    private bool get_select, set_select;

    private bool visible_only;
    private bool enable_numbers;
    private bool fit_axis;
    private bool slideshow;

    private bool horizontal, vertical;

    private Vector2 grid_size;

    private ListManager listManager;

    public void InitializeOrganizer(Path new_select_path, Path new_edit_path)
    {
        listManager = GetComponent<ListManager>();

        edit_path = new_edit_path;
    }

    public void SetProperties(ListProperties listProperties)
    {
        visible_only = listProperties.visible_only;
        enable_numbers = listProperties.enable_numbers;
        fit_axis = listProperties.fit_axis;
        slideshow = listProperties.slideshow;
        grid_size = listProperties.grid_size;

        horizontal = listProperties.horizontal;
        vertical = listProperties.vertical;
    }

    public void SetListSize(float new_size)
    {
        base_size = new_size;

        //SetAnchors();
    }

    public Vector2 GetListSize()
    {
        Vector2 new_size;

        if (fit_axis)
        {
            int list_width = GetListWidth(base_size);
            int list_height = GetListHeight(base_size);

            new_size = new Vector2( horizontal  ? list_width * base_size : list_width * base_size, 
                                    vertical    ? ((listManager.id_list.Count + (listManager.id_list.Count % list_width)) * base_size) / list_width : list_height * base_size);
        } else {

            new_size = new Vector2( horizontal  ? grid_size.x * base_size : base_size,
                                    vertical    ? grid_size.y * base_size : base_size);
        }
            
        //Instead of sqrt size, use vertical/horizontal to determine the size
        //vertical * horizontal (either no lower than 1)
        //In case of regions: they're a given
        //Summary: don't use id_list to determine the size for "coordinate mode"

        list_size = new_size / base_size;

        return new Vector2(new_size.x - listManager.main_list.rect.width, new_size.y);  
    }

    int GetListWidth(float new_size)
    {
        int x = 0;

        while (-(x * new_size / 2f) + (x * new_size) < listManager.main_list.rect.max.x)
            x++;

        return x - 1;
    }

    int GetListHeight(float new_size)
    {
        int y = 0;

        while (-(y * new_size / 2f) + (y * new_size) < listManager.main_list.rect.max.y)
            y++;

        return y - 1;
    }

    public void SetRows()
    {
        int index = 0;
        
        for (int y = 0; y < list_size.y; y++)
        {
            for (int x = 0; x < list_size.x; x++)
            {
                /*
                while (ListPosition(y) < listMax.y)
                {
                    y++;
                    x += maxWidth;
                }

                if (ListPosition(y) > listMin.y)
                    break;
                    */

                RectTransform new_element = listManager.SpawnElement(element_list, element_prefab);
                new_element.transform.SetParent(listManager.list_parent, false);

                new_element.name = listManager.table + " " + index;

                SetElement(new_element, index);

                int temp_index = index;

                new_element.GetComponent<Button>().onClick.AddListener(delegate { listManager.SelectElement(listManager.id_list[temp_index], listManager.editable); });

                new_element.gameObject.SetActive(true);

                index++;

                if (index == listManager.id_list.Count)
                    break;
            }
        }

        if (enable_numbers)
            InitializeNumbers();
    }

    void InitializeNumbers()
    {
        for(int y = 0; y < list_size.y; y++)
            listManager.numberManager.SetNumbers(listManager.numberManager.vertical_number_parent, y, new Vector2(0, -(base_size * 0.5f) + (listManager.list_parent.sizeDelta.y / 2f) - (y * base_size)));

        for (int x = 0; x < list_size.x; x++)
            listManager.numberManager.SetNumbers(listManager.numberManager.horizontal_number_parent, x, new Vector2(-((base_size * 0.5f) * (list_size.x - 1)) + (x * base_size), 0));
    }

    void SetElement(RectTransform rect, int index)
    {
        rect.sizeDelta = new Vector2(base_size, base_size);

        /*
        float offset_x = 0f;
        float offset_y = 0f;

        if (GetComponent<ListManager>().vertical_slider.gameObject.activeInHierarchy)
            offset_x = 10;
        if (GetComponent<ListManager>().horizontal_slider.gameObject.activeInHierarchy)
            offset_y = 10;
        */

        rect.transform.localPosition = new Vector2( -((base_size * 0.5f) * (list_size.x - 1)) + (index % list_size.x * base_size),
                                                     -(base_size * 0.5f) + (listManager.list_parent.sizeDelta.y / 2f) - (Mathf.Floor(index / list_size.x) * base_size));
    }

    public void SelectElement(int id)
    {
        SetElement(element_selection, id - 1);

        element_selection.gameObject.SetActive(true);
    }

    public void ResetSelection()
    {
        element_selection.gameObject.SetActive(false);
    }

    public void CloseList()
    {
        listManager.ResetElement(element_list);

        ResetSelection();
    } 
}
