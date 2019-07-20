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
    public RectTransform    listParent;

    private Vector3         listMin, 
                            listMax;

    public Vector2          listSize        { get; set; }
    public RectTransform    ParentRect      { get { return transform.parent.GetComponent<RectTransform>(); } }

    public List<SelectionElement> elementList = new List<SelectionElement>();
    public SelectionElement selectedElement { get; set; }

    public Route selectedRoute { get; set; }

    public SegmentController segmentController;

    public void InitializeList(ListProperties listProperties)
    {
        if (GetComponent<IOrganizer>() != null) return;
        
        this.listProperties = listProperties;
        
        switch(listProperties.displayType)
        {
            case DisplayManager.Type.None:      organizer = null;                                           break;
            case DisplayManager.Type.Button:    organizer = gameObject.AddComponent<ButtonOrganizer>();     break;
            case DisplayManager.Type.Tile:      organizer = gameObject.AddComponent<TileOrganizer>();       break;
            case DisplayManager.Type.Panel:     organizer = gameObject.AddComponent<PanelOrganizer>();      break;
            case DisplayManager.Type.PanelTile: organizer = gameObject.AddComponent<PanelTileOrganizer>();  break;
            case DisplayManager.Type.MultiGrid: organizer = gameObject.AddComponent<MultiGridOrganizer>();  break;
            default: Debug.Log("CASE MISSING: " + listProperties.displayType);                              break;
        }

        if (organizer == null) return;

        organizer.InitializeOrganizer();

        overlayManager.InitializeOverlay(this);
    }

    public void SetProperties()
    {
        if (organizer == null) return;

        scrollRect.horizontal   = listProperties.horizontal;
        scrollRect.vertical     = listProperties.vertical;

        selectionProperty       = listProperties.selectionProperty;
        selectionType           = listProperties.selectionType;

        organizer.InitializeProperties();

        overlayManager.SetOverlayProperties(listProperties);

        transform.parent.gameObject.SetActive(true);
    }

    public void SetList()
    {
        if (organizer == null) return;

        if (listProperties.DataController.DataList.Count == 0) return;

        list.SetElementSize();

        overlayManager.ActivateOverlay(organizer, list);

        overlayManager.SetOverlaySize();

        listParent.sizeDelta = list.GetListSize(listProperties.DataController.DataList.Count, true);

        listSize = list.GetListSize(listProperties.DataController.DataList.Count, false);

        if (!listProperties.enablePaging)
            SetData();

        overlayManager.SetOverlay();
        
        listMin = rectTransform.TransformPoint(new Vector2(rectTransform.rect.min.x, rectTransform.rect.min.y));
        listMax = rectTransform.TransformPoint(new Vector2(rectTransform.rect.max.x, rectTransform.rect.max.y));

        if (EditorManager.historyManager.returned || !listProperties.DataController.SegmentController.editorController.pathController.loaded)
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

        organizer.ResetData(listProperties.DataController.DataList);
    }

    public void UpdateOverlay()
    {
        if (organizer == null) return;

        overlayManager.UpdateOverlay();
    }

    public void CorrectPosition(SelectionElement element)
    {
        if (!listProperties.enablePositionCorrection) return;

        //To offset jittering when selecting edge elements
        float positionOffset = 0.999f;

        Vector3 inverseElementPoint = transform.InverseTransformPoint(element.transform.position) * positionOffset;

        Vector3 intermediateMin = new Vector3(inverseElementPoint.x - element.RectTransform.rect.max.x, 
                                              inverseElementPoint.y - element.RectTransform.rect.max.y, 
                                              inverseElementPoint.z);

        Vector3 intermediateMax = new Vector3(inverseElementPoint.x + element.RectTransform.rect.max.x, 
                                              inverseElementPoint.y + element.RectTransform.rect.max.y, 
                                              inverseElementPoint.z);

        Vector3 elementMin = transform.TransformPoint(intermediateMin);
        Vector3 elementMax = transform.TransformPoint(intermediateMax);

        if (elementMax.x > listMax.x ||
            elementMin.x < listMin.x ||
            elementMax.z > listMax.z ||
            elementMin.z < listMin.z)
        {
            scrollRect.horizontalNormalizedPosition = ((element.transform.localPosition.x - list.ElementSize.x / 2) + listParent.rect.width / 2) / ((listParent.rect.width - list.ElementSize.x) / 2) / 2;
            scrollRect.verticalNormalizedPosition = (element.transform.localPosition.y + ((listParent.sizeDelta.y - list.ElementSize.y) / 2)) / (listParent.sizeDelta.y - list.ElementSize.y);
        }
    }

    public void AutoSelectElement()
    {
        if (organizer == null) return;

        if (listProperties.DataController.DataList.Count == 0) return;
        
        if (selectionType == SelectionManager.Type.Automatic)
        {
            SelectionElement element = list.ElementList.FirstOrDefault();

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
        
        elementList.Clear();

        if (selectedElement != null)
            selectedElement.CancelSelection();

        transform.parent.gameObject.SetActive(false);
    }

    public void CloseOrganizer()
    {
        
    }
}
