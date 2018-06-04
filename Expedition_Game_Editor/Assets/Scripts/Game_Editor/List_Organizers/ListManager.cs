using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ListManager : MonoBehaviour
{
    IOrganizer organizer;

    public bool editable;

    private Path select_path;
    private Path edit_path;
    
    public List<int> id_list = new List<int>();

    public string table;

    public RectTransform main_list,
                         list_parent,
                         number_parent;

    public Slider slider;

    public int selected_id;

    public int sort_type;

    Vector3 listMin, listMax;

    public bool source;

    public GameObject list_options;

    private Button edit_button;

    OptionOrganizer optionOrganizer;

    public void SetAxis(bool horizontal, bool vertical)
    {
        main_list.GetComponent<ScrollRect>().horizontal = horizontal;
        main_list.GetComponent<ScrollRect>().vertical = vertical;
    }

    public void SetupList(RowManager rowManager, List<int> new_id_list, Path new_select_path, Path new_edit_path)
    {
        id_list.Clear();
        id_list = new List<int>(rowManager.id_list);

        table = rowManager.table;

        select_path = new_select_path;
        edit_path = new_edit_path;

        editable = (rowManager.edit_index.Length > 0);

        sort_type = rowManager.sort_type;

        switch (sort_type)
        {
            case 0:
                organizer = GetComponent<DisplayOrganizer>();
                GetComponent<DisplayOrganizer>().SetProperties(select_path, edit_path, rowManager.visible_only, rowManager.zigzag);
                break;
            case 1:
                organizer = GetComponent<ListOrganizer>();
                GetComponent<ListOrganizer>().SetProperties(edit_path, rowManager.get_select, rowManager.set_select, rowManager.visible_only);
                break;
            case 2:
                organizer = GetComponent<GridOrganizer>();
                GetComponent<GridOrganizer>().SetProperties(edit_path, rowManager.get_select, rowManager.set_select, rowManager.visible_only, rowManager.coordinate_mode, rowManager.limitless);
                break;
            default:
                break;
        }

        organizer.SetListSize(rowManager.row_size); 
    }

    public void SetListSize(float rect_width)
    {
        //Exception: not nice
        if(sort_type == 0)
            GetComponent<DisplayOrganizer>().SetList(rect_width);

        list_parent.sizeDelta = organizer.GetListSize();

        listMin = main_list.TransformPoint(new Vector2(0, main_list.rect.min.y));
        listMax = main_list.TransformPoint(new Vector2(0, main_list.rect.max.y));

        main_list.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;

        slider.gameObject.SetActive(list_parent.sizeDelta.y > main_list.rect.max.y * 2);

        SetRows();
    }

    public void SetRows()
    {
        optionOrganizer = list_options.GetComponent<OptionOrganizer>();

        if (editable)
            EnableAdding(edit_path);

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
        NavigationManager.navigation_manager.OpenStructure(new_editor, source, false);
    }

    public void OpenSource(Path new_editor)
    {
        NavigationManager.navigation_manager.OpenSource(new_editor);
    }

    public void SelectElement(int id, bool edit)
    {
        if (edit)
            EnableEditing(edit_path);

        organizer.SelectElement(id);

        NavigationManager.SelectElement(id);
    }

    public void EnableAdding(Path add_path)
    {
        Button add_button = GetComponent<OptionManager>().AddButton();

        add_button.GetComponentInChildren<Text>().text = "Add " + table;

        //TEMPORARY ID! Create placeholder and display in the future
        add_button.onClick.AddListener(delegate { OpenEditor(NewPath(add_path, 1)); });
    }

    public void EnableEditing(Path edit_path)
    {
        if (edit_button == null)
            edit_button = GetComponent<OptionManager>().AddButton();
        else
            edit_button.onClick.RemoveAllListeners();

        edit_button.onClick.AddListener(delegate { OpenEditor(NewPath(edit_path, NavigationManager.set_id)); });

        edit_button.GetComponentInChildren<Text>().text = "Edit";

        GetComponent<OptionManager>().optionOrganizer.SortOptions();
    }

    //Not sure
    public List<int> NewSelect(List<int> select_path, int new_index)
    {
        return null;
    }
    public List<int> NewEdit(List<int> edit_path, int new_index)
    {
        return null;
    }

    

    public Path NewPath(Path path, int id)
    {
        Path new_path = new Path(new List<int>(), new List<int>());

        for (int i = 0; i < path.editor.Count; i++)
            new_path.editor.Add(path.editor[i]);

        for (int i = 0; i < path.id.Count; i++)
            new_path.id.Add(path.id[i]);

        //Trouble
        if (new_path.id.Count < new_path.editor.Count)
            new_path.id.Add(id);
        else
            new_path.id[new_path.id.Count - 1] = id;

        return new_path;
    }

    public void CloseList()
    {
        main_list.GetComponent<ScrollRect>().horizontal = false;
        main_list.GetComponent<ScrollRect>().vertical = false;

        edit_button = null;

        organizer.CloseList();

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

        new_element.transform.SetParent(list_parent, false);

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

    static public Text SpawnText(List<Text> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].gameObject.activeInHierarchy)
            {
                list[i].gameObject.SetActive(true);
                return list[i];
            }
        }

        Text new_text = Instantiate(Resources.Load<Text>("Editor/Text"));
        list.Add(new_text);

        return new_text;
    }
}
