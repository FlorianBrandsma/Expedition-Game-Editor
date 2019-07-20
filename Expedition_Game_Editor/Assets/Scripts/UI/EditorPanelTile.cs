﻿using UnityEngine;
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
    private SelectionElement ElementChild { get { return Element.child; } }

    private Texture IconTexture
    {
        get { return icon.texture; }
        set
        {
            InitializeIcon();
            icon.texture = value;
        }
    }

    private Data ChildButtonData
    {
        get { return ElementChild.route.data; }
        set
        {
            InitializeEdit();
            ElementChild.route.data = value;
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
        ElementChild.InitializeElement(Element.ListManager, ElementChild.selectionType, properties.childProperty);

        ElementChild.gameObject.SetActive(true);

        content.offsetMax = new Vector2(-ElementChild.GetComponent<RectTransform>().rect.width, content.offsetMax.y);
    }

    public void SetElement()
    {
        switch (Element.route.data.DataController.DataType)
        {
            case Enums.DataType.TerrainInteractable:SetTerrainInteractableElement();break;
            case Enums.DataType.TerrainObject:      SetTerrainObjectElement();      break;
            default: Debug.Log("CASE MISSING: " + Element.route.data.DataController.DataType); break;
        }

        if (properties.childProperty != SelectionManager.Property.None)
            ChildButtonData = Element.route.data;
    }

    private void SetTerrainInteractableElement()
    {
        Data data = Element.route.data;
        TerrainInteractableDataElement dataElement = (TerrainInteractableDataElement)data.DataElement;

        idText.text = dataElement.id.ToString();
        headerText.text = dataElement.interactableName;

        if (properties.icon)
            IconTexture = Resources.Load<Texture2D>(dataElement.objectGraphicIconPath);
    }

    private void SetTerrainObjectElement()
    {
        Data data = Element.route.data;
        TerrainObjectDataElement dataElement = (TerrainObjectDataElement)data.DataElement;

        idText.text = dataElement.id.ToString();
        headerText.text = dataElement.objectGraphicName;

        if (properties.icon)
            IconTexture = Resources.Load<Texture2D>(dataElement.objectGraphicIconPath);
    }

    public void CloseElement()
    {
        content.offsetMin = new Vector2(5, content.offsetMin.y);
        content.offsetMax = new Vector2(-5, content.offsetMax.y);

        headerText.text = string.Empty;
        idText.text = string.Empty;

        if (properties.icon)
            iconParent.gameObject.SetActive(false);

        if (properties.childProperty != SelectionManager.Property.None)
            ElementChild.gameObject.SetActive(false);
    }
}
