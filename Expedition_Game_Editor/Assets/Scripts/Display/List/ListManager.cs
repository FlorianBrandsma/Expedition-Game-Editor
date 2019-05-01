﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class ListManager : MonoBehaviour
{
    public IOrganizer organizer { get; set; }
    private IList list { get { return GetComponent<IList>(); } }

    public SelectionManager.Property  selectionProperty   { get; set; }
    public SelectionManager.Type      selectionType       { get; set; }

    public ListProperties   listProperties  { get; set; }
    public PathManager      pathManager     { get; set; }

    public OverlayManager   overlayManager;

    public ScrollRect       scrollRect      { get { return GetComponent<ScrollRect>(); } }
    public RectTransform    rectTransform   { get { return GetComponent<RectTransform>(); } }
    public RectTransform    listParent;

    private Vector3         listMin, 
                            listMax;

    public Vector2          listSize       { get; set; }

    public List<SelectionElement> elementList = new List<SelectionElement>();
    public SelectionElement selectedElement { get; set; }

    public Route selectedRoute { get; set; }

    public void InitializeList(ListProperties listProperties)
    {
        this.listProperties = listProperties;

        switch(listProperties.displayType)
        {
            case DisplayManager.Type.None:      organizer = null;                                           break;
            case DisplayManager.Type.Button:    organizer = gameObject.AddComponent<ButtonOrganizer>();     break;
            case DisplayManager.Type.Tile:      organizer = gameObject.AddComponent<TileOrganizer>();       break;
            case DisplayManager.Type.Panel:     organizer = gameObject.AddComponent<PanelOrganizer>();      break;
            case DisplayManager.Type.PanelTile: organizer = gameObject.AddComponent<PanelTileOrganizer>();  break;
            default:                                                                                        break;
        }

        if (organizer == null) return;

        organizer.InitializeOrganizer();

        overlayManager.InitializeOverlay(this);

        SelectionManager.lists.Add(this);
    }

    public void SetProperties()
    {
        if (organizer == null) return;

        scrollRect.horizontal   = listProperties.horizontal;
        scrollRect.vertical     = listProperties.vertical;

        selectionProperty       = listProperties.selectionProperty;
        selectionType           = listProperties.selectionType;

        organizer.SetProperties();

        overlayManager.SetOverlayProperties(listProperties);

        transform.parent.gameObject.SetActive(true);
    }

    public void SetList()
    {
        if (organizer == null) return;

        if (listProperties.SegmentController.DataController.DataList.Count == 0) return;

        list.SetElementSize();

        overlayManager.ActivateOverlay(organizer, list);

        overlayManager.SetOverlaySize();

        listParent.sizeDelta = list.GetListSize(listProperties.SegmentController.DataController.DataList.Count, true);

        listSize = list.GetListSize(listProperties.SegmentController.DataController.DataList.Count, false);

        if (!listProperties.enablePaging)
            SetData();

        overlayManager.SetOverlay();

        listMin = rectTransform.TransformPoint(new Vector2(rectTransform.rect.min.x, rectTransform.rect.min.y));
        listMax = rectTransform.TransformPoint(new Vector2(rectTransform.rect.max.x, rectTransform.rect.max.y));

        //Debug.Log(EditorManager.historyManager.returned);

        if (EditorManager.historyManager.returned || !listProperties.SegmentController.editorController.pathController.loaded)
            ResetListPosition();
    }

    private void ResetListPosition()
    {
        scrollRect.verticalNormalizedPosition = 1f;
        scrollRect.horizontalNormalizedPosition = 0f;
    }

    public void UpdateData()
    {
        if (!gameObject.activeInHierarchy) return;

        organizer.UpdateData();
    }

    private void SetData()
    {
        organizer.SetData();
    }

    public void ResetData()
    {
        if (organizer == null) return;

        organizer.ResetData(listProperties.SegmentController.DataController.DataList);
    }

    public void UpdateOverlay()
    {
        if (organizer == null) return;

        overlayManager.UpdateOverlay();
    }

    public void ResetSelection()
    {
        SelectElement(selectedRoute);
    }

    public void SelectElement(Route route)
    {
        if (selectedElement != null) return;

        if (selectionProperty == SelectionManager.Property.Set) return;

        foreach (SelectionElement element in elementList)
        {
            //Check if element has child first (and that child is active)
            //If child data matches route data, check if property matches in case parent and child have same data
            if (element.child != null && element.child.gameObject.activeInHierarchy && 
                element.child.GeneralData().Equals(route.GeneralData()))
            {
                if (element.child.route.property == route.property)
                {
                    selectedElement = element.child;

                    element.child.ActivateSelection();

                    selectedRoute = route.Copy();

                    CorrectPosition(element);

                    return;
                }  
            }

            //Either child didn't exist or have matching property
            //All that's left is for main element data to match the route data
            if (element.GeneralData().Equals(route.GeneralData()))
            {
                selectedElement = element;

                element.ActivateSelection();

                selectedRoute = route.Copy();

                CorrectPosition(element);

                return;
            }
        }
    }

    private void CorrectPosition(SelectionElement element)
    {
        if (element.transform.position.x > listMax.x ||
            element.transform.position.x < listMin.x ||
            element.transform.position.z > listMax.z ||
            element.transform.position.z < listMin.z)
        {
            scrollRect.horizontalNormalizedPosition = ((element.transform.localPosition.x - list.ElementSize.x / 2) + listParent.rect.width / 2) / ((listParent.rect.width - list.ElementSize.x) / 2) / 2;
            scrollRect.verticalNormalizedPosition   = (element.transform.localPosition.y + ((listParent.sizeDelta.y - list.ElementSize.y) / 2)) / (listParent.sizeDelta.y - list.ElementSize.y);
        }
    }

    public void CancelSelection(Route route)
    {
        foreach (SelectionElement element in elementList)
        {
            if (element.GeneralData().Equals(route.GeneralData()))
            {
                if (element.child != null && element.child.route.property == route.property)
                    element.child.CancelSelection();
                else
                    element.CancelSelection();

                selectedElement = null;

                return;
            }
        }
    }

    public void AutoSelectElement()
    {
        if (organizer == null) return;

        if (selectionType == SelectionManager.Type.Automatic)
        {
            SelectionElement element = list.GetElement(0);

            element.GetComponent<Button>().onClick.Invoke();
        }
    }

    public void CloseList()
    {
        if (organizer == null) return;

        scrollRect.horizontal = false;
        scrollRect.vertical = false;

        overlayManager.CloseOverlay();
        organizer.CloseOrganizer();

        selectedElement = null;

        elementList.Clear();

        SelectionManager.lists.RemoveAt(SelectionManager.lists.IndexOf(this));

        transform.parent.gameObject.SetActive(false);
    }

    public void CloseOrganizer()
    {
        
    }

    public SelectionElement SpawnElement(List<SelectionElement> list, SelectionElement elementPrefab, Enums.ElementType elementType)
    {
        foreach(SelectionElement element in list)
        {
            if (!element.gameObject.activeInHierarchy && element.elementType == elementType)
            {
                InitializeElement(element);
                return element;
            }     
        }

        SelectionElement newElement = Instantiate(elementPrefab);
        newElement.elementType = elementType;

        InitializeElement(newElement);

        list.Add(newElement);

        return newElement;
    }

    public void InitializeElement(SelectionElement element)
    {
        element.InitializeElement(this, selectionProperty);

        element.transform.SetParent(listParent, false);
    }

    public void ResetElement()
    {
        foreach(SelectionElement element in elementList)
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

        elementList.Clear();
    }
}
