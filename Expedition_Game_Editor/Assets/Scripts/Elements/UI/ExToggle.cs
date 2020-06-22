﻿using UnityEngine;
using UnityEngine.UI;

public class ExToggle : MonoBehaviour, IEditorElement
{
    public Color enabledColor;
    public Color disabledColor;

    public Toggle Toggle { get { return GetComponent<Toggle>(); } }

    public void EnableElement(bool enable)
    {
        Toggle.interactable = enable;

        Toggle.graphic.color = enable ? enabledColor : disabledColor;
    }
}
