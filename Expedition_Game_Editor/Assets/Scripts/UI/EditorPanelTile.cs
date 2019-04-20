using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EditorPanelTile : MonoBehaviour, IElement
{
    private SelectionElement element { get { return GetComponent<SelectionElement>(); } }
    private PanelTileProperties properties;

    public Text id;
    public Text header;
    public RawImage icon;

    private SelectionElement edit_button { get { return element.child; } }

    public RectTransform content;

    public void InitializeElement()
    {
        properties = element.listManager.listProperties.GetComponent<PanelTileProperties>();

        if (properties.icon)
        {
            content.offsetMin = new Vector2(icon.rectTransform.rect.width, content.offsetMin.y);
            icon.gameObject.SetActive(true);
        }

        if (properties.edit)
        {
            edit_button.parent_element = element;

            edit_button.InitializeElement(element.listManager, SelectionManager.Property.Edit);

            edit_button.gameObject.SetActive(true);

            content.offsetMax = new Vector2(-edit_button.GetComponent<RectTransform>().rect.width, content.offsetMax.y);
        }
    }

    public void SetElement()
    {
        switch (element.route.data.controller.data_type)
        {
            case Enums.DataType.TerrainElement: SetTerrainElementElement(); break;
            case Enums.DataType.TerrainObject:  SetTerrainObjectElement();  break;
            default: Debug.Log("YOU ARE MISSING THE DATATYPE");             break;
        }
    }

    private void SetTerrainElementElement()
    {
        Data data = element.route.data;
        TerrainElementDataElement data_element = data.element.Cast<TerrainElementDataElement>().FirstOrDefault();

        id.text = data_element.id.ToString();
        header.text = data_element.original_name;

        if (properties.icon)
            icon.texture = Resources.Load<Texture2D>(data_element.icon);

        if (properties.edit)
            edit_button.route.data = data;
    }

    private void SetTerrainObjectElement()
    {
        Data data = element.route.data;
        TerrainObjectDataElement data_element = data.element.Cast<TerrainObjectDataElement>().FirstOrDefault();

        id.text = data_element.id.ToString();
        header.text = data_element.original_name;

        if (properties.icon)
            icon.texture = Resources.Load<Texture2D>(data_element.icon);

        if (properties.edit)
            edit_button.route.data = data;
    }

    public void CloseElement()
    {
        content.offsetMin = new Vector2(5, content.offsetMin.y);
        content.offsetMax = new Vector2(-5, content.offsetMax.y);

        header.text = string.Empty;
        id.text = string.Empty;

        if (properties.icon)
            icon.gameObject.SetActive(false);

        if (properties.edit)
            edit_button.gameObject.SetActive(false);
    }
}
