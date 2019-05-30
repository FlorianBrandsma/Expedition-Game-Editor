﻿using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EditorPanel : MonoBehaviour, IElement
{
    public Text idText;
    public Text headerText;
    public Text descriptionText;
    public RawImage icon;
    public RectTransform content;

    private string header;
    private string description;
    private string iconPath;

    private PanelProperties properties;

    private SelectionElement Element    { get { return GetComponent<SelectionElement>(); } }
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
        properties = Element.ListManager.listProperties.GetComponent<PanelProperties>();
    }

    private void InitializeIcon()
    {
        content.offsetMin = new Vector2(icon.rectTransform.rect.width, content.offsetMin.y);
        icon.gameObject.SetActive(true);
    }

    private void InitializeEdit()
    {
        EditButton.InitializeElement(Element.ListManager, EditButton.selectionProperty);

        EditButton.gameObject.SetActive(true);

        content.offsetMax = new Vector2(-EditButton.GetComponent<RectTransform>().rect.width, content.offsetMax.y);
    }

    public void SetElement()
    {
        switch (Element.route.data.DataController.DataType)
        {
            case Enums.DataType.Chapter:        SetChapterElement();        break;
            case Enums.DataType.Phase:          SetPhaseElement();          break;
            case Enums.DataType.PhaseRegion:    SetPhaseRegionElement();    break;
            case Enums.DataType.Quest:          SetQuestElement();          break;
            case Enums.DataType.Objective:      SetObjectiveElement();      break;
            case Enums.DataType.Task:           SetTaskElement();           break;
            case Enums.DataType.Region:         SetRegionElement();         break;
            case Enums.DataType.ObjectGraphic:  SetObjectGraphicElement();  break;
            case Enums.DataType.Item:           SetItemElement();           break;
            case Enums.DataType.Element:        SetElementElement();        break;
            case Enums.DataType.TerrainElement: SetTerrainElementElement(); break;
            default:                            Debug.Log("CASE MISSING");  break;
        }

        if (descriptionText == null) return;

        if (headerText.text == string.Empty)
            descriptionText.rectTransform.offsetMax = new Vector2(descriptionText.rectTransform.offsetMax.x, 0);
        else
            descriptionText.rectTransform.offsetMax = new Vector2(descriptionText.rectTransform.offsetMax.x, -headerText.rectTransform.rect.height);
    }

    private void SetChapterElement()
    {
        Data data = Element.route.data;
        var dataElement = (ChapterDataElement)data.DataElement;

        if(Element.selectionProperty == SelectionManager.Property.Get)
        {
            header              = dataElement.Name;
            description         = dataElement.Notes;

        } else {

            header              = dataElement.originalName;
            description         = dataElement.originalNotes;
        }

        idText.text             = dataElement.id.ToString();
        headerText.text         = header;
        descriptionText.text    = description;

        if (properties.edit)
            EditButtonData = data;
    }

    private void SetPhaseElement()
    {
        Data data = Element.route.data;
        var dataElement = (PhaseDataElement)data.DataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header              = dataElement.Name;
            description         = dataElement.Notes;

        } else {

            header              = dataElement.originalName;
            description         = dataElement.originalNotes;
        }

        idText.text             = dataElement.id.ToString();
        headerText.text         = header;
        descriptionText.text    = description;

        if (properties.edit)
            EditButtonData = data;
    }

    private void SetPhaseRegionElement()
    {
        Data data = Element.route.data;
        var dataElement = (PhaseRegionDataElement)data.DataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.Name;

        } else {

            header      = dataElement.originalName;
        }

        idText.text     = dataElement.id.ToString();
        headerText.text = header;
    }

    private void SetQuestElement()
    {
        Data data = Element.route.data;
        var dataElement = (QuestDataElement)data.DataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header              = dataElement.Name;
            description         = dataElement.Notes;

        } else {

            header              = dataElement.originalName;
            description         = dataElement.originalNotes;
        }

        idText.text             = dataElement.id.ToString();
        headerText.text         = header;
        descriptionText.text    = description;

        if (properties.edit)
            EditButtonData = data;
    }

    private void SetObjectiveElement()
    {
        Data data = Element.route.data;
        var dataElement = (ObjectiveDataElement)data.DataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header              = dataElement.Name;
            description         = dataElement.Notes;

        } else {

            header              = dataElement.originalName;
            description         = dataElement.originalNotes;
        }

        idText.text             = dataElement.id.ToString();
        headerText.text         = header;
        descriptionText.text    = description;

        if (properties.edit)
            EditButtonData = data;
    }

    private void SetTerrainElementElement()
    {
        Data data = Element.route.data;
        var dataElement = (TerrainElementDataElement)data.DataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.elementName;
            iconPath    = dataElement.objectGraphicIcon;

        } else {

            header      = dataElement.originalElementName;
            iconPath    = dataElement.originalObjectGraphicIcon;
        }

        idText.text     = dataElement.id.ToString();
        headerText.text = header;

        if (properties.icon)
            IconTexture = Resources.Load<Texture2D>(iconPath);

        if (properties.edit)
            EditButtonData = data;
    }

    private void SetTaskElement()
    {
        Data data = Element.route.data;
        var dataElement = (TaskDataElement)data.DataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            description         = dataElement.Description;

        } else {

            description         = dataElement.originalDescription;
        }

        idText.text             = dataElement.id.ToString();
        descriptionText.text    = description;

        if (properties.edit)
            EditButtonData = data;
    }

    private void SetRegionElement()
    {
        Data data = Element.route.data;
        var dataElement = (RegionDataElement)data.DataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.Name;

        } else {

            header      = dataElement.originalName;
        }

        idText.text     = dataElement.id.ToString();
        headerText.text = header;

        if (properties.edit)
            EditButtonData = data;
    }

    private void SetObjectGraphicElement()
    {
        Data data = Element.route.data;
        var dataElement = (ObjectGraphicDataElement)data.DataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.Name;
            iconPath    = dataElement.Icon;

        } else {

            header      = dataElement.originalName;
            iconPath    = dataElement.originalIcon;
        }

        idText.text     = dataElement.id.ToString();
        headerText.text = header;
        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetItemElement()
    {
        Data data = Element.route.data;
        var dataElement = (ItemDataElement)data.DataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.Name;
            iconPath    = dataElement.objectGraphicIcon;

        } else {

            header      = dataElement.originalName;
            iconPath    = dataElement.originalObjectGraphicIcon;
        }

        idText.text     = dataElement.id.ToString();
        headerText.text = header;
        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetElementElement()
    {
        Data data = Element.route.data;
        var dataElement = (ElementDataElement)data.DataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.Name;
            iconPath    = dataElement.objectGraphicIcon;

        } else {

            header      = dataElement.originalName;
            iconPath    = dataElement.originalObjectGraphicIcon;
        }

        idText.text     = dataElement.id.ToString();
        headerText.text = header;
        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    public void CloseElement()
    {
        content.offsetMin = new Vector2(10, content.offsetMin.y);
        content.offsetMax = new Vector2(-10, content.offsetMax.y);

        headerText.text = string.Empty;
        idText.text = string.Empty;

        if (descriptionText != null)
            descriptionText.text = string.Empty;

        if (properties.icon)
            icon.gameObject.SetActive(false);

        if (properties.edit)
            EditButton.gameObject.SetActive(false);
    }
}
