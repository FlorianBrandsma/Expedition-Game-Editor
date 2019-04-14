using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EditorButton : MonoBehaviour, IElement
{
    private SelectionElement element { get { return GetComponent<SelectionElement>(); } }
    private ButtonProperties properties;

    public Text label;
    public RawImage icon;

    public void InitializeElement()
    {
        properties = element.listManager.listProperties.GetComponent<ButtonProperties>();

        icon.texture = Resources.Load<Texture2D>("Textures/Icons/" + element.selectionProperty.ToString());
    }

    public void SetElement()
    {
        switch (element.data_type)
        {
            case Enums.DataType.Item:
                SetItemElement();
                break;
            case Enums.DataType.Element:
                //SetPhaseElement();
                break;
        }
    }

    private void SetItemElement()
    {
        ItemDataElement data = element.data.Cast<ItemDataElement>().FirstOrDefault();

        label.text = data.original_name;
    }

    public void CloseElement()
    {

    }
}
