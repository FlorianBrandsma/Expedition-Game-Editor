using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderOverlay : MonoBehaviour, IOverlay
{
    private List<ExSlider> sliderList = new List<ExSlider>();

    private ExSlider horizontalSlider,
                     verticalSlider;

    private RectTransform mainList;

    private OverlayManager overlayManager { get { return GetComponent<OverlayManager>(); } }

    public void InitializeOverlay(IDisplayManager displayManager)
    {
        mainList = displayManager.RectTransform;
    }
    
    public void ActivateOverlay(IOrganizer organizer)
    {
        int listCount = overlayManager.DisplayManager.Display.DataController.Data.dataList.Count;
        var list = (IList)organizer;

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

            sliderList.Add(verticalSlider);

            verticalSlider.transform.SetParent(overlayManager.vertical_max, false);

            verticalSlider.gameObject.SetActive(true);
        }

        if (overlayManager.horizontal_max.gameObject.activeInHierarchy)
        {
            horizontalSlider = SpawnHorizontalSlider();

            sliderList.Add(horizontalSlider);

            horizontalSlider.transform.SetParent(overlayManager.horizontal_max, false);

            horizontalSlider.gameObject.SetActive(true);
        }

        UpdateOverlay();
    }

    public void UpdateOverlay()
    {
        if (verticalSlider != null)
            verticalSlider.Slider.value = Mathf.Clamp(mainList.GetComponent<ScrollRect>().verticalNormalizedPosition, 0, 1);
        if (horizontalSlider != null)
            horizontalSlider.Slider.value = Mathf.Clamp(mainList.GetComponent<ScrollRect>().horizontalNormalizedPosition, 0, 1);
    }

    public void CloseOverlay()
    {
        ResetSliders();

        DestroyImmediate(this);
    }

    public ExSlider SpawnHorizontalSlider()
    {
        var prefab = Resources.Load<ExSlider>("Elements/UI/SliderHorizontal");
        var slider = (ExSlider)PoolManager.SpawnObject(prefab);

        slider.gameObject.SetActive(true);

        return slider;
    }

    public ExSlider SpawnVerticalSlider()
    {
        var prefab = Resources.Load<ExSlider>("Elements/UI/SliderVertical");
        var slider = (ExSlider)PoolManager.SpawnObject(prefab);

        slider.gameObject.SetActive(true);

        return slider;
    }

    public void ResetSliders()
    {
        sliderList.ForEach(x => PoolManager.ClosePoolObject(x));
        
        sliderList.Clear();
    }
}
