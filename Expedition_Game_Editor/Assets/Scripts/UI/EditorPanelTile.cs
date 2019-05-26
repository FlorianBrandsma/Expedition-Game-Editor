﻿using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EditorPanelTile : MonoBehaviour, IElement
{
    public Text id;
    public Text header;
    public RawImage icon;
    public RectTransform content;

    private PanelTileProperties properties;

    private SelectionElement Element    { get { return GetComponent<SelectionElement>(); } }
    private SelectionElement EditButton { get { return Element.child; } }

    public void InitializeElement()
    {
        properties = Element.ListManager.listProperties.GetComponent<PanelTileProperties>();

        if (properties.icon)
        {
            content.offsetMin = new Vector2(icon.rectTransform.rect.width, content.offsetMin.y);
            icon.gameObject.SetActive(true);
        }

        if (properties.edit)
        {
            EditButton.InitializeElement(Element.ListManager, EditButton.selectionProperty);

            EditButton.gameObject.SetActive(true);

            content.offsetMax = new Vector2(-EditButton.GetComponent<RectTransform>().rect.width, content.offsetMax.y);
        }
    }

    public void SetElement()
    {
        switch (Element.route.data.DataController.DataType)
        {
            case Enums.DataType.TerrainElement: SetTerrainElementElement(); break;
            case Enums.DataType.TerrainObject:  SetTerrainObjectElement();  break;
            default: Debug.Log("YOU ARE MISSING THE DATATYPE");             break;
        }
    }

    private void SetTerrainElementElement()
    {
        Data data = Element.route.data;
        TerrainElementDataElement dataElement = data.ElementData.Cast<TerrainElementDataElement>().FirstOrDefault();

        id.text = dataElement.id.ToString();
        header.text = dataElement.name;

        if (properties.icon)
            icon.texture = Resources.Load<Texture2D>(dataElement.objectGraphicIcon);

        if (properties.edit)
            EditButton.route.data = data;
    }

    private void SetTerrainObjectElement()
    {
        Data data = Element.route.data;
        TerrainObjectDataElement dataElement = data.ElementData.Cast<TerrainObjectDataElement>().FirstOrDefault();

        id.text = dataElement.id.ToString();
        header.text = dataElement.name;

        if (properties.icon)
            icon.texture = Resources.Load<Texture2D>(dataElement.icon);

        if (properties.edit)
            EditButton.route.data = data;
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
            EditButton.gameObject.SetActive(false);
    }
}
