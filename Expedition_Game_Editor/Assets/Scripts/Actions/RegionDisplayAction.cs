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
        Dropdown dropdown = ActionManager.actionManager.AddDropdown(actionProperties);

        RegionDisplayManager.activeDisplay = (RegionDisplayManager.Display)path.FindLastRoute(Enums.DataType.Region).controller;

        dropdown.captionText.text = Enum.GetName(typeof(RegionDisplayManager.Display), RegionDisplayManager.activeDisplay);

        foreach (var display in Enum.GetValues(typeof(RegionDisplayManager.Display)))
        {
            dropdown.options.Add(new Dropdown.OptionData(display.ToString()));
        }

        dropdown.value = (int)RegionDisplayManager.activeDisplay;

        dropdown.onValueChanged.AddListener(delegate { RegionDisplayManager.SetDisplay(dropdown.value, pathController.route.path); });
    }

    public void CloseAction() {}
}
