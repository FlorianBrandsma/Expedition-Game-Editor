using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class LanguageManager : MonoBehaviour
{
    public void SetLanguages()
    {
        Dropdown language_dropdown = GetComponent<OptionManager>().AddDropdown();
    }
}
