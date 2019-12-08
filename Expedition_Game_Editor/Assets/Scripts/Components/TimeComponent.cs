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

    Dropdown dropdown;

    public void InitializeComponent(Path path)
    {
        EditorManager.editorManager.TimeManager.SetLighting();
    }

    public void SetComponent(Path path)
    {
        dropdown = ComponentManager.componentManager.AddDropdown(component);

        dropdown.captionText.text = Enum.GetName(typeof(TimeManager.Time), TimeManager.activeTime);

        foreach (var time in Enum.GetValues(typeof(TimeManager.Time)))
        {
            dropdown.options.Add(new Dropdown.OptionData(time.ToString()));
        }

        dropdown.value = (int)TimeManager.activeTime;

        dropdown.onValueChanged.AddListener(delegate { EditorManager.editorManager.TimeManager.SetTime((TimeManager.Time)dropdown.value); });

        SetImportComponent();
    }
    
    private void SetImportComponent()
    {
        Button button = ComponentManager.componentManager.AddButton(import_component);

        button.onClick.RemoveAllListeners();

        int import_time = (TimeManager.activeTime == 0 ? 1 : 0);

        button.GetComponentInChildren<Text>().text = "Import " + Enum.GetName(typeof(TimeManager.Time), import_time);
    }

    public void CloseComponent() { }
}
