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
        Dropdown display_dropdown = ComponentManager.componentManager.AddDropdown(component);

        display_dropdown.onValueChanged.RemoveAllListeners();

        display_dropdown.options.Clear();

        display_dropdown.captionText.text = Enum.GetName(typeof(RegionDisplayManager.Display), RegionDisplayManager.active_display);

        foreach (var display in Enum.GetValues(typeof(RegionDisplayManager.Display)))
        {
            display_dropdown.options.Add(new Dropdown.OptionData(display.ToString()));
        }

        display_dropdown.value = (int)RegionDisplayManager.active_display;

        display_dropdown.onValueChanged.AddListener(delegate { RegionDisplayManager.SetDisplay(display_dropdown.value, pathController.route.path); });
    }

    public void OpenPath(Path path, IEnumerable data)
    {
        EditorManager.editorManager.OpenPath(PathManager.ReloadPath(path, data));
    }

    public void CloseComponent() { }
}
