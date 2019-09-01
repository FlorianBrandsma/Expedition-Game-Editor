using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OverlayManager : MonoBehaviour
{
    private RectTransform   mainList;

    public RectTransform    horizontal_min,
                            vertical_min,
                            horizontal_max,
                            vertical_max;

    public IDisplayManager  DisplayManager { get; set; }

    public void InitializeOverlay(IDisplayManager displayManager)
    {
        DisplayManager = displayManager;

        mainList = DisplayManager.RectTransform;

        horizontal_min.GetComponent<OverlayBorder>().Activate();
        horizontal_max.GetComponent<OverlayBorder>().Activate();
        vertical_min.GetComponent<OverlayBorder>().Activate();
        vertical_max.GetComponent<OverlayBorder>().Activate();
    }

    public void SetOverlayProperties(ListProperties listProperties)
    {
        if (listProperties.enableNumbers)
            gameObject.AddComponent<NumberManager>();

        if (listProperties.enableSliders)
            gameObject.AddComponent<SliderManager>();

        if (listProperties.enablePaging)
            gameObject.AddComponent<PagingManager>();

        foreach (IOverlay overlay in GetComponents<IOverlay>())
            overlay.InitializeOverlay(DisplayManager);
    }

    public void ActivateOverlay(IOrganizer organizer, IList list)
    {
        foreach (IOverlay overlay in GetComponents<IOverlay>())
            overlay.ActivateOverlay(organizer, list);
    }

    public void SetOverlaySize()
    {
        if (horizontal_min.gameObject.activeInHierarchy)
        {
            vertical_min.GetComponent<RectTransform>().offsetMax = new Vector2(vertical_min.offsetMax.x, -horizontal_min.rect.height);
            vertical_max.GetComponent<RectTransform>().offsetMax = new Vector2(vertical_max.offsetMax.x, -horizontal_min.rect.height);
        } 

        if (horizontal_max.gameObject.activeInHierarchy)
        {
            vertical_min.GetComponent<RectTransform>().offsetMin = new Vector2(vertical_min.offsetMin.x, horizontal_max.rect.height);
            vertical_max.GetComponent<RectTransform>().offsetMin = new Vector2(vertical_max.offsetMin.x, horizontal_max.rect.height);
        }

        if (vertical_min.gameObject.activeInHierarchy)
        {
            horizontal_min.GetComponent<RectTransform>().offsetMin = new Vector2(vertical_min.rect.width, horizontal_min.offsetMin.y);
            horizontal_max.GetComponent<RectTransform>().offsetMin = new Vector2(vertical_min.rect.width, horizontal_max.offsetMin.y);
        }

        if (vertical_max.gameObject.activeInHierarchy)
        {
            horizontal_min.GetComponent<RectTransform>().offsetMax = new Vector2(-vertical_max.rect.width, horizontal_min.offsetMax.y);
            horizontal_max.GetComponent<RectTransform>().offsetMax = new Vector2(-vertical_max.rect.width, horizontal_max.offsetMax.y);
        }

        SetListSize();
    }

    public void SetListSize()
    {
        if (horizontal_min.gameObject.activeInHierarchy)
            mainList.offsetMax = new Vector2(mainList.offsetMin.x, -horizontal_min.rect.height);

        if (horizontal_max.gameObject.activeInHierarchy)
            mainList.offsetMin = new Vector2(mainList.offsetMax.x, horizontal_max.rect.height);

        if (vertical_min.gameObject.activeInHierarchy)
            mainList.offsetMin = new Vector2(vertical_min.rect.width, mainList.offsetMin.y);

        if (vertical_max.gameObject.activeInHierarchy)
            mainList.offsetMax = new Vector2(-vertical_max.rect.width, mainList.offsetMax.y);
    }

    public void SetOverlay()
    {
        foreach (IOverlay overlay in GetComponents<IOverlay>())
            overlay.SetOverlay();
    }

    public void UpdateOverlay()
    {
        foreach(IOverlay overlay in GetComponents<IOverlay>())
            overlay.UpdateOverlay();  
    }

    public void CloseOverlay()
    {
        mainList.offsetMin = Vector2.zero;
        mainList.offsetMax = Vector2.zero;

        horizontal_min.GetComponent<OverlayBorder>().Deactivate();
        horizontal_max.GetComponent<OverlayBorder>().Deactivate();
        vertical_min.GetComponent<OverlayBorder>().Deactivate();
        vertical_max.GetComponent<OverlayBorder>().Deactivate();
  
        foreach (IOverlay overlay in GetComponents<IOverlay>())
            overlay.CloseOverlay();    
    }  
}
