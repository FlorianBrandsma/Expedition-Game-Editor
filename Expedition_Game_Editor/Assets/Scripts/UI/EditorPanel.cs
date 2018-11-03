using UnityEngine;
using UnityEngine.UI;

public class EditorPanel : MonoBehaviour, IElement
{
    public Text id;
    public Text header;
    public Text content;

    public void SetElement()
    {
        SelectionElement element = GetComponent<SelectionElement>();

        id.text = element.data.id.ToString();
    }
}
