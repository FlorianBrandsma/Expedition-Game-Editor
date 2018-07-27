using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ListManager : MonoBehaviour
{
    IController controller { get; set; }

    IOrganizer organizer;

    public bool editable { get; set; }

    private Path select_path;
    private Path edit_path;

    public List<int> id_list = new List<int>();

    public string table;

    public RectTransform    list_area { get; set; }
    public RectTransform    main_list,
                            list_parent;

    public Vector2  list_size { get; set; }
    public float    base_size { get; set; }

    private bool auto_select;

    public int selected_id;

    Vector3 listMin, listMax;

    public bool source;

    public RowManager rowManager { get; set; }

    public OverlayManager overlayManager;
    public ActionManager actionManager;

    public SelectionField selectionField;

    private Button edit_button;

    public void InitializeList(RowManager new_rowManager)
    {
        rowManager = new_rowManager;

        id_list = new List<int>(rowManager.id_list);

        table = rowManager.table;
        
        select_path = rowManager.select_path;
        edit_path = rowManager.edit_path;

        if(actionManager != null)
            editable = (rowManager.edit_index.Count > 0);

        switch(rowManager.sort_type)
        {
            case Enums.SortType.Panel: organizer = gameObject.AddComponent<PanelOrganizer>();   break;
            case Enums.SortType.List:  organizer = gameObject.AddComponent<ListOrganizer>();    break;
            case Enums.SortType.Grid:  organizer = gameObject.AddComponent<GridOrganizer>();    break;
            default: break;
        }

        organizer.InitializeOrganizer(select_path, edit_path);

        overlayManager.InitializeOverlay(this);
    }

    public void SetProperties(ListProperties listProperties)
    {
        controller = listProperties.controller;

        list_area = listProperties.list_area;

        base_size = listProperties.base_size;

        main_list.GetComponent<ScrollRect>().horizontal = listProperties.horizontal;
        main_list.GetComponent<ScrollRect>().vertical = listProperties.vertical;

        auto_select = listProperties.auto_select;

        organizer.SetProperties(listProperties);

        overlayManager.SetOverlayProperties(listProperties);
    }

    public void SetListSize(float base_size)
    {
        organizer.SetListSize(base_size);

        //Activate Borders here
        overlayManager.ActivateOverlay(organizer);

        overlayManager.SetOverlaySize();

        list_parent.sizeDelta = organizer.GetListSize(id_list, true);

        list_size = organizer.GetListSize(id_list, false);

        //listMin = main_list.TransformPoint(new Vector2(0, main_list.rect.min.y));
        //listMax = main_list.TransformPoint(new Vector2(0, main_list.rect.max.y));

        main_list.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
        main_list.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0f;

        SetRows();

        overlayManager.SetOverlay();   
    }

    public void SetRows()
    {
        organizer.SetRows(id_list);

        //Automatically selects and highlights an element on startup by id
        if (controller.GetField().target_editor != controller.GetField().windowManager.main_target_editor)
        {
            if (auto_select)
                AutoSelectElement(controller.GetField().windowManager.main_target_editor.id);
        }
    }

    public void ResetRows()
    {
        organizer.ResetRows(id_list);
    }

    public void UpdateRows()
    {
        overlayManager.UpdateOverlay();
    }

    public void OpenPath(Path new_path)
    {
        EditorManager.editorManager.windows[0].OpenPath(new_path);
    }

    public void AutoSelectElement(int id)
    {
        organizer.GetElement(id_list.IndexOf(id)).SelectElement();
    }

    public void ActivateSelection()
    {
        if (editable)
            EnableEditing(edit_path);
    }

    public void DeactivateSelection(bool reset)
    {
        if (editable)
            DisableEditing(reset);
    }

    public void EnableEditing(Path edit_path)
    {
        if (edit_button == null)
            edit_button = actionManager.AddButton();

        edit_button.GetComponentInChildren<Text>().text = "Edit";

        edit_button.onClick.AddListener(delegate { OpenPath(NewPath(edit_path, selectionField.selection.id)); });
    }
        
    public void DisableEditing(bool reset)
    {
        if(edit_button != null)
        {
            edit_button.onClick.RemoveAllListeners();

            actionManager.RemoveAction(edit_button.GetComponent<RectTransform>(), reset);

            edit_button = null;
        }
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
        DeactivateSelection(false);

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

                element.GetComponent<SelectionElement>().selectionField = selectionField;

                return element;
            }     
        }

        RectTransform new_element = Instantiate(element_prefab);
        new_element.GetComponent<SelectionElement>().selectionField = selectionField;

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

            if (list[i].GetComponent<SelectionElement>().edit_button != null)
                list[i].GetComponent<SelectionElement>().edit_button.onClick.RemoveAllListeners();
        }
    }
}
