using UnityEngine;
using UnityEngine.UI;

public class EditorDropdown : MonoBehaviour, IEditorElement
{
    public Dropdown Dropdown { get { return GetComponent<Dropdown>(); } }
    public EditorElement EditorElement { get { return GetComponent<EditorElement>(); } }

    public void EnableElement(bool enable)
    {
        Dropdown.interactable = enable;
    }
}
