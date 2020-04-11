using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TimeAction : MonoBehaviour, IAction
{
    public ActionProperties actionProperties;

    public void InitializeAction(Path path)
    {
        EditorManager.editorManager.TimeManager.SetLighting();
    }

    public void SetAction(Path path)
    {
        var dropdown = ActionManager.actionManager.AddDropdown(actionProperties);

        dropdown.captionText.text = TimeManager.FormatTime(TimeManager.activeTime);

        for(int hour = 0; hour < TimeManager.hoursInDay; hour++)
        {
            dropdown.options.Add(new Dropdown.OptionData(TimeManager.FormatTime(hour)));
        }

        dropdown.value = (int)TimeManager.activeTime;

        dropdown.onValueChanged.AddListener(delegate { EditorManager.editorManager.TimeManager.SetTime(dropdown.value); });
    }
    
    public void CloseAction() { }
}
