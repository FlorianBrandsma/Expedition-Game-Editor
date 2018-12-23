using UnityEngine;
using UnityEngine.UI;
using System;

public class DisplayComponent : MonoBehaviour
{
    public void InitializeDisplay()
    {
        Dropdown display_dropdown = GetComponent<EditorController>().componentManager.AddDropdown();

        display_dropdown.onValueChanged.RemoveAllListeners();

        display_dropdown.options.Clear();

        display_dropdown.captionText.text = Enum.GetName(typeof(DisplayManager.Display), DisplayManager.active_display);

        foreach (var display in Enum.GetValues(typeof(DisplayManager.Display)))
        {
            display_dropdown.options.Add(new Dropdown.OptionData(display.ToString()));
        }

        display_dropdown.value = (int)DisplayManager.active_display;

        display_dropdown.onValueChanged.AddListener(delegate { DisplayManager.SetDisplay(display_dropdown.value); });
    }
}
