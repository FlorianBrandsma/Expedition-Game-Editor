using UnityEngine;
using UnityEngine.UI;

public class EditorToggle : MonoBehaviour, IEditorElement
{
    public Toggle Toggle { get { return GetComponent<Toggle>(); } }
    public EditorElement EditorElement { get { return GetComponent<EditorElement>(); } }
    
    public void EnableElement(bool enable)
    {
        Toggle.interactable = enable;

        Toggle.graphic.color = enable ? EditorElement.enabledColor : EditorElement.disabledColor;
    }
}
