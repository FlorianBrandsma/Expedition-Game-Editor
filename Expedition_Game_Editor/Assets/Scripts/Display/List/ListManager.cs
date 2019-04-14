using UnityEngine;
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

    public RectTransform    list_parent;

    private Vector3         list_min, 
                            list_max;

    public Vector2          list_size       { get; set; }

    public List<SelectionElement> element_list = new List<SelectionElement>();
    public SelectionElement selected_element { get; set; }

    public Route selected_route { get; set; }

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

    public void SetListSize()
    {
        if (organizer == null) return;

        if (listProperties.dataController.data_list.Count == 0) return;

        list.SetElementSize();

        overlayManager.ActivateOverlay(organizer, list);

        overlayManager.SetOverlaySize();

        list_parent.sizeDelta = list.GetListSize(listProperties.dataController.data_list.Count, true);

        list_size = list.GetListSize(listProperties.dataController.data_list.Count, false);

        SetData();

        overlayManager.SetOverlay();

        list_min = rectTransform.TransformPoint(new Vector2(rectTransform.rect.min.x, rectTransform.rect.min.y));
        list_max = rectTransform.TransformPoint(new Vector2(rectTransform.rect.max.x, rectTransform.rect.max.y));

        if (EditorManager.historyManager.returned || !listProperties.dataController.segmentController.editorController.pathController.loaded)
            ResetListPosition();
    }

    private void ResetListPosition()
    {
        scrollRect.verticalNormalizedPosition = 1f;
        scrollRect.horizontalNormalizedPosition = 0f;
    }

    public void UpdateData()
    {
        organizer.UpdateData();
    }

    private void SetData()
    {
        organizer.SetData();
    }

    public void ResetData()
    {
        if (organizer == null) return;

        organizer.ResetData(listProperties.dataController.data_list);
    }

    public void UpdateOverlay()
    {
        if (organizer == null) return;

        overlayManager.UpdateOverlay();
    }

    public void ResetSelection()
    {
        SelectElement(selected_route);
    }

    public void SelectElement(Route route)
    {
        if (selected_element != null) return;

        if (selectionProperty == SelectionManager.Property.Set) return;

        foreach (SelectionElement element in element_list)
        {
            //Check if element has child first
            //If child data matches route data, check if property matches in case parent and child have same data
            if (element.child != null && element.child.GeneralData().Equals(route.GeneralData()))
            {
                if (element.child.selectionProperty == route.property)
                {
                    selected_element = element.child;

                    element.child.ActivateSelection();

                    selected_route = route.Copy();

                    CorrectPosition(element);

                    return;
                }  
            }

            //Either child didn't exist or have matching property
            //All that's left is for main element data to match the route data
            if (element.GeneralData().Equals(route.GeneralData()))
            {
                selected_element = element;

                element.ActivateSelection();

                CorrectPosition(element);

                return;
            }
        }
    }

    private void CorrectPosition(SelectionElement element)
    {
        if (element.transform.position.x > list_max.x ||
            element.transform.position.x < list_min.x ||
            element.transform.position.z > list_max.z ||
            element.transform.position.z < list_min.z)
        {
            scrollRect.horizontalNormalizedPosition = (element.transform.localPosition.x + list_parent.sizeDelta.x) / (list_parent.sizeDelta.x * 2);
            scrollRect.verticalNormalizedPosition   = (element.transform.localPosition.y + ((list_parent.sizeDelta.y - list.element_size.y) / 2)) / (list_parent.sizeDelta.y - (list.element_size.y));
        }
    }

    public void CancelSelection(Route route)
    {
        foreach (SelectionElement element in element_list)
        {
            if (element.GeneralData().Equals(route.GeneralData()))
            {
                if (element.child != null && element.child.selectionProperty == route.property)
                    element.child.CancelSelection();
                else
                    element.CancelSelection();

                selected_element = null;

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

        selected_element = null;

        element_list.Clear();

        SelectionManager.lists.RemoveAt(SelectionManager.lists.IndexOf(this));

        transform.parent.gameObject.SetActive(false);
    }

    public void CloseOrganizer()
    {
        
    }

    public SelectionElement SpawnElement(List<SelectionElement> list, SelectionElement element_prefab)
    {
        foreach(SelectionElement element in list)
        {
            if (!element.gameObject.activeInHierarchy)
            {
                InitializeElement(element);
                return element;
            }     
        }

        SelectionElement new_element = Instantiate(element_prefab);

        InitializeElement(new_element);

        list.Add(new_element);

        return new_element;
    }

    public void InitializeElement(SelectionElement element)
    {
        element.InitializeElement(this, selectionProperty);

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
