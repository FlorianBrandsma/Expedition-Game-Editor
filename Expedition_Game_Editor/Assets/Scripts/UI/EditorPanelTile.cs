using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EditorPanelTile : MonoBehaviour, IElement
{
    private SelectionElement element;
    private PanelTileProperties properties;

    public Text id;
    public Text header;
    public RawImage icon;

    public SelectionElement edit_button;

    public RectTransform content;

    public void InitializeElement()
    {
        element = GetComponent<SelectionElement>();
        properties = element.listManager.listProperties.GetComponent<PanelTileProperties>();

        if (properties.icon)
        {
            content.offsetMin = new Vector2(icon.rectTransform.rect.width, content.offsetMin.y);
            icon.gameObject.SetActive(true);
        }
        if (properties.edit)
        {
            edit_button.gameObject.SetActive(true);
            content.offsetMax = new Vector2(-edit_button.GetComponent<RectTransform>().rect.width, content.offsetMax.y);
        }
    }

    public void SetElement()
    {
        
    }

    //public void SetElement(IEnumerable data)
    //{
    //    id.text = elementData.id.ToString();
    //    header.text = elementData.name;

    //    if(properties.icon)
    //        icon.texture = Resources.Load<Texture2D>(elementData.icon);

    //    if (properties.edit)
    //    {
    //        GeneralData edit_data = properties.edit_data.Copy();
    //        edit_data.id = elementData.id;
    //        edit_button.InitializeElement(element.listManager, SelectionManager.Property.Edit);

    //        //edit_button.SetElement(elementData, data);
    //    }
    //}

    public void CloseElement()
    {
        content.offsetMin = new Vector2(5, content.offsetMin.y);
        content.offsetMax = new Vector2(-5, content.offsetMax.y);

        if (properties.icon)
            icon.gameObject.SetActive(false);

        if (properties.edit)
            edit_button.gameObject.SetActive(false);
    }
}
