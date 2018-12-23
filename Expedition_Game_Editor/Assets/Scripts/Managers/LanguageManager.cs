using UnityEngine;
using System.Collections;

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

    static public void SetLanguage(int new_language)
    {
        active_language = (Language)new_language;

        EditorManager.editorManager.ResetEditor();
    }
}
