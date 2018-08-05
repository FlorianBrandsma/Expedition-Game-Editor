using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlideshowManager : MonoBehaviour, IOverlay
{
    static public List<Button> button_list = new List<Button>();
    private List<Button> button_list_local = new List<Button>();

    private Color active_color      = Color.white;
    private Color inactive_color    = Color.gray;

    private int     element_total;
    private int     page_limit;
    private int     page_count;
    private float   button_size;

    int             selected_page;

    OverlayManager  overlayManager;
    IOrganizer      organizer;


    public void InitializeOverlay(ListManager listManager)
    {
        overlayManager = GetComponent<OverlayManager>();    
    }

    public void ActivateOverlay(IOrganizer new_organizer)
    {
        organizer = new_organizer;

        Vector2 list_size = organizer.GetListSize(overlayManager.listManager.id_list, false);

        page_limit =  (int)list_size.x;
        element_total = overlayManager.listManager.id_list.Count;

        page_count = Mathf.CeilToInt((float)element_total / page_limit);

        if (page_count > 1)
            overlayManager.horizontal_max.gameObject.SetActive(true);        
    }

    public void SetOverlay()
    {
        if(overlayManager.horizontal_max.gameObject.activeInHierarchy)
        {
            float max_size = overlayManager.horizontal_max.rect.height;

            button_size = (overlayManager.horizontal_max.rect.width / 2) / page_count;

            if (button_size > max_size)
                button_size = max_size;

            for (int i = 0; i < page_count; i++)
            {
                Button button = SpawnButton();
                button_list_local.Add(button);

                button.GetComponent<RectTransform>().sizeDelta = new Vector2(button_size, button_size);

                button.transform.SetParent(overlayManager.horizontal_max, false);

                float offset = -(button_size * (page_count - 1));

                button.transform.localPosition = new Vector2(offset + ((button_size * i) * 2), 0);

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
        selected_page = page;

        button_list_local[selected_page].GetComponent<RawImage>().color = active_color;

        OpenPage();
    }

    void ResetSelection()
    {
        button_list_local[selected_page].GetComponent<RawImage>().color = inactive_color;
    }

    void OpenPage()
    {
        int start = selected_page * page_limit;
        int count = page_limit;

        int remainder = element_total - start;

        if (count > remainder)
            count = remainder;

        organizer.ResetRows(overlayManager.listManager.id_list.GetRange(start, count));
    }
     
    public void UpdateOverlay()
    {

    }

    public void CloseOverlay()
    {
        ResetButtons();

        DestroyImmediate(this);
    }

    public Button SpawnButton()
    {
        foreach (Button button in button_list)
        {
            if (!button.gameObject.activeInHierarchy)
                return button;
        }

        Button new_button = Instantiate(Resources.Load<Button>("Editor/Overlay/Slideshow_Button"));

        button_list.Add(new_button);

        return new_button;
    }

    public void ResetButtons()
    {
        foreach (Button button in button_list_local)
        {
            button.onClick.RemoveAllListeners();
            button.GetComponent<RawImage>().color = inactive_color;
            button.gameObject.SetActive(false);
        }   
    }
}
