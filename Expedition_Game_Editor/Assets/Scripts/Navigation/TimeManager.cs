using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TimeManager : MonoBehaviour
{
    static public int active_time;
    static public int base_time;

    static public string[] times = { "Day", "Night" };

    static public void GetTimes()
    {
        active_time = 0;
    }

    public void SortTimes()
    {
        Dropdown time_dropdown = GetComponent<SubEditor>().actionManager.AddDropdown();

        time_dropdown.onValueChanged.RemoveAllListeners();

        time_dropdown.options.Clear();

        time_dropdown.captionText.text = times[active_time];

        for (int i = 0; i < times.Length; i++)
        {
            time_dropdown.options.Add(new Dropdown.OptionData(times[i]));
        }

        time_dropdown.value = active_time;

        time_dropdown.onValueChanged.AddListener(delegate { SetTime(time_dropdown.value); });

        SetBase();
    }

    void SetBase()
    {
        Button base_button = GetComponent<SubEditor>().actionManager.AddButton();

        base_button.GetComponentInChildren<Text>().text = "Import " + times[base_time];

        base_button.onClick.RemoveAllListeners();
    }

    static public void SetTime(int new_time)
    {
        active_time = new_time;

        ResetStructure();
    }

    static void ResetStructure()
    {
        NavigationManager.navigation_manager.RefreshStructure();
    }
}
