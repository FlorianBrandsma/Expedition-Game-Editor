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

    Dropdown dropdown;

    public void InitializeAction(Path path)
    {

    }

    public void SetAction(Path path)
    {
        dropdown = ActionManager.actionManager.AddDropdown(actionProperties);

        dropdown.captionText.text = Enum.GetName(typeof(LanguageManager.Language), LanguageManager.active_language);

        foreach (var language in Enum.GetValues(typeof(LanguageManager.Language)))
        {
            dropdown.options.Add(new Dropdown.OptionData(language.ToString()));
        }

        dropdown.value = (int)LanguageManager.active_language;

        dropdown.onValueChanged.AddListener(delegate { LanguageManager.SetLanguage(dropdown.value); });
    }

    public void CloseAction() { }
}
