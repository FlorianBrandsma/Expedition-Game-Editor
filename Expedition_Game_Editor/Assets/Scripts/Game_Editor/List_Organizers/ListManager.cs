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

    public PathManager pathManager { get; set; }

    public string table;
    public int type;

    public RectTransform    list_area { get; set; }
    public RectTransform    main_list,
                            list_parent;

    public Vector2  list_size { get; set; }
    public float    base_size { get; set; }

    public Enums.SelectionProperty selectionProperty { get; set; }
    public Enums.SelectionType selectionType { get; set; }
    private bool always_on;

    public int selected_id;

    public bool source;

    public ListData listData { get; set; }

    public OverlayManager overlayManager;
    public ActionManager actionManager;

    public SelectionGroup selectionGroup { get; set; }

    private Button edit_button;

    public void InitializeList(ListData new_listData)
    {
        listData = new_listData;

        id_list = new List<int>(listData.id_list);

        table = listData.data.table;
        type  = listData.data.type;

        select_path = listData.select_path;
        edit_path = listData.edit_path;

        /*
        if(actionManager != null)
            editable = (rowManager.pathManager.edit.editor.Count > 0);
        */

        switch(listData.sort_type)
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

        selectionGroup = controller.field.selectionGroup;

        list_area = listProperties.list_area;

        base_size = listProperties.base_size;

        main_list.GetComponent<ScrollRect>().horizontal = listProperties.horizontal;
        main_list.GetComponent<ScrollRect>().vertical = listProperties.vertical;

        selectionProperty = listProperties.selectionProperty;
        selectionType = listProperties.selectionType;
        always_on = listProperties.always_on;
 
        /*
        if(selectionType != Enums.SelectionType.None && selectionProperty != Enums.SelectionProperty.Set)
            Debug.Log(this + ":" + EditorManager.PathString(pathManager.structure));
        */

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

        main_list.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
        main_list.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0f;

        SetRows();

        overlayManager.SetOverlay();   
    }

    public void SetRows()
    {
        organizer.SetRows(id_list);

        if (selectionType == Enums.SelectionType.Automatic)
        {
            AutoSelectElement(controller.field.windowManager.main_target_editor.data.id);
        }

        //Automatically selects and highlights an element on startup by id
        if (controller.field.target_editor != controller.field.windowManager.main_target_editor)
        {
            
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

    public void AutoSelectElement(int id)
    {
        if (id <= 0)
            id = 1;

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
        /*
        if (edit_button == null)
            edit_button = actionManager.AddButton();

        edit_button.GetComponentInChildren<Text>().text = "Edit";

        edit_button.onClick.AddListener(delegate { OpenPath(NewPath(edit_path, selectionGroup.selection.data.id)); });
        */
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

        for (int i = 0; i < path.structure.Count; i++)
            new_path.structure.Add(path.structure[i]);

        for (int i = 0; i < path.id.Count; i++)
            new_path.id.Add(path.id[i]);

        //Trouble (edit: is it?)
        if (new_path.id.Count < new_path.structure.Count)
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

    public SelectionElement SpawnElement(List<SelectionElement> list, SelectionElement element_prefab, int index)
    {
        foreach(SelectionElement element in list)
        {
            if (!element.gameObject.activeInHierarchy)
            {
                SetElement(element, index);
                return element;
            }     
        }

        SelectionElement new_element = Instantiate(element_prefab);

        SetElement(new_element, index);

        list.Add(new_element);

        return new_element;
    }

    public void SetElement(SelectionElement element, int index)
    {
        element.InitializeSelection(this, index);

        element.selectionGroup = selectionGroup;

        element.transform.SetParent(list_parent, false);
    }

    public void ResetElement(List<SelectionElement> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            list[i].gameObject.SetActive(false);
            list[i].GetComponent<Button>().onClick.RemoveAllListeners();

            if (list[i].edit_button != null)
                list[i].edit_button.onClick.RemoveAllListeners();
        }
    }
}
