using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class NumberOverlay : MonoBehaviour, IOverlay
{
    static public List<Text> numberList = new List<Text>();
    private List<Text> numberListLocal = new List<Text>();

    private ListManager     listManager;

    private RectTransform   listParent;

    private RectTransform   horizontalParent,
                            verticalParent;

    private OverlayManager  overlayManager { get { return GetComponent<OverlayManager>(); } }

    public void InitializeOverlay(IDisplayManager displayManager)
    {
        listManager = (ListManager)displayManager;

        listParent = listManager.listParent;
    }

    public void ActivateOverlay(IOrganizer organizer)
    {
        int listCount = overlayManager.DisplayManager.Display.DataController.DataList.Count;

        var list = (IList)organizer;

        Vector2 listSize = list.GetListSize(listCount, false);

        if (listSize.x > 0)
            overlayManager.horizontal_min.gameObject.SetActive(true);

        if (listSize.y > 0)
            overlayManager.vertical_min.gameObject.SetActive(true);
    }

    public void SetOverlay()
    {
        Vector2 listSize = listManager.listSize;

        Vector2 baseSize = new Vector2(listParent.rect.width / listSize.x, listParent.rect.height / listSize.y);

        if(overlayManager.horizontal_min.gameObject.activeInHierarchy)
        {
            horizontalParent = overlayManager.horizontal_min.GetComponent<OverlayBorder>().ScrollParent();

            for (int x = 0; x < listSize.x; x++)
                SetNumbers(overlayManager.horizontal_min, x, -((baseSize.x * 0.5f) * (listSize.x - 1)) + (x * baseSize.x));
        }

        if(overlayManager.vertical_min.gameObject.activeInHierarchy)
        {
            verticalParent = overlayManager.vertical_min.GetComponent<OverlayBorder>().ScrollParent();

            for (int y = 0; y < listSize.y; y++)
                SetNumbers(overlayManager.vertical_min, y, -(baseSize.y * 0.5f) + (listParent.sizeDelta.y / 2f) - (y * baseSize.y));
        }

        UpdateOverlay();
    }

    public void UpdateOverlay()
    {
        if (verticalParent != null)
            verticalParent.transform.localPosition = new Vector2(0, listParent.transform.localPosition.y );

        if (horizontalParent != null)
            horizontalParent.transform.localPosition = new Vector2(listParent.transform.localPosition.x, 0);
    }

    public void SetNumbers(RectTransform numberParent, int index, float new_position)
    {
        OverlayBorder overlayBorder = numberParent.GetComponent<OverlayBorder>();

        Text newText = SpawnText(numberList);
        numberListLocal.Add(newText);

        newText.text = (index + 1).ToString();
        newText.transform.SetParent(overlayBorder.scroll_parent, false);

        if (overlayBorder.vertical)
            newText.transform.localPosition = new Vector2(0, new_position);

        if (overlayBorder.horizontal)
            newText.transform.localPosition = new Vector2(new_position, 0);
    }

    private Text SpawnText(List<Text> list)
    {
        foreach (Text element in list)
        {
            if (!element.gameObject.activeInHierarchy)
            {
                element.gameObject.SetActive(true);
                return element;
            }
        }

        Text newText = Instantiate(Resources.Load<Text>("UI/Number"));
        list.Add(newText);

        return newText;
    }

    public void ResetText(List<Text> list)
    {
        foreach (Text element in list)
            element.gameObject.SetActive(false);
    }

    public void CloseOverlay()
    {
        ResetText(numberListLocal);

        DestroyImmediate(this);
    }
}
