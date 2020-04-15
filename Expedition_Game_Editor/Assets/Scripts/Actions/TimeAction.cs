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

    //Can make "IAction linkedAction" if that would be useful elsewhere
    public RegionNavigationAction regionNavigationAction;

    public void InitializeAction(Path path)
    {
        TimeManager.instance.SetLighting();
    }

    public void SetAction(Path path)
    {
        var dropdown = ActionManager.actionManager.AddDropdown(actionProperties);

        dropdown.captionText.text = TimeManager.FormatTime(TimeManager.activeTime, true);

        for(int hour = 0; hour < TimeManager.hoursInDay; hour++)
        {
            dropdown.options.Add(new Dropdown.OptionData(TimeManager.FormatTime(hour, true)));
        }

        dropdown.value = (int)TimeManager.activeTime;

        dropdown.onValueChanged.AddListener(delegate { SetTime(dropdown.value); });
    }
    
    private void SetTime(int time)
    {
        TimeManager.activeTime = time;

        if (regionNavigationAction.active)
            regionNavigationAction.SelectOption(Enums.DataType.Interaction);

        TimeManager.instance.SetTime(time, true);
    }

    public void CloseAction() { }
}
