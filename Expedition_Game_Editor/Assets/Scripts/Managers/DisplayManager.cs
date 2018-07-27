using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DisplayManager : MonoBehaviour
{
    static public int active_display;

    private string[] displays = System.Enum.GetNames(typeof(Enums.DisplayMode));

    static public void GetDisplay()
    {
        active_display = 0;
    }

    public void InitializeDisplay()
    {
        Dropdown display_dropdown = GetComponent<EditorController>().actionManager.AddDropdown();

        display_dropdown.onValueChanged.RemoveAllListeners();

        display_dropdown.options.Clear();

        display_dropdown.captionText.text = displays[active_display];

        foreach(string display in displays)
        {
            display_dropdown.options.Add(new Dropdown.OptionData(display));
        }

        display_dropdown.value = active_display;

        display_dropdown.onValueChanged.AddListener(delegate { SetDisplay(display_dropdown.value); });
    }

    static public void SetDisplay(int new_display)
    {
        active_display = new_display;
        Debug.Log("Activate " + new_display);

        //ResetEditor();
    }
}
