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

    public RectTransform    list_area { get; set; }
    public RectTransform    main_list,
                            list_parent;

    public Vector2  list_size { get; set; }
    public float    base_size { get; set; }

    public int selected_id;

    Vector3 listMin, listMax;

    public bool source;

    public OverlayManager overlayManager;

    public ActionManager actionManager;
    private Button edit_button;

    public void InitializeList(RowManager rowManager, List<int> new_id_list, Path new_select_path, Path new_edit_path)
    {
        id_list.Clear();
        id_list = new List<int>(rowManager.id_list);

        table = rowManager.table;

        select_path = new_select_path;
        edit_path = new_edit_path;

        editable = (rowManager.edit_index.Length > 0);

        //Possibly add/destroy components
        switch(rowManager.sort_type)
        {
            case 0: organizer = gameObject.AddComponent<DisplayOrganizer>(); break;
            case 1: organizer = gameObject.AddComponent<ListOrganizer>(); break;
            case 2: organizer = gameObject.AddComponent<GridOrganizer>(); break;
            default: break;
        }

        organizer.InitializeOrganizer(select_path, edit_path);

        overlayManager.InitializeOverlay(this);
    }

    public void SetProperties(ListProperties listProperties)
    {
        list_area = listProperties.list_area;

        base_size = listProperties.base_size;

        main_list.GetComponent<ScrollRect>().horizontal = listProperties.horizontal;
        main_list.GetComponent<ScrollRect>().vertical = listProperties.vertical;

        organizer.SetProperties(listProperties);

        overlayManager.SetOverlayProperties(listProperties);
    }

    public void SetListSize(float base_size)
    {
        organizer.SetListSize(base_size);
        
        list_parent.sizeDelta = organizer.GetListSize(true);

        listMin = main_list.TransformPoint(new Vector2(0, main_list.rect.min.y));
        listMax = main_list.TransformPoint(new Vector2(0, main_list.rect.max.y));

        main_list.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
        main_list.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0f;

        SetRows();

        list_size = organizer.GetListSize(false);

        overlayManager.SetOverlay();

        //Activate all overlay components

        //overlayManager.SetOverlay(overlayManager.sliderManager);  
        //Set numbers
        //Set slideshow

        //combine?

        //Instead of "enabling numbers", add and remove organizer components. component = enabled
    }

    public void SetRows()
    {
        if (editable)
            EnableAdding(edit_path);

        organizer.SetRows();
    }

    public void UpdateRows()
    {
        overlayManager.UpdateOverlay();
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
        Button add_button = actionManager.AddButton();

        add_button.GetComponentInChildren<Text>().text = "Add " + table;

        //TEMPORARY ID! Create placeholder and display in the future
        add_button.onClick.AddListener(delegate { OpenEditor(NewPath(add_path, 1)); });
    }

    public void EnableEditing(Path edit_path)
    {
        if (edit_button != null)
            edit_button.onClick.RemoveAllListeners();
         else 
            edit_button = actionManager.AddButton();

        edit_button.GetComponentInChildren<Text>().text = "Edit";

        edit_button.onClick.AddListener(delegate { OpenEditor(NewPath(edit_path, NavigationManager.set_id)); });
    }
        
    public Path NewPath(Path path, int id)
    {
        Path new_path = new Path(new List<int>(), new List<int>());

        for (int i = 0; i < path.editor.Count; i++)
            new_path.editor.Add(path.editor[i]);

        for (int i = 0; i < path.id.Count; i++)
            new_path.id.Add(path.id[i]);

        //Trouble (edit: is it?)
        if (new_path.id.Count < new_path.editor.Count)
            new_path.id.Add(id);
        else
            new_path.id[new_path.id.Count - 1] = id;

        return new_path;
    }

    public void CloseList()
    {
        edit_button = null;

        main_list.GetComponent<ScrollRect>().horizontal = false;
        main_list.GetComponent<ScrollRect>().vertical = false;

        overlayManager.CloseOverlay();

        organizer.CloseList();
    }

    public RectTransform SpawnElement(List<RectTransform> list, RectTransform element_prefab)
    {
        foreach(RectTransform element in list)
        {
            if (!element.gameObject.activeInHierarchy)
            {
                element.transform.SetParent(list_parent, false);
                return element;
            }     
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
}
