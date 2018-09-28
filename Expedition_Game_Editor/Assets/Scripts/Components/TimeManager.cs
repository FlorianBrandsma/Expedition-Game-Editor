using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TimeManager : MonoBehaviour
{
    public enum Time
    {
        Day,
        Night,
    }

    static public Time default_time = Time.Day;
    static public Time active_time;

    static public void GetTimes()
    {
        active_time = default_time;
    }

    public void SetTimes()
    {
        Dropdown dropdown = GetComponent<EditorController>().actionManager.AddDropdown();

        dropdown.onValueChanged.RemoveAllListeners();

        dropdown.options.Clear();

        dropdown.captionText.text = Enum.GetName(typeof(Time), active_time);

        foreach (var time in Enum.GetValues(typeof(Time)))
        {
            dropdown.options.Add(new Dropdown.OptionData(time.ToString()));
        }

        dropdown.value = (int)active_time;

        dropdown.onValueChanged.AddListener(delegate { SetTime(dropdown.value); });

        SetBase();
    }

    void SetBase()
    {
        Button base_button = GetComponent<EditorController>().actionManager.AddButton();

        base_button.GetComponentInChildren<Text>().text = "Import " + Enum.GetName(typeof(Time), default_time);

        base_button.onClick.RemoveAllListeners();
    }

    static public void SetTime(int new_time)
    {
        active_time = (Time)new_time;

        ResetEditor();
    }

    static void ResetEditor()
    {
        EditorManager.editorManager.ResetEditor();
    }
}
