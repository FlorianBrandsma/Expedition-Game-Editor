using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class ListManager : MonoBehaviour, IDisplayManager
{
    public IOrganizer organizer;

    public IDisplay         Display { get; set; }
    private ListProperties  listProperties;

    public OverlayManager   overlayManager;

    public ScrollRect       ScrollRect      { get { return GetComponent<ScrollRect>(); } }
    public RectTransform    RectTransform   { get { return GetComponent<RectTransform>(); } }
    public RectTransform    listParent;

    private Vector3         listMin, 
                            listMax;

    public Vector2          listSize        { get; set; }
    public RectTransform    ParentRect      { get { return transform.parent.GetComponent<RectTransform>(); } }

    public List<SelectionElement> elementList = new List<SelectionElement>();
    public SelectionElement SelectedElement { get; set; }

    public Route selectedRoute { get; set; }

    public SegmentController segmentController;

    private IList List { get { return GetComponent<IList>(); } }

    public void InitializeList(ListProperties listProperties)
    {
        if (GetComponent<IOrganizer>() != null) return;

        Display = listProperties;
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

        ScrollRect.horizontal   = listProperties.horizontal;
        ScrollRect.vertical     = listProperties.vertical;

        organizer.InitializeProperties();

        overlayManager.SetOverlayProperties(listProperties);

        transform.parent.gameObject.SetActive(true);
    }

    public void SetList()
    {
        if (organizer == null) return;

        var dataList = Display.DataController.DataList;

        if (dataList.Count == 0) return;

        SelectionElementManager.dataElementPool = SelectionElementManager.dataElementPool.Concat(dataList).ToList();

        List.SetElementSize();

        overlayManager.ActivateOverlay(organizer, List);

        overlayManager.SetOverlaySize();

        listParent.sizeDelta = List.GetListSize(dataList.Count, true);

        listSize = List.GetListSize(dataList.Count, false);

        if (!listProperties.enablePaging)
            SetData();

        overlayManager.SetOverlay();
        
        listMin = RectTransform.TransformPoint(new Vector2(RectTransform.rect.min.x, RectTransform.rect.min.y));
        listMax = RectTransform.TransformPoint(new Vector2(RectTransform.rect.max.x, RectTransform.rect.max.y));
        
        if (EditorManager.loadType == Enums.LoadType.Reload || !Display.DataController.SegmentController.Loaded)
            ResetListPosition();
    }

    private void ResetListPosition()
    {
        ScrollRect.verticalNormalizedPosition = 1f;
        ScrollRect.horizontalNormalizedPosition = 0f;
    }

    public void UpdateData()
    {
        organizer.UpdateData();
    }

    private void SetData()
    {
        organizer.SetData();
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
            ScrollRect.horizontalNormalizedPosition = ((element.transform.localPosition.x - List.ElementSize.x / 2) + listParent.rect.width / 2) / ((listParent.rect.width - List.ElementSize.x) / 2) / 2;
            ScrollRect.verticalNormalizedPosition = (element.transform.localPosition.y + ((listParent.sizeDelta.y - List.ElementSize.y) / 2)) / (listParent.sizeDelta.y - List.ElementSize.y);
        }
    }

    public void AutoSelectElement()
    {
        if (organizer == null) return;
        
        if (Display.DataController.DataList.Count == 0) return;
        
        if (Display.SelectionType == SelectionManager.Type.Automatic)
        {
            SelectionElement element = List.ElementList.FirstOrDefault();

            element.GetComponent<Button>().onClick.Invoke();
        }
    }

    public void CloseList()
    {
        if (organizer == null) return;

        Display.DataController.DataList.ForEach(x =>
        {
            SelectionElementManager.dataElementPool.RemoveAll(y => ((GeneralData)x).Equals((GeneralData)y));
        });

        ScrollRect.horizontal = false;
        ScrollRect.vertical = false;

        overlayManager.CloseOverlay();
        organizer.CloseOrganizer();
        
        elementList.Clear();

        if (SelectedElement != null)
            SelectedElement.CancelSelection();

        transform.parent.gameObject.SetActive(false);
    }

    public void CloseOrganizer() { }
}
