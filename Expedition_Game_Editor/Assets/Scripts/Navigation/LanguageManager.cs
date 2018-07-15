using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LanguageManager : MonoBehaviour
{
    static public int active_language;

    private string[] languages = new string[] {"English", "Dutch" };

    static public void GetLanguage()
    {
        active_language = 0;
    }

    public void SetLanguages()
    {
        Dropdown language_dropdown = GetComponent<EditorController>().actionManager.AddDropdown();

        language_dropdown.onValueChanged.RemoveAllListeners();

        language_dropdown.options.Clear();

        language_dropdown.captionText.text = languages[active_language];

        for (int i = 0; i < languages.Length; i++)
        {
            language_dropdown.options.Add(new Dropdown.OptionData(languages[i]));
        }

        language_dropdown.value = active_language;

        language_dropdown.onValueChanged.AddListener(delegate { SetLanguage(language_dropdown.value); });
    }

    static public void SetLanguage(int new_language)
    {
        active_language = new_language;

        ResetEditor();
    }

    static void ResetEditor()
    {
        EditorManager.editorManager.ResetEditor();
    }
}
