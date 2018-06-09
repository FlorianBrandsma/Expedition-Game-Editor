using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ListManager : MonoBehaviour
{
    IOrganizer organizer;

    public List<Text> number_list = new List<Text>();

    public bool editable;

    private Path select_path;
    private Path edit_path;
    
    public List<int> id_list = new List<int>();

    public string table;

    public RectTransform    main_list,
                            list_parent;

    public RectTransform    horizontal_number_parent,
                            vertical_number_parent;

    private bool    enable_numbers;

    public Slider   horizontal_slider,
                    vertical_slider;

    private float    horizontal_offset;
    private float    vertical_offset;

    public float    slider_offset;
    public float    slider_size;

    public int selected_id;

    Vector3 listMin, listMax;

    public bool source;

    public GameObject list_options;

    private Button edit_button;

    OptionOrganizer optionOrganizer;

    public void InitializeList(RowManager rowManager, List<int> new_id_list, Path new_select_path, Path new_edit_path)
    {
        id_list.Clear();
        id_list = new List<int>(rowManager.id_list);

        table = rowManager.table;

        select_path = new_select_path;
        edit_path = new_edit_path;

        editable = (rowManager.edit_index.Length > 0);

        if (rowManager.sort_type == 0)
            organizer = GetComponent<DisplayOrganizer>();
        else if (rowManager.sort_type == 1)
            organizer = GetComponent<ListOrganizer>();
        else if (rowManager.sort_type == 2)
            organizer = GetComponent<GridOrganizer>();

        organizer.InitializeOrganizer(select_path, edit_path);
    }

    public void SetProperties(ListProperties listProperties)
    {
        main_list.GetComponent<ScrollRect>().horizontal = listProperties.horizontal;
        main_list.GetComponent<ScrollRect>().vertical = listProperties.vertical;

        enable_numbers = listProperties.enable_numbers;

        organizer.SetProperties(listProperties);
    }

    public void SetListSize(float rect_width, float base_size)
    {
        organizer.SetListSize(base_size);

        if (organizer is DisplayOrganizer)
            GetComponent<DisplayOrganizer>().SetList(rect_width);

        list_parent.sizeDelta = organizer.GetListSize();

        listMin = main_list.TransformPoint(new Vector2(0, main_list.rect.min.y));
        listMax = main_list.TransformPoint(new Vector2(0, main_list.rect.max.y));

        SetSliders();

        main_list.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
        main_list.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0f;

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
        if(enable_numbers)
            UpdateNumberPositions();

        UpdateSliders();
    }

    public void SetNumbers(RectTransform parent, int index, Vector2 new_position)
    {
        Text newDigit = ListManager.SpawnText(number_list);
        newDigit.text = (index + 1).ToString();
        newDigit.transform.SetParent(parent, false);

        if (parent == vertical_number_parent) 
            newDigit.transform.localPosition = new Vector2(new_position.x, new_position.y - vertical_offset);
        else
            newDigit.transform.localPosition = new Vector2(new_position.x + horizontal_offset, new_position.y);
    }

    void UpdateNumberPositions()
    {
        vertical_number_parent.transform.localPosition = new Vector2(0, list_parent.transform.localPosition.y + main_list.offsetMin.y);
        horizontal_number_parent.transform.localPosition = new Vector2(list_parent.transform.localPosition.x + main_list.offsetMax.x, 0);
    }

    void SetSliders()
    {
        RectTransform vertical_rect = vertical_slider.GetComponent<RectTransform>();
        RectTransform horizontal_rect = horizontal_slider.GetComponent<RectTransform>();

        if (list_parent.sizeDelta.y > main_list.rect.max.y * 2)
        {
            main_list.offsetMax = new Vector2(-vertical_rect.rect.width, main_list.offsetMax.y);

            horizontal_offset = vertical_rect.rect.width / 2f;

            vertical_slider.gameObject.SetActive(true);
        }

        if ((list_parent.sizeDelta.x + main_list.rect.width) > main_list.rect.max.x * 2)
        {
            main_list.offsetMin = new Vector2(main_list.offsetMin.x, horizontal_rect.rect.height);

            list_parent.sizeDelta = new Vector2(list_parent.sizeDelta.x + horizontal_rect.rect.height, list_parent.sizeDelta.y);

            vertical_offset = horizontal_rect.rect.height / 2f;

            horizontal_slider.gameObject.SetActive(true);
        }

        if (vertical_slider.gameObject.activeInHierarchy)
            horizontal_slider.GetComponent<RectTransform>().offsetMax = new Vector2(-slider_offset, horizontal_rect.offsetMax.y);

        if (horizontal_slider.gameObject.activeInHierarchy)
            vertical_slider.GetComponent<RectTransform>().offsetMin = new Vector2(vertical_rect.offsetMin.x, slider_offset);
    }

    void UpdateSliders()
    {
        if (vertical_slider.gameObject.activeInHierarchy)
            vertical_slider.value = Mathf.Clamp(main_list.GetComponent<ScrollRect>().verticalNormalizedPosition, 0, 1);
        if (horizontal_slider.gameObject.activeInHierarchy)
            horizontal_slider.value = Mathf.Clamp(main_list.GetComponent<ScrollRect>().horizontalNormalizedPosition, 0, 1);
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

    void CloseSliders()
    {
        main_list.offsetMin = new Vector2(main_list.offsetMin.x, 0);
        main_list.offsetMax = new Vector2(0, main_list.offsetMax.y);

        horizontal_offset = 0;
        vertical_offset = 0;

        horizontal_slider.GetComponent<RectTransform>().offsetMax = new Vector2(0, horizontal_slider.GetComponent<RectTransform>().offsetMax.y);
        vertical_slider.GetComponent<RectTransform>().offsetMin = new Vector2(vertical_slider.GetComponent<RectTransform>().offsetMin.x, 0);

        vertical_slider.gameObject.SetActive(false);
        horizontal_slider.gameObject.SetActive(false);
    }

    public void CloseList()
    {
        main_list.GetComponent<ScrollRect>().horizontal = false;
        main_list.GetComponent<ScrollRect>().vertical = false;

        CloseSliders();

        edit_button = null;

        organizer.CloseList();
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

    public void ResetText()
    {
        for (int i = 0; i < number_list.Count; i++)
        {
            number_list[i].gameObject.SetActive(false);
        }
    }
}
