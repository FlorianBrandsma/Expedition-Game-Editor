using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderManager : MonoBehaviour, IOverlay
{
    static public List<Slider> horizontalSliderList = new List<Slider>();
    static public List<Slider> verticalSliderList = new List<Slider>();
    private List<Slider> sliderListLocal = new List<Slider>();

    private Slider  horizontalSlider,
                    verticalSlider;

    private RectTransform mainList;

    private OverlayManager overlayManager { get { return GetComponent<OverlayManager>(); } }

    public void InitializeOverlay(IDisplayManager displayManager)
    {
        mainList = displayManager.RectTransform;
    }
    
    public void ActivateOverlay(IOrganizer organizer, IList list)
    {
        int listCount = overlayManager.DisplayManager.Display.DataController.DataList.Count;
        
        Vector2 listSize = list.GetListSize(listCount, true);

        bool horizontal = mainList.GetComponent<ScrollRect>().horizontal;
        bool vertical = mainList.GetComponent<ScrollRect>().vertical;

        if(vertical)
        {
            if (listSize.y > (mainList.rect.height) - (horizontal ? overlayManager.horizontal_max.rect.height : 0))
                overlayManager.vertical_max.gameObject.SetActive(true);
        }

        if(horizontal)
        {
            if ((listSize.x + mainList.rect.width) > (mainList.rect.width) - (vertical ? overlayManager.vertical_max.rect.width : 0))
                overlayManager.horizontal_max.gameObject.SetActive(true);
        } 
    }

    public void SetOverlay()
    {
        if (overlayManager.vertical_max.gameObject.activeInHierarchy)
        {
            verticalSlider = SpawnVerticalSlider();

            sliderListLocal.Add(verticalSlider);

            verticalSlider.transform.SetParent(overlayManager.vertical_max, false);

            verticalSlider.gameObject.SetActive(true);
        }

        if (overlayManager.horizontal_max.gameObject.activeInHierarchy)
        {
            horizontalSlider = SpawnHorizontalSlider();

            sliderListLocal.Add(horizontalSlider);

            horizontalSlider.transform.SetParent(overlayManager.horizontal_max, false);

            horizontalSlider.gameObject.SetActive(true);
        }

        UpdateOverlay();
    }

    public void UpdateOverlay()
    {
        if (verticalSlider != null)
            verticalSlider.value = Mathf.Clamp(mainList.GetComponent<ScrollRect>().verticalNormalizedPosition, 0, 1);
        if (horizontalSlider != null)
            horizontalSlider.value = Mathf.Clamp(mainList.GetComponent<ScrollRect>().horizontalNormalizedPosition, 0, 1);
    }

    public void CloseOverlay()
    {
        ResetSliders();

        DestroyImmediate(this);
    }

    public Slider SpawnHorizontalSlider()
    {
        foreach(Slider slider in horizontalSliderList)
        {
            if (!slider.gameObject.activeInHierarchy)
                return slider;      
        }

        Slider newSlider = Instantiate(Resources.Load<Slider>("Editor/Overlay/Slider_Horizontal"));

        horizontalSliderList.Add(newSlider);

        return newSlider;
    }

    public Slider SpawnVerticalSlider()
    {
        foreach (Slider slider in verticalSliderList)
        {
            if (!slider.gameObject.activeInHierarchy)
                return slider;
        }

        Slider newSlider = Instantiate(Resources.Load<Slider>("Editor/Overlay/Slider_Vertical"));

        verticalSliderList.Add(newSlider);

        return newSlider;
    }

    public void ResetSliders()
    {
        foreach (Slider slider in sliderListLocal)
            slider.gameObject.SetActive(false);
    }
}
