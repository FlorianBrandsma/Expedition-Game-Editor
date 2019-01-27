using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TimeComponent : MonoBehaviour, IComponent
{
    public EditorComponent component;
    public EditorComponent import_component;

    public void InitializeComponent(Path new_path)
    {

    }

    public void SetComponent(Path new_path)
    {
        Dropdown dropdown = ComponentManager.componentManager.AddDropdown(component);

        dropdown.onValueChanged.RemoveAllListeners();

        dropdown.options.Clear();

        dropdown.captionText.text = Enum.GetName(typeof(TimeManager.Time), TimeManager.active_time);

        foreach (var time in Enum.GetValues(typeof(TimeManager.Time)))
        {
            dropdown.options.Add(new Dropdown.OptionData(time.ToString()));
        }

        dropdown.value = (int)TimeManager.active_time;

        dropdown.onValueChanged.AddListener(delegate { TimeManager.SetTime(dropdown.value); });

        SetImportComponent();
    }
    
    private void SetImportComponent()
    {
        Button button = ComponentManager.componentManager.AddButton(import_component);

        button.onClick.RemoveAllListeners();

        int import_time = (TimeManager.active_time == 0 ? 1 : 0);

        button.GetComponentInChildren<Text>().text = "Import " + Enum.GetName(typeof(TimeManager.Time), import_time);
    }

    public void CloseComponent() { }
}
