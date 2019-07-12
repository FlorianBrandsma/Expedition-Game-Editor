using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EditorPanelTile : MonoBehaviour, IElement
{
    public Text idText;
    public Text headerText;
    public RectTransform iconParent;
    public RawImage icon;
    public RectTransform content;

    private string header;
    private string description;
    private string iconPath;

    private PanelTileProperties properties;

    private SelectionElement Element { get { return GetComponent<SelectionElement>(); } }
    private SelectionElement EditButton { get { return Element.child; } }

    private Texture IconTexture
    {
        get { return icon.texture; }
        set
        {
            InitializeIcon();
            icon.texture = value;
        }
    }

    private Data EditButtonData
    {
        get { return EditButton.route.data; }
        set
        {
            InitializeEdit();
            EditButton.route.data = value;
        }
    }

    public void InitializeElement()
    {
        properties = Element.ListManager.listProperties.GetComponent<PanelTileProperties>();
    }

    private void InitializeIcon()
    {
        content.offsetMin = new Vector2(iconParent.rect.width, content.offsetMin.y);
        iconParent.gameObject.SetActive(true);
    }

    private void InitializeEdit()
    {
        EditButton.InitializeElement(Element.ListManager, EditButton.selectionType, EditButton.selectionProperty);

        EditButton.gameObject.SetActive(true);

        content.offsetMax = new Vector2(-EditButton.GetComponent<RectTransform>().rect.width, content.offsetMax.y);
    }

    public void SetElement()
    {
        switch (Element.route.data.DataController.DataType)
        {
            case Enums.DataType.TerrainInteractable:SetTerrainInteractableElement();break;
            case Enums.DataType.TerrainObject:      SetTerrainObjectElement();      break;
            default: Debug.Log("CASE MISSING: " + Element.route.data.DataController.DataType); break;
        }
    }

    private void SetTerrainInteractableElement()
    {
        Data data = Element.route.data;
        TerrainInteractableDataElement dataElement = (TerrainInteractableDataElement)data.DataElement;

        idText.text = dataElement.id.ToString();
        headerText.text = dataElement.interactableName;

        if (properties.icon)
            IconTexture = Resources.Load<Texture2D>(dataElement.objectGraphicIconPath);

        if (properties.edit)
            EditButtonData = data;
    }

    private void SetTerrainObjectElement()
    {
        Data data = Element.route.data;
        TerrainObjectDataElement dataElement = (TerrainObjectDataElement)data.DataElement;

        idText.text = dataElement.id.ToString();
        headerText.text = dataElement.name;

        if (properties.icon)
            IconTexture = Resources.Load<Texture2D>(dataElement.icon);

        if (properties.edit)
            EditButtonData = data;
    }

    public void CloseElement()
    {
        content.offsetMin = new Vector2(5, content.offsetMin.y);
        content.offsetMax = new Vector2(-5, content.offsetMax.y);

        headerText.text = string.Empty;
        idText.text = string.Empty;

        if (properties.icon)
            iconParent.gameObject.SetActive(false);

        if (properties.edit)
            EditButton.gameObject.SetActive(false);
    }
}
