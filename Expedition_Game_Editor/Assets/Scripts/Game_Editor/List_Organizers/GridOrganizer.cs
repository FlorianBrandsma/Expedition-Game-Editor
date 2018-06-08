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
    private bool show_numbers;
    private bool coordinate_mode;

    private ListManager listManager;

    public void InitializeOrganizer(Path new_select_path, Path new_edit_path)
    {
        listManager = GetComponent<ListManager>();

        edit_path = new_edit_path;
    }

    public void SetProperties(ListProperties listProperties)
    {
        visible_only = listProperties.visible_only;
        show_numbers = listProperties.enable_numbers;
        coordinate_mode = listProperties.coordinate_mode;
    }

    public void SetListSize(float new_size)
    {
        base_size = new_size;

        //SetAnchors();
    }

    public Vector2 GetListSize()
    {
        Vector2 new_size;

        if (!coordinate_mode)
        {
            int temp_width = GetListWidth(base_size);

            new_size = new Vector2(temp_width * base_size, ((listManager.id_list.Count + (listManager.id_list.Count % temp_width)) * base_size) / temp_width);
        } else
            new_size = new Vector2(listManager.id_list.Count * base_size, listManager.id_list.Count * base_size);

        //Change coordinate size after id_list is no longer placeholder
        //new_size = new Vector2(Mathf.Sqrt(listManager.id_list.Count) * base_size, Mathf.Sqrt(listManager.id_list.Count) * base_size);

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

                //Review
                new_element.GetComponent<Button>().onClick.AddListener(delegate { listManager.SelectElement(listManager.id_list[index], listManager.editable); });

                new_element.gameObject.SetActive(true);

                index++;

                //TEMPORARY
                if (coordinate_mode && index == listManager.id_list.Count * listManager.id_list.Count)
                    break;

                if (!coordinate_mode && index == listManager.id_list.Count)
                    break;
            }
        } 
    }

    void SetElement(RectTransform rect, int x)
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
        rect.transform.localPosition = new Vector2( -((base_size * 0.5f) * (list_size.x - 1)) + (x % list_size.x * base_size),
                                                     -(base_size * 0.5f) + (listManager.list_parent.sizeDelta.y / 2f) - (Mathf.Floor(x / list_size.x) * base_size));
    }

    public void SelectElement(int id)
    {
        if (!coordinate_mode)
            SetElement(element_selection, id - 1);


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
    } 
}
