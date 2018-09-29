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

    public SelectionManager.Property  selectionProperty   { get; set; }
    public SelectionManager.Type      selectionType       { get; set; }

    public int              selected_id;

    public ListData         listData        { get; set; }
    public PathManager      pathManager     { get; set; }

    public OverlayManager   overlayManager;
    public ActionManager    actionManager;

    public RectTransform    list_area       { get; set; }
    public RectTransform    main_list,
                            list_parent;

    public Vector2          list_size       { get; set; }
    public float            base_size       { get; set; }

    public List<SelectionElement> element_list = new List<SelectionElement>();

    //private bool always_on;

    public void InitializeList(ListData new_listData)
    {
        listData = new_listData;

        switch(listData.listProperties.listType)
        {
            case ListProperties.Type.None:  organizer = null;                                       break;
            case ListProperties.Type.List:  organizer = gameObject.AddComponent<ListOrganizer>();   break;
            case ListProperties.Type.Grid:  organizer = gameObject.AddComponent<GridOrganizer>();   break;
            case ListProperties.Type.Panel: organizer = gameObject.AddComponent<PanelOrganizer>();  break;
            default:                                                                                break;
        }

        if (organizer == null) return;

        organizer.InitializeOrganizer();

        overlayManager.InitializeOverlay(this);  
    }

    public void SetProperties(ListProperties listProperties)
    {
        if (organizer == null) return;

        controller = listProperties.controller;

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
        if (organizer == null) return;

        organizer.SetListSize(base_size);

        overlayManager.ActivateOverlay(organizer);

        overlayManager.SetOverlaySize();

        list_parent.sizeDelta = organizer.GetListSize(listData.list, true);

        list_size = organizer.GetListSize(listData.list, false);

        main_list.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
        main_list.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0f;

        SetRows();

        overlayManager.SetOverlay();   
    }

    public void SetRows()
    {
        if (organizer == null) return;

        organizer.SetRows(listData.list);
    }

    public void ResetRows()
    {
        if (organizer == null) return;

        organizer.ResetRows(listData.list);
    }

    public void UpdateRows()
    {
        if (organizer == null) return;

        overlayManager.UpdateOverlay();
    }

    public void GetIndex(SelectionElement element)
    {
        //Debug.Log(element_list.IndexOf(element));

        //Set list position based on index + element size
    }

    public void SelectElement(Route route)
    {
        foreach(SelectionElement element in element_list)
        {
            if (element.data.Equals(route.data))
            {
                if (element.child != null && element.child.selectionProperty == route.origin.selectionProperty)
                    element.child.SelectElement();
                else
                    element.SelectElement();

                break;
            }                    
        }
    }

    public void CancelSelection(Route route)
    {
        foreach (SelectionElement element in element_list)
        {
            if (element.data.Equals(route.data))
            {
                if (element.child != null && element.child.selectionProperty == route.origin.selectionProperty)
                    element.child.CancelSelection();
                else
                    element.CancelSelection();

                break;
            }
        }
    }

    public void AutoSelectElement()
    {
        if (organizer == null) return;

        if (selectionType == SelectionManager.Type.Automatic)
        {
            if (!controller.loaded)
            {
                SelectionElement element = organizer.GetElement(0);

                element.GetComponent<Button>().onClick.Invoke();
            }
        }
    }

    public void CloseList()
    {
        if (organizer == null) return;

        main_list.GetComponent<ScrollRect>().horizontal = false;
        main_list.GetComponent<ScrollRect>().vertical = false;

        overlayManager.CloseOverlay();

        organizer.CloseList();

        element_list.Clear();
    }

    public SelectionElement SpawnElement(List<SelectionElement> list, SelectionElement element_prefab, ElementData data)
    {
        foreach(SelectionElement element in list)
        {
            if (!element.gameObject.activeInHierarchy)
            {
                InitializeElement(element, data);
                return element;
            }     
        }

        SelectionElement new_element = Instantiate(element_prefab);

        InitializeElement(new_element, data);

        list.Add(new_element);

        return new_element;
    }

    public void InitializeElement(SelectionElement element, ElementData data)
    {
        element.InitializeSelection(this, data, selectionProperty);

        if (element.child != null)
            element.child.InitializeSelection(this, data, SelectionManager.Property.Edit);

        element.transform.SetParent(list_parent, false);
    }

    public void ResetElement(List<SelectionElement> list)
    {
        foreach(SelectionElement element in list)
        {
            element.gameObject.SetActive(false);
            element.GetComponent<Button>().onClick.RemoveAllListeners();

            if (element.child != null)
                element.child.GetComponent<Button>().onClick.RemoveAllListeners();
        }
    }
}
