using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class RegionDisplayComponent : MonoBehaviour, IComponent
{
    public EditorComponent component;

    private PathController controller;

    public void InitializeComponent(Path new_path)
    {
        controller = GetComponent<PathController>();
    }

    public void SetComponent(Path new_path)
    {
        Dropdown display_dropdown = ComponentManager.componentManager.AddDropdown(component);

        display_dropdown.onValueChanged.RemoveAllListeners();

        display_dropdown.options.Clear();

        display_dropdown.captionText.text = Enum.GetName(typeof(RegionDisplayManager.Display), RegionDisplayManager.active_display);

        foreach (var display in Enum.GetValues(typeof(RegionDisplayManager.Display)))
        {
            display_dropdown.options.Add(new Dropdown.OptionData(display.ToString()));
        }

        display_dropdown.value = (int)RegionDisplayManager.active_display;

        display_dropdown.onValueChanged.AddListener(delegate { RegionDisplayManager.SetDisplay(display_dropdown.value, controller.path); });
    }

    public void OpenPath(Path path, IEnumerable data)
    {
        EditorManager.editorManager.OpenPath(PathManager.ReloadPath(path, data));
    }

    public void CloseComponent() { }
}
