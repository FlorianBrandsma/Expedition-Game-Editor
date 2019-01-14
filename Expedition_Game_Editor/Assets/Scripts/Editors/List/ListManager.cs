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

    public ListData         listData        { get; set; }
    public PathManager      pathManager     { get; set; }

    public OverlayManager   overlayManager;

    private ScrollRect      scrollRect;
    public RectTransform    rectTransform { get; set; }

    public RectTransform    list_parent;

    private Vector3         list_min, 
                            list_max;

    public Vector2          list_size       { get; set; }

    public List<SelectionElement> element_list = new List<SelectionElement>();
    public SelectionElement selected_element { get; set; }

    //private bool always_on;

    private void Awake()
    {
        scrollRect      = GetComponent<ScrollRect>();
        rectTransform   = GetComponent<RectTransform>();
    }

    public void InitializeList(ListData new_listData)
    {
        listData = new_listData;

        switch(listData.listProperties.listType)
        {
            case ListProperties.Type.None:      organizer = null;                                           break;
            case ListProperties.Type.Button:    organizer = gameObject.AddComponent<ButtonOrganizer>();     break;
            case ListProperties.Type.Tile:      organizer = gameObject.AddComponent<TileOrganizer>();       break;
            case ListProperties.Type.Panel:     organizer = gameObject.AddComponent<PanelOrganizer>();      break;
            case ListProperties.Type.PanelTile: organizer = gameObject.AddComponent<PanelTileOrganizer>();  break;
            default:                                                                                        break;
        }

        if (organizer == null) return;

        organizer.InitializeOrganizer();

        overlayManager.InitializeOverlay(this);

        SelectionManager.lists.Add(this);
    }

    public void SetProperties(ListProperties listProperties)
    {
        if (organizer == null) return;

        controller = listProperties.controller;

        scrollRect.horizontal = listProperties.horizontal;
        scrollRect.vertical   = listProperties.vertical;

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

        organizer.SetElementSize();

        overlayManager.ActivateOverlay(organizer);

        overlayManager.SetOverlaySize();

        list_parent.sizeDelta = organizer.GetListSize(listData.list, true);

        list_size = organizer.GetListSize(listData.list, false);

        SetRows();

        overlayManager.SetOverlay();

        list_min = rectTransform.TransformPoint(new Vector2(rectTransform.rect.min.x, rectTransform.rect.min.y));
        list_max = rectTransform.TransformPoint(new Vector2(rectTransform.rect.max.x, rectTransform.rect.max.y));

        if (!listData.controller.loaded)
            ResetListPosition();   
    }

    public void ResetListPosition()
    {
        scrollRect.verticalNormalizedPosition = 1f;
        scrollRect.horizontalNormalizedPosition = 0f;
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
            //Check if element has child first
            //If child data matches route data, check if property matches in case parent and child have same data
            if (element.child != null && element.child.data.Equals(route.data))
            {
                if (element.child.selectionProperty == route.origin.selectionProperty)
                {
                    element.child.ActivateSelection();

                    CorrectPosition(element);

                    return;
                }  
            }

            //Either child didn't exist or have matching property
            //All that's left is for main element data to match the route data
            if (element.data.Equals(route.data))
            {
                element.ActivateSelection();

                CorrectPosition(element);

                return;
            }
        }
    }

    public void CorrectPosition(SelectionElement element)
    {
        if (element.transform.position.x > list_max.x ||
            element.transform.position.x < list_min.x ||
            element.transform.position.z > list_max.z ||
            element.transform.position.z < list_min.z)
        {
            scrollRect.horizontalNormalizedPosition = (element.transform.localPosition.x + list_parent.sizeDelta.x) / (list_parent.sizeDelta.x * 2);
            scrollRect.verticalNormalizedPosition = (element.transform.localPosition.y + ((list_parent.sizeDelta.y - organizer.element_size.y) / 2)) / (list_parent.sizeDelta.y - (organizer.element_size.y));
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

        scrollRect.horizontal = false;
        scrollRect.vertical = false;

        overlayManager.CloseOverlay();

        organizer.CloseList();

        element_list.Clear();

        SelectionManager.lists.RemoveAt(SelectionManager.lists.IndexOf(this));
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

        element.transform.SetParent(list_parent, false);
    }

    public void ResetElement(List<SelectionElement> list)
    {
        foreach(SelectionElement element in list)
        {
            element.GetComponent<IElement>().CloseElement();

            element.gameObject.SetActive(false);
            element.GetComponent<Button>().onClick.RemoveAllListeners();

            if (element.child != null)
            {
                element.child.gameObject.SetActive(false);
                element.child.GetComponent<Button>().onClick.RemoveAllListeners();
            }      
        }
    }
}
