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

    public Vector2 GetListSize(List<int> id_list, bool exact)
    {
        Vector2 new_size;

        if (fit_axis)
        {
            int list_width  = GetListWidth();
            int list_height = GetListHeight();

            if (list_width > id_list.Count)
                list_width = id_list.Count;

            if (list_height > id_list.Count)
                list_height = id_list.Count;

            new_size = new Vector2( horizontal  ? ((id_list.Count + (id_list.Count % list_height)) * base_size) / list_height : list_width * base_size, 
                                    vertical    ? ((id_list.Count + (id_list.Count % list_width))  * base_size) / list_width : list_height * base_size);
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

    public void SetRows(List<int> id_list)
    {
        RectTransform element_prefab = Resources.Load<RectTransform>("Editor/Organizer/Grid/Grid_Prefab");

        int i = 0;

        list_size = GetListSize(id_list, false);

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

                new_element.name = listManager.table + " " + i;

                SetElement(new_element, i);

                int index = i;

                new_element.GetComponent<Button>().onClick.AddListener(delegate { listManager.SelectElement(index, listManager.editable); });

                i++;

                if (i == listManager.id_list.Count)
                    break;
            }
        }
    }

    public void ResetRows(List<int> filter)
    {
        CloseList();
        SetRows(filter);
    }

    public void SelectElement(int id)
    {
        if (element_selection == null)
        {
            element_selection = listManager.SpawnElement(selection_list, Resources.Load<RectTransform>("Editor/Organizer/Grid/Grid_Selection"));
            element_selection.SetAsFirstSibling();
        }

        SetElement(element_selection, id);
    }

    void SetElement(RectTransform element, int index)
    {
        element.sizeDelta = new Vector2(base_size, base_size);

        element.transform.localPosition = new Vector2( -((base_size * 0.5f) * (list_size.x - 1)) + (index % list_size.x * base_size),
                                                        -(base_size * 0.5f) + (listManager.list_parent.sizeDelta.y / 2f) - (Mathf.Floor(index / list_size.x) * base_size));

        element.gameObject.SetActive(true);
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
