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

    public Vector2          listSize        { get; set; }
    public RectTransform    ParentRect      { get { return transform.parent.GetComponent<RectTransform>(); } }

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

        overlayManager.SetOverlayProperties(listProperties);

        transform.parent.gameObject.SetActive(true);
    }

    public void SelectData()
    {
        organizer.SelectData();
    }

    public void SetList()
    {
        if (organizer == null) return;

        var dataList = Display.DataController.DataList;

        if (dataList.Count == 0) return;
        
        overlayManager.ActivateOverlay(organizer, List);

        overlayManager.SetOverlaySize();

        listParent.sizeDelta = List.GetListSize(dataList.Count, true);

        listSize = List.GetListSize(dataList.Count, false);

        //Select data after the list has been resized, so that the position may be properly corrected
        organizer.SelectData();
        
        if (!listProperties.enablePaging)
            SetData();

        overlayManager.SetOverlay();

        if (!Display.DataController.SegmentController.Loaded)
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

    public void CorrectPosition(IDataElement dataElement)
    {
        if (!listProperties.enablePositionCorrection) return;

        //To offset jittering when selecting edge elements
        float positionOffset = 0.999f;

        var elementPosition = List.GetElementPosition(dataElement.Index);

        var localElementPosition = new Vector2(elementPosition.x + listParent.localPosition.x,
                                               elementPosition.y + listParent.localPosition.y) * positionOffset;

        var elementMin = new Vector2(localElementPosition.x - List.ElementSize.x / 2,
                                     localElementPosition.y - List.ElementSize.y / 2);

        var elementMax = new Vector2(localElementPosition.x + List.ElementSize.x / 2,
                                     localElementPosition.y + List.ElementSize.y / 2);

        if (elementMax.x > RectTransform.rect.max.x ||
            elementMin.x < RectTransform.rect.min.x ||
            elementMax.y > RectTransform.rect.max.y ||
            elementMin.y < RectTransform.rect.min.y)
        {
            //if(elementMax.x > RectTransform.rect.max.x)
            //    Debug.Log("1:CORRECT POSITION");
            //if (elementMin.x < RectTransform.rect.min.x)
            //    Debug.Log("2:CORRECT POSITION");
            //if (elementMax.y > RectTransform.rect.max.y)
            //    Debug.Log("3:CORRECT POSITION");
            //if (elementMin.y < RectTransform.rect.min.y)
            //    Debug.Log("4:CORRECT POSITION");

            ScrollRect.horizontalNormalizedPosition = ((elementPosition.x - List.ElementSize.x / 2) + listParent.rect.width / 2) / ((listParent.rect.width - List.ElementSize.x) / 2) / 2;
            ScrollRect.verticalNormalizedPosition   = (elementPosition.y + ((listParent.sizeDelta.y - List.ElementSize.y) / 2)) / (listParent.sizeDelta.y - List.ElementSize.y);
        }
    }

    public void AutoSelectElement()
    {
        if (organizer == null) return;
        
        if (Display.DataController.DataList.Count == 0) return;
        
        if (Display.SelectionType == SelectionManager.Type.Automatic)
        {
            SelectionElement element = List.ElementList.FirstOrDefault();

            element.InvokeSelection();
        }
    }

    public void CloseList()
    {
        if (organizer == null) return;

        ScrollRect.horizontal = false;
        ScrollRect.vertical = false;
        
        overlayManager.CloseOverlay();
        organizer.CloseOrganizer();
        
        transform.parent.gameObject.SetActive(false);
    }

    public void CloseOrganizer() { }
}
