using UnityEngine;
using UnityEngine.UI;

public class EditorPanel : MonoBehaviour, IElement
{
    private PanelProperties properties;

    public Text id;
    public Text header;
    public Text description;
    public RawImage icon;

    public SelectionElement edit_button;

    public RectTransform content;

    public void SetElement()
    {
        SelectionElement element = GetComponent<SelectionElement>();
        properties = element.listManager.listData.listProperties.GetComponent<PanelProperties>();

        if(properties.icon)
        {
            content.offsetMin = new Vector2(icon.rectTransform.rect.width, content.offsetMin.y);
            icon.gameObject.SetActive(true);
        }

        if (properties.edit)
        {
            edit_button.gameObject.SetActive(true);
            content.offsetMax = new Vector2(-edit_button.GetComponent<RectTransform>().rect.width, content.offsetMax.y);
        }

        id.text = element.data.id.ToString();
    }

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
