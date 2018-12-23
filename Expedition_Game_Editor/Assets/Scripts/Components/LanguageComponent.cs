﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LanguageComponent : MonoBehaviour
{
    private LanguageManager.Language language;

    public void SetLanguages()
    {
        Dropdown dropdown = GetComponent<EditorController>().componentManager.AddDropdown();

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
}
