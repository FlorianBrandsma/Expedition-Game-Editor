﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ButtonActionManager : MonoBehaviour
{
    public Button   apply_button,
                    close_button;

    public Toggle   trash_toggle;

    public void InitializeButtons(LayoutSection section)
    {
        if (apply_button != null)
            apply_button.onClick.AddListener(delegate { section.ApplyChanges(); });
        
        if(close_button != null)
            close_button.onClick.AddListener(delegate { section.CloseEditor(); });    
    }

    public void SetButtons(bool changed)
    {
        if (apply_button != null)
            apply_button.interactable = changed;
    }

    public void CloseButtons()
    {
        if(trash_toggle != null)
            trash_toggle.isOn = false;
    }
}
