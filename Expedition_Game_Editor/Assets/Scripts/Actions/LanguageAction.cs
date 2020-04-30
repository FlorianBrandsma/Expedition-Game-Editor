using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LanguageAction : MonoBehaviour, IAction
{
    public ActionProperties actionProperties;

    private LanguageManager.Language language;

    private ExDropdown dropdown;

    public void InitializeAction(Path path)
    {

    }

    public void SetAction(Path path)
    {
        dropdown = ActionManager.instance.AddDropdown(actionProperties);

        dropdown.Dropdown.captionText.text = Enum.GetName(typeof(LanguageManager.Language), LanguageManager.active_language);

        foreach (var language in Enum.GetValues(typeof(LanguageManager.Language)))
        {
            dropdown.Dropdown.options.Add(new Dropdown.OptionData(language.ToString()));
        }

        dropdown.Dropdown.value = (int)LanguageManager.active_language;

        dropdown.Dropdown.onValueChanged.AddListener(delegate { LanguageManager.SetLanguage(dropdown.Dropdown.value); });
    }

    public void CloseAction() { }
}
