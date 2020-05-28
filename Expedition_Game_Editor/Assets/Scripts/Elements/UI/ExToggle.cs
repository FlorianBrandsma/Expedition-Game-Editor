using UnityEngine;
using UnityEngine.UI;

public class ExToggle : MonoBehaviour, IEditorElement
{
    public Toggle Toggle { get { return GetComponent<Toggle>(); } }
    public ExElement EditorElement { get { return GetComponent<ExElement>(); } }
    
    public void EnableElement(bool enable)
    {
        Toggle.interactable = enable;

        Toggle.graphic.color = enable ? EditorElement.enabledColor : EditorElement.disabledColor;
    }
}
