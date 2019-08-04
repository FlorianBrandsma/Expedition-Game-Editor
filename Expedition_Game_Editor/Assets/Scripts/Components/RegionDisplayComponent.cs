using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class RegionDisplayComponent : MonoBehaviour, IComponent
{
    public EditorComponent component;

    private PathController pathController { get { return GetComponent<PathController>(); } }

    public void InitializeComponent(Path path) { }

    public void SetComponent(Path path)
    {
        Dropdown dropdown = ComponentManager.componentManager.AddDropdown(component);

        RegionDisplayManager.activeDisplay = (RegionDisplayManager.Display)path.FindLastRoute(Enums.DataType.Region).controller;

        dropdown.captionText.text = Enum.GetName(typeof(RegionDisplayManager.Display), RegionDisplayManager.activeDisplay);

        foreach (var display in Enum.GetValues(typeof(RegionDisplayManager.Display)))
        {
            dropdown.options.Add(new Dropdown.OptionData(display.ToString()));
        }

        dropdown.value = (int)RegionDisplayManager.activeDisplay;

        dropdown.onValueChanged.AddListener(delegate { RegionDisplayManager.SetDisplay(dropdown.value, pathController.route.path); });
    }

    public void CloseComponent() {}
}
