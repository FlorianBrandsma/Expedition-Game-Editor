using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class RegionDisplayComponent : MonoBehaviour, IComponent
{
    public EditorComponent component;
    Dropdown dropdown;
    private PathController pathController { get { return GetComponent<PathController>(); } }

    public void InitializeComponent(Path path) { }

    public void SetComponent(Path path)
    {
        dropdown = ComponentManager.componentManager.AddDropdown(component);

        dropdown.captionText.text = Enum.GetName(typeof(RegionDisplayManager.Display), RegionDisplayManager.active_display);

        foreach (var display in Enum.GetValues(typeof(RegionDisplayManager.Display)))
        {
            dropdown.options.Add(new Dropdown.OptionData(display.ToString()));
        }

        dropdown.value = (int)RegionDisplayManager.active_display;

        dropdown.onValueChanged.AddListener(delegate { RegionDisplayManager.SetDisplay(dropdown.value, pathController.route.path); });
    }

    public void InitializePath(Path path, IEnumerable data)
    {
        EditorManager.editorManager.InitializePath(PathManager.ReloadPath(path, data));
    }

    public void CloseComponent() {}
}
