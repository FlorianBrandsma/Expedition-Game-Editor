using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class GridOrganizer : MonoBehaviour, IOrganizer
{
    bool activated;

    static public List<RectTransform> element_list = new List<RectTransform>();
    private List<RectTransform> element_list_local = new List<RectTransform>();

    static public List<RectTransform> selection_list = new List<RectTransform>();
    private RectTransform element_selection;

    private Path edit_path;

    private float base_size;

    private Vector2 list_size;

    private bool get_select, set_select;

    private bool fit_axis;

    private bool visible_only;

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
        
        fit_axis = listProperties.fit_axis;

        grid_size = listProperties.grid_size;

        horizontal = listProperties.horizontal;
        vertical = listProperties.vertical;
    }

    public void SetListSize(float new_size)
    {
        base_size = new_size;
    }

    public Vector2 GetListSize(bool exact)
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
            
        list_size = new_size / base_size;

        if (exact)
            return new Vector2(new_size.x - listManager.main_list.rect.width, new_size.y);
        else
            return list_size;
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
        RectTransform element_prefab = Resources.Load<RectTransform>("Editor/Organizer/Grid/Grid_Prefab");

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

                element_list_local.Add(new_element);

                new_element.name = listManager.table + " " + index;

                SetElement(new_element, index);

                int temp_index = index;

                new_element.GetComponent<Button>().onClick.AddListener(delegate { listManager.SelectElement(listManager.id_list[temp_index], listManager.editable); });

                index++;

                if (index == listManager.id_list.Count)
                    break;
            }
        }
    }

    void SetElement(RectTransform element, int index)
    {
        element.sizeDelta = new Vector2(base_size, base_size);

        element.transform.localPosition = new Vector2( -((base_size * 0.5f) * (list_size.x - 1)) + (index % list_size.x * base_size),
                                                        -(base_size * 0.5f) + (listManager.list_parent.sizeDelta.y / 2f) - (Mathf.Floor(index / list_size.x) * base_size));

        element.gameObject.SetActive(true);
    }

    public void SelectElement(int id)
    {
        if (element_selection == null)
        {
            element_selection = listManager.SpawnElement(selection_list, Resources.Load<RectTransform>("Editor/Organizer/Grid/Grid_Selection"));
            element_selection.SetAsFirstSibling();
        }

        SetElement(element_selection, id - 1); 
    }

    public void ResetSelection()
    {
        element_selection.gameObject.SetActive(false);
    }

    public void CloseList()
    {
        if (element_selection != null)
            ResetSelection();

        listManager.ResetElement(element_list_local);

        DestroyImmediate(this);
    } 
}
