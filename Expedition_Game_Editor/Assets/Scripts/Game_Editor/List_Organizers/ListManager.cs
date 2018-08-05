using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ListManager : MonoBehaviour
{
    private IController controller;
    private IOrganizer organizer;

    public Enums.SelectionProperty  selectionProperty   { get; set; }
    public Enums.SelectionType      selectionType       { get; set; }

    public List<int>        id_list = new List<int>();
    public int              selected_id;

    public ListData         listData        { get; set; }
    public PathManager      pathManager     { get; set; }
    public SelectionGroup   selectionGroup  { get; set; }

    public OverlayManager   overlayManager;
    public ActionManager    actionManager;

    public RectTransform    list_area       { get; set; }
    public RectTransform    main_list,
                            list_parent;

    public Vector2          list_size       { get; set; }
    public float            base_size       { get; set; }

    //private bool always_on;

    public void InitializeList(ListData new_listData)
    {
        listData = new_listData;

        id_list = new List<int>(listData.id_list);

        switch(listData.sort_type)
        {
            case Enums.SortType.Panel: organizer = gameObject.AddComponent<PanelOrganizer>();   break;
            case Enums.SortType.List:  organizer = gameObject.AddComponent<ListOrganizer>();    break;
            case Enums.SortType.Grid:  organizer = gameObject.AddComponent<GridOrganizer>();    break;
            default: break;
        }

        organizer.InitializeOrganizer();

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
        //always_on = listProperties.always_on;
 
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
            AutoSelectElement();
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

    public void AutoSelectElement()
    {
        SelectionElement element = organizer.GetElement(0);

        element.OpenPath(element.data.path);
    }

    public void CloseList()
    {
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
                InitializeElement(element, index);
                return element;
            }     
        }

        SelectionElement new_element = Instantiate(element_prefab);

        InitializeElement(new_element, index);

        list.Add(new_element);

        return new_element;
    }

    public void InitializeElement(SelectionElement element, int index)
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
