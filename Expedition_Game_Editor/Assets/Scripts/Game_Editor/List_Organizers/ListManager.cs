using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ListManager : MonoBehaviour
{
    IOrganizer organizer;

    public List<int> id_list = new List<int>();

    public string table;

    public RectTransform main_list,
                         list_parent,
                         number_parent;

    public float list_size;
    public float list_offset;

    public Slider slider;

    public int selected_id;
    private bool get_select, set_select;

    public int sort_type;

    Vector3 listMin, listMax;

    public bool source;

    public Button add_button;
    public Button edit_button;

    public void SetupList(int new_sort_type, string new_table, List<int> new_id_list, float new_base_height, Path base_select_path, Path base_edit_path, bool zigzag, bool new_get_select, bool new_set_select)
    {
        id_list.Clear();
        id_list = new List<int>(new_id_list);

        table = new_table;

        sort_type = new_sort_type;

        get_select = new_get_select;
        set_select = new_set_select;

        switch(sort_type)
        {
            case 0:
                organizer = GetComponent<DisplayOrganizer>();
                GetComponent<DisplayOrganizer>().SetProperties(base_select_path, base_edit_path, zigzag);
                break;
            case 1:
                organizer = GetComponent<ListOrganizer>();
                GetComponent<ListOrganizer>().SetProperties(base_edit_path, get_select, set_select);
                break;
            case 2:
                organizer = GetComponent<GridOrganizer>();
                GetComponent<GridOrganizer>().SetProperties(base_edit_path, get_select, set_select);
                break;
            default:
                break;
        }

        organizer.OpenList(new_base_height);

        if(edit_button != null)
        {
            DisableEditing();
        }

        if (base_edit_path.editor.Count > 0)
            EnableAdding(base_edit_path);
    }

    public void SetList(float rect_width)
    {
        //Exception: not nice
        if(sort_type == 0)
            GetComponent<DisplayOrganizer>().SetList(rect_width);

        //Return Vector2 instead of int
        //list_parent.sizeDelta = new Vector2(list_parent.sizeDelta.x, organizer.GetListSize());

        list_parent.sizeDelta = organizer.GetListSize();

        listMin = main_list.TransformPoint(new Vector2(0, main_list.rect.min.y));
        listMax = main_list.TransformPoint(new Vector2(0, main_list.rect.max.y));

        main_list.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;

        slider.gameObject.SetActive(list_parent.sizeDelta.y > main_list.rect.max.y * 2);

        organizer.SetRows();
    }

    public void UpdateRows()
    {
        SetSlider();
    }

    void SetSlider()
    {
        if (slider.gameObject.activeInHierarchy)
            slider.value = Mathf.Clamp(main_list.GetComponent<ScrollRect>().verticalNormalizedPosition, 0, 1);
    }

    public void OpenEditor(Path new_editor)
    {
        NavigationManager.navigation_manager.OpenEditor(new_editor, source, false);
    }

    public void OpenSource(Path new_editor)
    {
        NavigationManager.navigation_manager.OpenSource(new_editor);
    }

    public void SelectElement(int id, bool edit)
    {     
        if (edit)
            EnableEditing();

        organizer.SelectElement(id);

        NavigationManager.SelectElement(id);
    }

    public void EnableAdding(Path add_path)
    {
        add_button.onClick.RemoveAllListeners();
        //Temp id
        add_button.onClick.AddListener(delegate { OpenEditor(NewPath(add_path, 0)); });

        main_list.offsetMin = new Vector2(main_list.offsetMin.x, list_size);

        add_button.GetComponentInChildren<Text>().text = "Add " + table;

        add_button.gameObject.SetActive(true); 
    }

    public Path NewPath(Path path, int index)
    {
        Path new_path = new Path(new List<int>(), new List<int>());

        for (int i = 0; i < path.editor.Count; i++)
        {
            new_path.editor.Add(path.editor[i]);
            new_path.id.Add(0);
        }

        new_path.id[new_path.id.Count - 1] = id_list[index];

        return new_path;
    }

    public void DisableAdding()
    {
        main_list.offsetMin = new Vector2(main_list.offsetMin.x, list_size - list_offset);

        add_button.gameObject.SetActive(false);
    }

    public void EnableEditing()
    {
        if (add_button != null)
            add_button.GetComponent<RectTransform>().anchorMax = new Vector2(0.8f, 1);

        edit_button.gameObject.SetActive(true);
    }

    void DisableEditing()
    {
        if (add_button != null)
            add_button.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);

        edit_button.gameObject.SetActive(false);
    }

    public void CloseList()
    {
        organizer.CloseList();

        if(add_button != null)
            DisableAdding();

        gameObject.SetActive(false);
    }

    public RectTransform SpawnElement(List<RectTransform> list, RectTransform element_prefab)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].gameObject.activeInHierarchy)
                return list[i];
        }
        RectTransform new_element = Instantiate(element_prefab);
        list.Add(new_element);

        return new_element;
    }

    public void ResetElement(List<RectTransform> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].gameObject.SetActive(false);
            list[i].GetComponent<Button>().onClick.RemoveAllListeners();

            if (list[i].GetComponent<ListElement>().edit_button != null)
                list[i].GetComponent<ListElement>().edit_button.onClick.RemoveAllListeners();
        }
    }
}
