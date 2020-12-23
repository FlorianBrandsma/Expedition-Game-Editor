using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ListManager : MonoBehaviour, IDisplayManager
{
    private Vector2 trackedPosition = new Vector2();

    public IOrganizer       Organizer       { get; set; }
    public IDisplay         Display         { get; set; }

    private ListProperties  listProperties;

    public OverlayManager   overlayManager;

    public ScrollRect       ScrollRect      { get { return GetComponent<ScrollRect>(); } }
    public RectTransform    RectTransform   { get { return GetComponent<RectTransform>(); } }
    public RectTransform    listParent;

    public Vector2          listSize        { get; set; }
    public RectTransform    ParentRect      { get { return transform.parent.GetComponent<RectTransform>(); } }

    public IList List { get { return GetComponent<IList>(); } }

    public void InitializeList(ListProperties listProperties)
    {
        if (GetComponent<IOrganizer>() != null) return;

        Display = listProperties;
        this.listProperties = listProperties;
        
        switch(listProperties.OrganizerType)
        {
            case DisplayManager.OrganizerType.None:      Organizer = null;                                           break;
            case DisplayManager.OrganizerType.Tile:      Organizer = gameObject.AddComponent<TileOrganizer>();       break;
            case DisplayManager.OrganizerType.Panel:     Organizer = gameObject.AddComponent<PanelOrganizer>();      break;
            case DisplayManager.OrganizerType.PanelTile: Organizer = gameObject.AddComponent<PanelTileOrganizer>();  break;
            case DisplayManager.OrganizerType.MultiGrid: Organizer = gameObject.AddComponent<MultiGridOrganizer>();  break;

            default: Debug.Log("CASE MISSING: " + listProperties.OrganizerType); break;
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

        var dataList = Display.DataController.Data.dataList;

        overlayManager.ActivateOverlay(Organizer);
        overlayManager.SetOverlaySize();

        listParent.sizeDelta = List.GetListSize(true);
        listSize = List.GetListSize(false);

        if (dataList.Count > 0)
        {
            if (!Display.DataController.SegmentController.Loaded)
                ResetListPosition();

            //Select data after the list has been resized, so that the position may be properly corrected
            //"Set" elements never receive this kind of visual feedback
            if (listProperties.SelectionProperty != SelectionManager.Property.Set)
                Organizer.SelectData();

            if (!listProperties.enablePaging)
                SetData();
        }

        overlayManager.SetOverlay();
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

        ResetData();
    }

    private void ResetData()
    {
        if (Mathf.Abs(trackedPosition.x - listParent.localPosition.x) > List.ElementSize.x ||
            Mathf.Abs(trackedPosition.y - listParent.localPosition.y) > List.ElementSize.y)
        {
            trackedPosition = listParent.localPosition;
            
            Organizer.ClearOrganizer();
            Organizer.SetData();
        }
    }

    public bool ElementBelowMin(Vector2 elementPosition, bool extraRow = false)
    {
        //To offset jittering when selecting edge elements
        float positionOffset = 0.999f;

        var extraRowOffset = extraRow ? List.ElementSize * 2 : new Vector2();
        
        var localElementPosition = new Vector2(elementPosition.x + listParent.localPosition.x,
                                               elementPosition.y + listParent.localPosition.y) * positionOffset;

        var elementMin = new Vector2(localElementPosition.x - List.ElementSize.x / 2,
                                     localElementPosition.y - List.ElementSize.y / 2);

        return (elementMin.x < RectTransform.rect.min.x - extraRowOffset.x ||
                elementMin.y < RectTransform.rect.min.y - extraRowOffset.y);
    }

    public bool ElementAboveMax(Vector2 elementPosition, bool extraRow = false)
    {
        //To offset jittering when selecting edge elements
        float positionOffset = 0.999f;

        var extraRowOffset = extraRow ? List.ElementSize * 2 : new Vector2();

        var localElementPosition = new Vector2(elementPosition.x + listParent.localPosition.x,
                                               elementPosition.y + listParent.localPosition.y) * positionOffset;

        var elementMax = new Vector2(localElementPosition.x + List.ElementSize.x / 2,
                                     localElementPosition.y + List.ElementSize.y / 2);

        return (elementMax.x > RectTransform.rect.max.x + extraRowOffset.x ||
                elementMax.y > RectTransform.rect.max.y + extraRowOffset.y);
    }

    public void CorrectPosition(IElementData elementData, List<IElementData> dataList)
    {
        if (!listProperties.enablePositionCorrection) return;

        var elementPosition = List.GetElementPosition(dataList.IndexOf(elementData));

        if (ElementBelowMin(elementPosition) || ElementAboveMax(elementPosition))
        {
            ScrollRect.horizontalNormalizedPosition = ((elementPosition.x - List.ElementSize.x / 2) + listParent.rect.width / 2) / ((listParent.rect.width - List.ElementSize.x) / 2) / 2;
            ScrollRect.verticalNormalizedPosition   = (elementPosition.y + ((listParent.sizeDelta.y - List.ElementSize.y) / 2)) / (listParent.sizeDelta.y - List.ElementSize.y);
        }
    }

    public void AutoSelectElement(int autoSelectId)
    {
        if (Organizer == null) return;
        
        if (Display.DataController.Data.dataList.Count == 0) return;

        EditorElement element = List.ElementList.Where(x => x.DataElement.ElementData.Id == autoSelectId).FirstOrDefault();

        if(element == null)
            element = List.ElementList.FirstOrDefault();

        element.InvokeSelection();
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
