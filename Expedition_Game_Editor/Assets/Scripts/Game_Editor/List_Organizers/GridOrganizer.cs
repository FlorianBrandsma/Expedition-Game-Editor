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

    private float list_width;

    private bool get_select, set_select;

    private bool visible_only;
    private bool show_numbers;
    private bool coordinate_mode;

    private ListManager listManager;

    public void SetProperties(Path new_edit_path, bool new_get_select, bool new_set_select, bool new_visible_only, bool new_show_numbers, bool new_coordinate_mode)
    {
        listManager = GetComponent<ListManager>();

        edit_path = new_edit_path;

        get_select = new_get_select;
        set_select = new_set_select;

        visible_only = new_visible_only;
        show_numbers = new_show_numbers;
        coordinate_mode = new_coordinate_mode;

        //Add horizontal/vertical options to listproperties
        GetComponent<ScrollRect>().horizontal = coordinate_mode;
    }

    public void SetListSize(float new_size)
    {
        base_size = new_size;

        SetList();
    }

    public void SetList()
    {
        list_width = GetListWidth(base_size);
    }

    int GetListWidth(float new_size)
    {
        int x = 0;

        while (-(x * new_size / 2f) + (x * new_size) < listManager.main_list.rect.max.x)
            x++;

        return x - 1;
    }

    public Vector2 GetListSize()
    {
        return new Vector2(listManager.list_parent.sizeDelta.x, ((listManager.id_list.Count + (listManager.id_list.Count % list_width)) * base_size) / list_width);
    }

    public void SetRows()
    {
        //int y = 0;

        if (!coordinate_mode)
            SetDefaultRows();
        else
            SetCoordinateRows();
    }

    void SetDefaultRows()
    {
        for (int x = 0; x < listManager.id_list.Count; x++)
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

            new_element.name = listManager.table + " " + x;

            SetDefaultElement(new_element, x);

            //OpenEditor
            int index = x;

            //Review
            new_element.GetComponent<Button>().onClick.AddListener(delegate { listManager.SelectElement(listManager.id_list[index], listManager.editable); });

            new_element.gameObject.SetActive(true);
        }
        /*
        list_manager.list_options.GetComponent<OptionOrganizer>().edit_button.onClick.RemoveAllListeners();
        list_manager.list_options.GetComponent<OptionOrganizer>().edit_button.onClick.AddListener(delegate { list_manager.OpenEditor(list_manager.NewPath(edit_path, NavigationManager.set_id)); });
        */
    }

    void SetDefaultElement(RectTransform rect, int x)
    {
        rect.sizeDelta = new Vector2(base_size, base_size);

        float offset_x = 0f;
        float offset_y = 0f;

        if (GetComponent<ListManager>().vertical_slider.gameObject.activeInHierarchy)
            offset_x = 10;
        if (GetComponent<ListManager>().horizontal_slider.gameObject.activeInHierarchy)
            offset_y = 10;

        rect.transform.localPosition = new Vector2( -((base_size * 0.5f) * (list_width - 1)) + (x % list_width * base_size) - offset_x,
                                                     -(base_size * 0.5f) + (listManager.list_parent.sizeDelta.y / 2f) - (Mathf.Floor(x / list_width) * base_size) - offset_y);
    }

    void SetCoordinateRows()
    {

    }

    void SetCoordinateElement()
    {

    }

    public void SelectElement(int id)
    {
        if (!coordinate_mode)
            SetDefaultElement(element_selection, id - 1);
        else
            SetCoordinateElement();

        element_selection.gameObject.SetActive(true);
    }

    public void ResetSelection()
    {
        element_selection.gameObject.SetActive(false);
    }

    public void CloseList()
    {
        listManager.ResetText();

        listManager.ResetElement(element_list);

        ResetSelection();

        gameObject.SetActive(false);
    } 
}
