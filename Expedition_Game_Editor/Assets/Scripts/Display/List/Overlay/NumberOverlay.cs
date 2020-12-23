using UnityEngine;
using System.Collections.Generic;

public class NumberOverlay : MonoBehaviour, IOverlay
{
    private List<ExText> numberListLocal = new List<ExText>();

    private IDisplayManager DisplayManager  { get; set; }
    private ListManager ListManager         { get { return (ListManager)DisplayManager; } }
    private ListProperties ListProperties   { get { return (ListProperties)DisplayManager.Display; } }

    private RectTransform   listParent;

    private RectTransform   horizontalParent,
                            verticalParent;

    private OverlayManager  overlayManager { get { return GetComponent<OverlayManager>(); } }

    public void InitializeOverlay(IDisplayManager displayManager)
    {
        DisplayManager = displayManager;

        listParent = ListManager.listParent;
    }

    public void ActivateOverlay(IOrganizer organizer)
    {
        int listCount = overlayManager.DisplayManager.Display.DataController.Data.dataList.Count;

        var listSize = ((IList)organizer).GetListSize(false);

        if (listSize.x > 0)
            overlayManager.horizontal_min.gameObject.SetActive(true);

        if (listSize.y > 0)
            overlayManager.vertical_min.gameObject.SetActive(true);
    }

    public void SetOverlay()
    {
        var listSize = ListManager.listSize;

        var baseSize = new Vector2(listParent.rect.width / listSize.x, listParent.rect.height / listSize.y);

        if(overlayManager.horizontal_min.gameObject.activeInHierarchy)
        {
            horizontalParent = overlayManager.horizontal_min.GetComponent<OverlayBorder>().scrollParent;

            for (int x = 0; x < listSize.x; x++)
            {
                var number = GetNumber(x);
                var position = -((baseSize.x * 0.5f) * (listSize.x - 1)) + (x * baseSize.x);

                SetNumbers(overlayManager.horizontal_min, number, position);
            }
        }

        if(overlayManager.vertical_min.gameObject.activeInHierarchy)
        {
            verticalParent = overlayManager.vertical_min.GetComponent<OverlayBorder>().scrollParent;

            for (int y = 0; y < listSize.y; y++)
            {
                var number = GetNumber(y);

                var position = -(baseSize.y * 0.5f) + (listParent.sizeDelta.y / 2f) - (y * baseSize.y);

                SetNumbers(overlayManager.vertical_min, number, position);
            }
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

    private string GetNumber(int index)
    {
        var number = "";

        if (ListProperties.AddProperty == SelectionManager.Property.None)
        {
            number = (index + 1).ToString();

        } else {

            if (index > 0)
                number = (index).ToString();
        }

        return number;
    }

    public void SetNumbers(RectTransform numberParent, string number, float position)
    {
        var overlayBorder = numberParent.GetComponent<OverlayBorder>();

        var prefab = Resources.Load<ExText>("Elements/UI/Text");
        
        var newText = (ExText)PoolManager.SpawnObject(prefab);
        numberListLocal.Add(newText);

        newText.Text.text = number;
        newText.transform.SetParent(overlayBorder.scrollParent, false);
        
        if (overlayBorder.vertical)
        {
            newText.RectTransform.anchorMin = new Vector2(0, 0.5f);
            newText.RectTransform.anchorMax = new Vector2(1, 0.5f);

            var width = overlayBorder.scrollParent.rect.width;
            newText.RectTransform.sizeDelta = new Vector2(0, width);

            newText.transform.localPosition = new Vector2(0, position);
        }
        
        if (overlayBorder.horizontal)
        {
            newText.RectTransform.anchorMin = new Vector2(0.5f, 0);
            newText.RectTransform.anchorMax = new Vector2(0.5f, 1);

            var height = overlayBorder.scrollParent.rect.height;
            newText.RectTransform.sizeDelta = new Vector2(height, 0);

            newText.transform.localPosition = new Vector2(position, 0);
        }
        
        newText.gameObject.SetActive(true);
    }

    public void ResetText(List<ExText> list)
    {
        list.ForEach(x => PoolManager.ClosePoolObject(x));

        list.Clear();
    }

    public void CloseOverlay()
    {
        ResetText(numberListLocal);

        DestroyImmediate(this);
    }
}
