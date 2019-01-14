using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LanguageComponent : MonoBehaviour, IComponent
{
    public EditorComponent component;

    private LanguageManager.Language language;

    public void SetComponent(Path new_path)
    {
        Dropdown dropdown = ComponentManager.componentManager.AddDropdown(component);

        dropdown.onValueChanged.RemoveAllListeners();

        dropdown.options.Clear();

        dropdown.captionText.text = Enum.GetName(typeof(LanguageManager.Language), LanguageManager.active_language);

        foreach (var language in Enum.GetValues(typeof(LanguageManager.Language)))
        {
            dropdown.options.Add(new Dropdown.OptionData(language.ToString()));
        }

        dropdown.value = (int)LanguageManager.active_language;

        dropdown.onValueChanged.AddListener(delegate { LanguageManager.SetLanguage(dropdown.value); });
    }

    public void CloseComponent() { }
}
