using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EditorButton : MonoBehaviour, IElement
{
    private SelectionElement Element { get { return GetComponent<SelectionElement>(); } }
    private ButtonProperties properties;

    public Text label;
    public RawImage icon;

    public Color ElementColor { set { } }

    public void InitializeElement()
    {
        //properties = element.ListManager.listProperties.GetComponent<ButtonProperties>();

        icon.texture = Resources.Load<Texture2D>("Textures/Icons/UI/" + Element.selectionProperty.ToString());
    }

    public void SetElement()
    {
        switch (Element.data.dataController.DataType)
        {
            case Enums.DataType.Item:           SetItemElement();           break;
            case Enums.DataType.ChapterRegion:  SetChapterRegionElement();  break;
            default: Debug.Log("CASE MISSING: " + Element.data.dataController.DataType); break;
        }
    }

    private void SetItemElement()
    {
        var data = (ItemDataElement)Element.data.dataElement;

        label.text = data.originalName;
    }

    private void SetChapterRegionElement()
    {
        var data = (ChapterRegionDataElement)Element.data.dataElement;

        label.text = data.name;
    }

    public void CloseElement() { }
}
