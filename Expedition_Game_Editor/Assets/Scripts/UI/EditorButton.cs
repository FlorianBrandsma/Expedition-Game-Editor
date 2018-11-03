using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EditorButton : MonoBehaviour, IElement
{
    public Text label;
    public RawImage icon;

    public void SetElement()
    {
        SelectionElement element = GetComponent<SelectionElement>();

        icon.texture = Resources.Load<Texture2D>("Textures/Icons/" + element.selectionProperty.ToString());
    }
}
