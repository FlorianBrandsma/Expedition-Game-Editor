using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class RegionDisplayAction : MonoBehaviour, IAction
{
    public ActionProperties actionProperties;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public void InitializeAction(Path path) { }

    public void SetAction(Path path)
    {
        RegionManager.activeDisplay = (RegionManager.Display)path.FindLastRoute(Enums.DataType.Region).controller;

        var dropdown = ActionManager.instance.AddDropdown(actionProperties);
        dropdown.Dropdown.captionText.text = Enum.GetName(typeof(RegionManager.Display), RegionManager.activeDisplay);

        foreach (var display in Enum.GetValues(typeof(RegionManager.Display)))
        {
            dropdown.Dropdown.options.Add(new Dropdown.OptionData(display.ToString()));
        }

        dropdown.Dropdown.value = (int)RegionManager.activeDisplay;

        dropdown.Dropdown.onValueChanged.AddListener(delegate { RegionManager.SetDisplay(dropdown.Dropdown.value, pathController.route.path); });
    }

    public void CloseAction() {}
}
