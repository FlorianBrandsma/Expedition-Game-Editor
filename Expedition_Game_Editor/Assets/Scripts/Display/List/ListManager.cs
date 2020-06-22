using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class ListManager : MonoBehaviour, IDisplayManager
{
    public IOrganizer       Organizer       { get; set; }
    public IDisplay         Display         { get; set; }

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
        
        switch(listProperties.elementType)
        {
            case DisplayManager.OrganizerType.None:      Organizer = null;                                           break;
            case DisplayManager.OrganizerType.Button:    Organizer = gameObject.AddComponent<ButtonOrganizer>();     break;
            case DisplayManager.OrganizerType.Tile:      Organizer = gameObject.AddComponent<TileOrganizer>();       break;
            case DisplayManager.OrganizerType.Panel:     Organizer = gameObject.AddComponent<PanelOrganizer>();      break;
            case DisplayManager.OrganizerType.PanelTile: Organizer = gameObject.AddComponent<PanelTileOrganizer>();  break;
            case DisplayManager.OrganizerType.MultiGrid: Organizer = gameObject.AddComponent<MultiGridOrganizer>();  break;

            default: Debug.Log("CASE MISSING: " + listProperties.elementType);                              break;
        }

        if (Organizer == null) return;

        Organizer.InitializeOrganizer();

        overlayManager.InitializeOverlay(this);
    }

    public void SetProperties()
    {
        if (Organizer == null) return;

        ScrollRect.horizontal   = listProperties.horizontal;
        ScrollRect.vertical     = listProperties.vertical;

        overlayManager.SetOverlayProperties(listProperties);

        transform.parent.gameObject.SetActive(true);
    }

    public void SetList()
    {
        if (Organizer == null) return;

        var dataList = Display.DataController.DataList;

        if (dataList.Count == 0) return;
        
        overlayManager.ActivateOverlay(Organizer);

        overlayManager.SetOverlaySize();

        listParent.sizeDelta = List.GetListSize(dataList.Count, true);

        listSize = List.GetListSize(dataList.Count, false);

        //Select data after the list has been resized, so that the position may be properly corrected
        //"Set" elements never receive this kind of visual feedback
        if(listProperties.SelectionProperty != SelectionManager.Property.Set)
            Organizer.SelectData();
        
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
        Organizer.UpdateData();
    }

    private void SetData()
    {
        Organizer.SetData();
    }

    public void UpdateOverlay()
    {
        if (Organizer == null) return;

        overlayManager.UpdateOverlay();
    }

    public void CorrectPosition(IElementData elementData)
    {
        if (!listProperties.enablePositionCorrection) return;

        //To offset jittering when selecting edge elements
        float positionOffset = 0.999f;

        var elementPosition = List.GetElementPosition(elementData.Index);

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
        if (Organizer == null) return;
        
        if (Display.DataController.DataList.Count == 0) return;
        
        if (Display.SelectionType == SelectionManager.Type.Automatic)
        {
            EditorElement element = List.ElementList.FirstOrDefault();

            element.InvokeSelection();
        }
    }

    public void CloseList()
    {
        if (Organizer == null) return;

        ScrollRect.horizontal = false;
        ScrollRect.vertical = false;
        
        overlayManager.CloseOverlay();
        Organizer.CloseOrganizer();
        
        transform.parent.gameObject.SetActive(false);
    }
}
