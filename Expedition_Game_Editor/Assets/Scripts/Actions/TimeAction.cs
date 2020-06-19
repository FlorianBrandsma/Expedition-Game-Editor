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
        var dropdown = ActionManager.instance.AddDropdown(actionProperties);

        dropdown.Dropdown.captionText.text = TimeManager.FormatTime(TimeManager.activeTime, true);

        for(int hour = 0; hour < TimeManager.hoursInDay; hour++)
        {
            dropdown.Dropdown.options.Add(new Dropdown.OptionData(TimeManager.FormatTime(hour, true)));
        }

        dropdown.Dropdown.value = TimeManager.activeTime;

        dropdown.Dropdown.onValueChanged.AddListener(delegate { SetTime(dropdown.Dropdown.value); });
    }
    
    private void SetTime(int time)
    {
        TimeManager.activeTime = time;
        
        if (regionNavigationAction.RegionType == Enums.RegionType.Interaction)
            regionNavigationAction.SelectOption(Enums.DataType.Interaction);
        
        TimeManager.instance.SetTime(time, true);
    }

    public void CloseAction() { }
}
