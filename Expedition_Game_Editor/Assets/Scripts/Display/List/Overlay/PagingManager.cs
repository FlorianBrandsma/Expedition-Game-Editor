using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PagingManager : MonoBehaviour, IOverlay
{
    static public List<Button> buttonList = new List<Button>();
    private List<Button> buttonListLocal = new List<Button>();

    private Color activeColor   = Color.white;
    private Color inactiveColor = Color.gray;

    private int     elementTotal;
    private int     pageLimit;
    private int     pageCount;
    private float   buttonSize;

    private int     selectedPage;

    private OverlayManager overlayManager { get { return GetComponent<OverlayManager>(); } }

    private IOrganizer organizer;
    private IList list;

    public void InitializeOverlay(IDisplayManager displayManager) { }

    public void ActivateOverlay(IOrganizer organizer)
    {
        this.organizer = organizer;
        list = (IList)organizer;

        int listCount = overlayManager.DisplayManager.Display.DataController.DataList.Count;

        Vector2 listSize = this.list.GetListSize(listCount, false);

        pageLimit =  (int)(listSize.x * listSize.y);
        elementTotal = listCount;

        pageCount = Mathf.CeilToInt((float)elementTotal / pageLimit);

        overlayManager.horizontal_max.gameObject.SetActive(true);        
    }

    public void SetOverlay()
    {
        if(overlayManager.horizontal_max.gameObject.activeInHierarchy)
        {
            float maxSize = overlayManager.horizontal_max.rect.height;

            buttonSize = (overlayManager.horizontal_max.rect.width / 2) / pageCount;

            if (buttonSize > maxSize)
                buttonSize = maxSize;

            for (int i = 0; i < pageCount; i++)
            {
                Button button = SpawnButton();
                buttonListLocal.Add(button);

                button.GetComponent<RectTransform>().sizeDelta = new Vector2(buttonSize, buttonSize);

                button.transform.SetParent(overlayManager.horizontal_max, false);

                float offset = -(buttonSize * (pageCount - 1));

                button.transform.localPosition = new Vector2(offset + ((buttonSize * i) * 2), 0);

                int index = i;

                button.onClick.AddListener(delegate { SelectPage(index); });

                button.gameObject.SetActive(true);
            }
            SelectPage(0);
        }      
    }

    void SelectPage(int page)
    {
        ResetSelection();
        selectedPage = page;

        buttonListLocal[selectedPage].GetComponent<RawImage>().color = activeColor;

        OpenPage();
    }

    void ResetSelection()
    {
        buttonListLocal[selectedPage].GetComponent<RawImage>().color = inactiveColor;
    }

    void OpenPage()
    {
        int start = selectedPage * pageLimit;
        int count = pageLimit;

        int remainder = elementTotal - start;

        if (count > remainder)
            count = remainder;

        var dataController = overlayManager.DisplayManager.Display.DataController;

        organizer.ResetData(dataController.DataList.GetRange(start, count));
    }
     
    public void UpdateOverlay()
    {

    }

    public void CloseOverlay()
    {
        CloseButtons();

        DestroyImmediate(this);
    }

    public Button SpawnButton()
    {
        foreach (Button button in buttonList)
        {
            if (!button.gameObject.activeInHierarchy)
                return button;
        }

        Button newButton = Instantiate(Resources.Load<Button>("Editor/Overlay/Slideshow_Button"));

        buttonList.Add(newButton);

        return newButton;
    }

    public void CloseButtons()
    {
        foreach (Button button in buttonListLocal)
        {
            button.onClick.RemoveAllListeners();
            button.GetComponent<RawImage>().color = inactiveColor;
            button.gameObject.SetActive(false);
        }   
    }
}
