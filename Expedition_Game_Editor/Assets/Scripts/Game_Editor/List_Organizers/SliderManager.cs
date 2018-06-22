using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderManager : MonoBehaviour, IOverlay
{
    static public List<Slider> slider_list = new List<Slider>();
    private List<Slider> slider_list_local = new List<Slider>();

    private Slider  horizontal_slider,
                    vertical_slider;

    private RectTransform   main_list, 
                            list_parent;

    OverlayManager          overlayManager;

    public void InitializeOverlay(RectTransform new_main_list, RectTransform new_list_parent)
    {
        overlayManager = GetComponent<OverlayManager>();

        main_list = new_main_list;
        list_parent = new_list_parent;
    }
    
    public void SetOverlay()
    {
        if (list_parent.sizeDelta.y > main_list.rect.max.y * 2)
        {
            vertical_slider = SpawnSlider("Vertical");

            slider_list_local.Add(vertical_slider);

            overlayManager.SetParent(   vertical_slider.GetComponent<RectTransform>(),
                                        overlayManager.vertical_max);

            vertical_slider.gameObject.SetActive(true);
        }

        if ((list_parent.sizeDelta.x + main_list.rect.width) > main_list.rect.max.x * 2)
        {
            horizontal_slider = SpawnSlider("Horizontal");

            slider_list_local.Add(horizontal_slider);

            overlayManager.SetParent(   horizontal_slider.GetComponent<RectTransform>(), 
                                        overlayManager.horizontal_max);

            horizontal_slider.gameObject.SetActive(true);
        }

        overlayManager.SetOverlaySize();
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

    public Slider SpawnSlider(string axis)
    {
        foreach(Slider slider in slider_list)
        {
            if (!slider.gameObject.activeInHierarchy)
                return slider;      
        }

        Slider new_slider = Instantiate(Resources.Load<Slider>("Editor/Overlay/Slider_" + axis));

        slider_list.Add(new_slider);

        return new_slider;
    }

    public void ResetSliders()
    {
        foreach (Slider slider in slider_list_local)
            slider.gameObject.SetActive(false);
    }
}
