using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderManager : MonoBehaviour, IOverlay
{
    static public List<Slider> horizontal_slider_list = new List<Slider>();
    static public List<Slider> vertical_slider_list = new List<Slider>();
    private List<Slider> slider_list_local = new List<Slider>();

    private Slider  horizontal_slider,
                    vertical_slider;

    private RectTransform   main_list;

    OverlayManager          overlayManager;

    public void InitializeOverlay(ListManager manager)
    {
        overlayManager = GetComponent<OverlayManager>();

        main_list = manager.rectTransform;
    }
    
    public void ActivateOverlay(IOrganizer organizer, IList list)
    {
        int list_count = overlayManager.listManager.listProperties.SegmentController.DataController.DataList.Count;
        
        Vector2 list_size = list.GetListSize(list_count, true);

        bool horizontal = main_list.GetComponent<ScrollRect>().horizontal;
        bool vertical = main_list.GetComponent<ScrollRect>().vertical;

        if(vertical)
        {
            if (list_size.y > (main_list.rect.height) - (horizontal ? overlayManager.horizontal_max.rect.height : 0))
                overlayManager.vertical_max.gameObject.SetActive(true);
        }

        if(horizontal)
        {
            if ((list_size.x + main_list.rect.width) > (main_list.rect.width) - (vertical ? overlayManager.vertical_max.rect.width : 0))
                overlayManager.horizontal_max.gameObject.SetActive(true);
        } 
    }

    public void SetOverlay()
    {
        if (overlayManager.vertical_max.gameObject.activeInHierarchy)
        {
            vertical_slider = SpawnVerticalSlider();

            slider_list_local.Add(vertical_slider);

            vertical_slider.transform.SetParent(overlayManager.vertical_max, false);

            vertical_slider.gameObject.SetActive(true);
        }

        if (overlayManager.horizontal_max.gameObject.activeInHierarchy)
        {
            horizontal_slider = SpawnHorizontalSlider();

            slider_list_local.Add(horizontal_slider);

            horizontal_slider.transform.SetParent(overlayManager.horizontal_max, false);

            horizontal_slider.gameObject.SetActive(true);
        }

        UpdateOverlay();
    }

    public void UpdateOverlay()
    {
        if (vertical_slider != null)
            vertical_slider.value = Mathf.Clamp(main_list.GetComponent<ScrollRect>().verticalNormalizedPosition, 0, 1);
        if (horizontal_slider != null)
            horizontal_slider.value = Mathf.Clamp(main_list.GetComponent<ScrollRect>().horizontalNormalizedPosition, 0, 1);
    }

    public void CloseOverlay()
    {
        ResetSliders();

        DestroyImmediate(this);
    }

    public Slider SpawnHorizontalSlider()
    {
        foreach(Slider slider in horizontal_slider_list)
        {
            if (!slider.gameObject.activeInHierarchy)
                return slider;      
        }

        Slider new_slider = Instantiate(Resources.Load<Slider>("Editor/Overlay/Slider_Horizontal"));

        horizontal_slider_list.Add(new_slider);

        return new_slider;
    }

    public Slider SpawnVerticalSlider()
    {
        foreach (Slider slider in vertical_slider_list)
        {
            if (!slider.gameObject.activeInHierarchy)
                return slider;
        }

        Slider new_slider = Instantiate(Resources.Load<Slider>("Editor/Overlay/Slider_Vertical"));

        vertical_slider_list.Add(new_slider);

        return new_slider;
    }

    public void ResetSliders()
    {
        foreach (Slider slider in slider_list_local)
            slider.gameObject.SetActive(false);
    }
}
