using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EditorButton : MonoBehaviour, IElement
{
    private SelectionElement element { get { return GetComponent<SelectionElement>(); } }

    public Text label;
    public RawImage icon;

    public void InitializeElement()
    {
        SelectionElement element = GetComponent<SelectionElement>();
        icon.texture = Resources.Load<Texture2D>("Textures/Icons/" + element.selectionProperty.ToString());
    }

    public void SetElement()
    {
        //if(label != null)
        //    label.text = elementData.name;
    }

    public void CloseElement()
    {

    }
}
