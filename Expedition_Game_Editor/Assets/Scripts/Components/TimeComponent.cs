using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TimeComponent : MonoBehaviour
{
    public void SetTimes()
    {
        Dropdown dropdown = GetComponent<EditorController>().componentManager.AddDropdown();

        dropdown.onValueChanged.RemoveAllListeners();

        dropdown.options.Clear();

        dropdown.captionText.text = Enum.GetName(typeof(TimeManager.Time), TimeManager.active_time);

        foreach (var time in Enum.GetValues(typeof(TimeManager.Time)))
        {
            dropdown.options.Add(new Dropdown.OptionData(time.ToString()));
        }

        dropdown.value = (int)TimeManager.active_time;

        dropdown.onValueChanged.AddListener(delegate { TimeManager.SetTime(dropdown.value); });
    }
}
