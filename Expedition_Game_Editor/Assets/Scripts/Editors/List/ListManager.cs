using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ListManager : MonoBehaviour
{
    private IController controller;
    private IOrganizer  organizer;

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

    private Vector3         list_min, 
                            list_max;

    public Vector2          list_size       { get; set; }
    public float            base_size       { get; set; }

    public List<SelectionElement> element_list = new List<SelectionElement>();
    public SelectionElement selected_element { get; set; }

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
        main_list.GetComponent<ScrollRect>().vertical   = listProperties.vertical;

        selectionProperty = listProperties.selectionProperty;
        selectionType = listProperties.selectionType;
        //always_on = listProperties.always_on;

        organizer.SetProperties(listProperties);

        overlayManager.SetOverlayProperties(listProperties);   
    }

    public void SetListSize()
    {
        if (listData.list.Count == 0) return;

        if (organizer == null) return;

        organizer.SetListSize();

        overlayManager.ActivateOverlay(organizer);

        overlayManager.SetOverlaySize();

        list_parent.sizeDelta = organizer.GetListSize(listData.list, true);

        list_size = organizer.GetListSize(listData.list, false);

        SetRows();

        overlayManager.SetOverlay();

        list_min = main_list.TransformPoint(new Vector2(main_list.rect.min.x, main_list.rect.min.y));
        list_max = main_list.TransformPoint(new Vector2(main_list.rect.max.x, main_list.rect.max.y));

        if (!listData.controller.loaded)
            ResetListPosition();   
    }

    public void ResetListPosition()
    {
        main_list.GetComponent<ScrollRect>().verticalNormalizedPosition = 1f;
        main_list.GetComponent<ScrollRect>().horizontalNormalizedPosition = 0f;
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

    public void SelectElement(Route route)
    {
        if (selectionProperty == SelectionManager.Property.Set) return;

        foreach(SelectionElement element in element_list)
        {
            if (element.data.Equals(route.data))
            {
                if (element.child != null && element.child.selectionProperty == route.origin.selectionProperty)
                    element.child.ActivateSelection();
                else
                    element.ActivateSelection();

                if (element.transform.position.x > list_max.x ||
                    element.transform.position.x < list_min.x ||
                    element.transform.position.z > list_max.z ||
                    element.transform.position.z < list_min.z)
                    CorrectPosition(element);

                return;
            }                    
        }
    }

    public void CorrectPosition(SelectionElement element)
    {
        main_list.GetComponent<ScrollRect>().horizontalNormalizedPosition = (element.transform.localPosition.x + list_parent.sizeDelta.x) / (list_parent.sizeDelta.x * 2);
        main_list.GetComponent<ScrollRect>().verticalNormalizedPosition = (element.transform.localPosition.y + ((list_parent.sizeDelta.y - organizer.element_size) / 2)) / (list_parent.sizeDelta.y - (organizer.element_size));
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

                return;
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
