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
        RegionDisplayManager.activeDisplay = (RegionDisplayManager.Display)path.FindLastRoute(Enums.DataType.Region).controller;

        var dropdown = ActionManager.instance.AddDropdown(actionProperties);
        dropdown.Dropdown.captionText.text = Enum.GetName(typeof(RegionDisplayManager.Display), RegionDisplayManager.activeDisplay);

        foreach (var display in Enum.GetValues(typeof(RegionDisplayManager.Display)))
        {
            dropdown.Dropdown.options.Add(new Dropdown.OptionData(display.ToString()));
        }

        dropdown.Dropdown.value = (int)RegionDisplayManager.activeDisplay;

        dropdown.Dropdown.onValueChanged.AddListener(delegate { RegionDisplayManager.SetDisplay(dropdown.Dropdown.value, pathController.route.path); });
    }

    public void CloseAction() {}
}
