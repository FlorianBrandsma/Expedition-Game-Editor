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

    public void InitializeAction(Path path) { }

    public void SetAction(Path path)
    {
        var dropdown = ActionManager.instance.AddDropdown(actionProperties);

        dropdown.Dropdown.captionText.text = TimeManager.FormatTime(TimeManager.instance.ActiveTime);

        for(int hour = 0; hour < TimeManager.hoursInDay; hour++)
        {
            dropdown.Dropdown.options.Add(new Dropdown.OptionData(TimeManager.FormatTime(hour * TimeManager.secondsInHour)));
        }

        dropdown.Dropdown.value = Mathf.FloorToInt(TimeManager.instance.ActiveTime / TimeManager.secondsInHour);

        dropdown.Dropdown.onValueChanged.AddListener(delegate { SetTime(dropdown.Dropdown.value); });
    }
    
    private void SetTime(int selectedHour)
    {
        //Time needs to be set here so it can be used to select the interaction
        TimeManager.instance.ActiveTime = selectedHour * TimeManager.secondsInHour;
        
        //There might be an underlying issue with resetting the editor which should be addressed
        //if time-changing related issues continue to arise
        if (RegionDisplayManager.regionType == Enums.RegionType.Interaction)
            regionNavigationAction.SelectInteraction();
        
        TimeManager.instance.SetEditorTime(selectedHour * TimeManager.secondsInHour, RegionDisplayManager.regionType != Enums.RegionType.Interaction);
    }

    public void CloseAction() { }
}
