using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LanguageManager : MonoBehaviour
{
    public enum Language
    {
        English,
        Dutch,
    }

    static public Language default_language = Language.English;
    static public Language active_language;

    static public void GetLanguage()
    {
        active_language = default_language;
    }

    public void SetLanguages()
    {
        Dropdown dropdown = GetComponent<EditorController>().actionManager.AddDropdown();

        dropdown.onValueChanged.RemoveAllListeners();

        dropdown.options.Clear();

        dropdown.captionText.text = Enum.GetName(typeof(Language), active_language);

        foreach (var language in Enum.GetValues(typeof(Language)))
        {
            dropdown.options.Add(new Dropdown.OptionData(language.ToString()));
        }

        dropdown.value = (int)active_language;

        dropdown.onValueChanged.AddListener(delegate { SetLanguage(dropdown.value); });
    }

    static public void SetLanguage(int new_language)
    {
        active_language = (Language)new_language;

        ResetEditor();
    }

    static void ResetEditor()
    {
        EditorManager.editorManager.ResetEditor();
    }
}
