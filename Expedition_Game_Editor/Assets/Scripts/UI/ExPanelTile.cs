﻿using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ExPanelTile : MonoBehaviour, IElement, IPoolable
{
    public Enums.ElementType elementType;

    public Text idText;
    public Text headerText;
    public RectTransform iconParent;
    public RawImage icon;
    public RectTransform content;
    public Image background;

    private string header;
    private string iconPath;

    private PanelTileProperties properties;

    public SelectionElement Element         { get { return GetComponent<SelectionElement>(); } }
    private SelectionElement ElementChild   { get { return Element.child; } }

    public Color ElementColor
    {
        set { background.color = value; }
    }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return elementType; } }
    public int Id                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    private Texture IconTexture
    {
        get { return icon.texture; }
        set
        {
            InitializeIcon();
            icon.texture = value;
        }
    }

    private SelectionElement.Data ChildButtonData
    {
        get { return ElementChild.data; }
        set
        {
            InitializeEdit();
            ElementChild.data = value;
        }
    }

    public IPoolable Instantiate()
    {
        var newElement = Instantiate(this);

        SelectionElementManager.Add(newElement.Element);

        return newElement;
    }

    public void InitializeElement()
    {
        properties = (PanelTileProperties)Element.DisplayManager.Display.Properties;
    }

    public void InitializeChildElement()
    {
        if (properties.childProperty != SelectionManager.Property.None)
            ChildButtonData = Element.data;
    }

    private void InitializeIcon()
    {
        content.offsetMin = new Vector2(iconParent.rect.width, content.offsetMin.y);
        iconParent.gameObject.SetActive(true);
    }

    private void InitializeEdit()
    {
        ElementChild.InitializeElement(Element.DisplayManager, ElementChild.selectionType, properties.childProperty);

        ElementChild.gameObject.SetActive(true);

        content.offsetMax = new Vector2(-ElementChild.GetComponent<RectTransform>().rect.width, content.offsetMax.y);
    }

    public void SetElement()
    {
        switch (Element.data.dataController.DataType)
        {
            case Enums.DataType.WorldInteractable:SetWorldInteractableElement();break;
            case Enums.DataType.WorldObject:      SetWorldObjectElement();      break;
            default: Debug.Log("CASE MISSING: " + Element.data.dataController.DataType); break;
        }
    }

    private void SetWorldInteractableElement()
    {
        var data = Element.data;
        WorldInteractableDataElement dataElement = (WorldInteractableDataElement)data.dataElement;

        idText.text = dataElement.Id.ToString();
        headerText.text = dataElement.interactableName;

        if (properties.icon)
            IconTexture = Resources.Load<Texture2D>(dataElement.objectGraphicIconPath);
    }

    private void SetWorldObjectElement()
    {
        var data = Element.data;
        WorldObjectDataElement dataElement = (WorldObjectDataElement)data.dataElement;

        idText.text = dataElement.Id.ToString();
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

    public void ClosePoolable()
    {
        //gameObject.SetActive(false);
    }
}