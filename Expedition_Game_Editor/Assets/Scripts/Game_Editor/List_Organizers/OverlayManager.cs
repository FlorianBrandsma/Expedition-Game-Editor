﻿using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OverlayManager : MonoBehaviour
{
    private List<Text>      number_list = new List<Text>();

    private RectTransform   main_list, 
                            list_parent;

    public RectTransform    horizontal_min,
                            vertical_min,
                            horizontal_max,
                            vertical_max;

    private bool            horizontal, 
                            vertical;

    public ListManager      listManager { get; set; }

    public void InitializeOverlay(ListManager new_listManager)
    {
        listManager = new_listManager;

        main_list = listManager.main_list;
        list_parent = listManager.list_parent;   
    }

    public void SetOverlayProperties(ListProperties listProperties)
    {
        if (listProperties.enable_numbers)
            gameObject.AddComponent<NumberManager>();

        if (listProperties.enable_sliders)
            gameObject.AddComponent<SliderManager>();

        if (listProperties.enable_slideshow)
            gameObject.AddComponent<SlideshowManager>();

        foreach (IOverlay overlay in GetComponents<IOverlay>())
            overlay.InitializeOverlay(main_list, list_parent);
    }

    public void SetOverlay()
    {
        foreach (IOverlay overlay in GetComponents<IOverlay>())
            overlay.SetOverlay(); 
    }

    public void SetParent(RectTransform element, RectTransform border)
    {
        element.SetParent(border, false);
        border.GetComponent<OverlayBorder>().Activate();
    }

    public void SetOverlaySize()
    {
        if (horizontal_min.GetComponent<OverlayBorder>().active || horizontal_min.GetComponent<OverlayBorder>().always_active)
        {
            vertical_min.GetComponent<RectTransform>().offsetMax = new Vector2(vertical_min.offsetMax.x, -horizontal_min.rect.height);
            vertical_max.GetComponent<RectTransform>().offsetMax = new Vector2(vertical_max.offsetMax.x, -horizontal_min.rect.height);
        } 

        if (horizontal_max.GetComponent<OverlayBorder>().active || horizontal_max.GetComponent<OverlayBorder>().always_active)
        {
            vertical_min.GetComponent<RectTransform>().offsetMin = new Vector2(vertical_min.offsetMin.x, horizontal_max.rect.height);
            vertical_max.GetComponent<RectTransform>().offsetMin = new Vector2(vertical_max.offsetMin.x, horizontal_max.rect.height);
        }

        if (vertical_min.GetComponent<OverlayBorder>().active || vertical_min.GetComponent<OverlayBorder>().always_active)
        {
            horizontal_min.GetComponent<RectTransform>().offsetMin = new Vector2(vertical_min.rect.width, horizontal_min.offsetMin.y);
            horizontal_max.GetComponent<RectTransform>().offsetMin = new Vector2(vertical_min.rect.width, horizontal_max.offsetMin.y);
        }

        if (vertical_max.GetComponent<OverlayBorder>().active || vertical_max.GetComponent<OverlayBorder>().always_active)
        {
            horizontal_min.GetComponent<RectTransform>().offsetMax = new Vector2(-vertical_max.rect.width, horizontal_min.offsetMax.y);
            horizontal_max.GetComponent<RectTransform>().offsetMax = new Vector2(-vertical_max.rect.width, horizontal_max.offsetMax.y);
        }

        SetListSize();
    }

    public void SetListSize()
    {
        main_list.offsetMin = Vector2.zero;
        main_list.offsetMax = Vector2.zero;

        if (horizontal_min.GetComponent<OverlayBorder>().active || horizontal_min.GetComponent<OverlayBorder>().always_active)
            main_list.offsetMax = new Vector2(main_list.offsetMin.x, -horizontal_min.rect.height);

        if (horizontal_max.GetComponent<OverlayBorder>().active || horizontal_max.GetComponent<OverlayBorder>().always_active)
            main_list.offsetMin = new Vector2(main_list.offsetMax.x, horizontal_max.rect.height);

        if (vertical_min.GetComponent<OverlayBorder>().active || vertical_min.GetComponent<OverlayBorder>().always_active)
            main_list.offsetMin = new Vector2(vertical_min.rect.width, main_list.offsetMin.y);

        if (vertical_max.GetComponent<OverlayBorder>().active || vertical_max.GetComponent<OverlayBorder>().always_active)
            main_list.offsetMax = new Vector2(-vertical_max.rect.width, main_list.offsetMax.y);
    }

    public void UpdateOverlay()
    {
        foreach(IOverlay overlay in GetComponents<IOverlay>())
            overlay.UpdateOverlay();  
    }

    public void CloseOverlay()
    {
        horizontal_min.GetComponent<OverlayBorder>().Deactivate();
        horizontal_max.GetComponent<OverlayBorder>().Deactivate();
        vertical_min.GetComponent<OverlayBorder>().Deactivate();
        vertical_max.GetComponent<OverlayBorder>().Deactivate();
  
        foreach (IOverlay overlay in GetComponents<IOverlay>())
        {
            overlay.CloseOverlay();    
        }
    }  
}
