using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DisplayManager : MonoBehaviour
{
    public enum Display
    {
        List,
        Diagram,
    }

    static public Display default_display = Display.List;
    static public Display active_display;

    static public void GetDisplay()
    {
        active_display = 0;
    }

    public void InitializeDisplay()
    {
        Dropdown display_dropdown = GetComponent<EditorController>().actionManager.AddDropdown();

        display_dropdown.onValueChanged.RemoveAllListeners();

        display_dropdown.options.Clear();

        display_dropdown.captionText.text = Enum.GetName(typeof(Display), active_display);

        foreach (var display in Enum.GetValues(typeof(Display)))
        {
            display_dropdown.options.Add(new Dropdown.OptionData(display.ToString()));
        }

        display_dropdown.value = (int)active_display;

        display_dropdown.onValueChanged.AddListener(delegate { SetDisplay(display_dropdown.value); });
    }

    static public void SetDisplay(int new_display)
    {
        active_display = (Display)new_display;
        Debug.Log("Activate " + new_display);

        //ResetEditor();
    }
}
