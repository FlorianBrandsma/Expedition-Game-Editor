using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EditorPanel : MonoBehaviour, IElement
{
    public Text idText;
    public Text headerText;
    public Text descriptionText;
    public RectTransform iconParent;
    public RawImage icon;
    public RectTransform content;

    private string header;
    private string description;
    private string iconPath;

    private PanelProperties properties;

    private SelectionElement Element    { get { return GetComponent<SelectionElement>(); } }
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

    private SelectionElement.Data ChildButtonData
    {
        get { return ElementChild.data; }
        set
        {
            InitializeEdit();
            ElementChild.data = value;
        }
    }
    
    public void InitializeElement()
    {
        properties = (PanelProperties)Element.DisplayManager.Display.Properties;
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
            case Enums.DataType.Chapter:            SetChapterElement();            break;
            case Enums.DataType.ChapterRegion:      SetChapterRegionElement();      break;
            case Enums.DataType.Phase:              SetPhaseElement();              break;
            case Enums.DataType.PhaseInteractable:  SetPhaseInteractableElement();  break;
            case Enums.DataType.Quest:              SetQuestElement();              break;
            case Enums.DataType.Objective:          SetObjectiveElement();          break;
            case Enums.DataType.Interaction:        SetInteractionElement();        break;
            case Enums.DataType.Region:             SetRegionElement();             break;
            case Enums.DataType.ObjectGraphic:      SetObjectGraphicElement();      break;
            case Enums.DataType.Item:               SetItemElement();               break;
            case Enums.DataType.Interactable:       SetInteractableElement();       break;
            case Enums.DataType.SceneInteractable:  SetSceneInteractableElement();  break;
            case Enums.DataType.PartyMember:        SetPartyMemberElement();        break;

            default: Debug.Log("CASE MISSING: " + Element.data.dataController.DataType);  break;
        }

        if (descriptionText != null)
        {
            if (headerText.text == string.Empty)
                descriptionText.rectTransform.offsetMax = new Vector2(descriptionText.rectTransform.offsetMax.x, 0);
            else
                descriptionText.rectTransform.offsetMax = new Vector2(descriptionText.rectTransform.offsetMax.x, -headerText.rectTransform.rect.height);
        }
    }

    private void SetChapterElement()
    {
        var data = Element.data;
        var dataElement = (ChapterDataElement)data.dataElement;

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
    }

    private void SetChapterRegionElement()
    {
        var data = Element.data;
        var dataElement = (ChapterRegionDataElement)data.dataElement;

        header = dataElement.name;

        idText.text = dataElement.id.ToString();
        headerText.text = header;
    }

    private void SetPhaseElement()
    {
        var data = Element.data;
        var dataElement = (PhaseDataElement)data.dataElement;

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
    }

    private void SetPhaseInteractableElement()
    {
        var data = Element.data;
        var dataElement = (PhaseInteractableDataElement)data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header = dataElement.interactableName;
            iconPath = dataElement.objectGraphicIcon;

        } else {

            header = dataElement.originalInteractableName;
            iconPath = dataElement.originalObjectGraphicIcon;
        }

        Element.elementStatus = dataElement.elementStatus;

        idText.text = dataElement.id.ToString();
        headerText.text = header;

        if (properties.icon)
            IconTexture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetQuestElement()
    {
        var data = Element.data;
        var dataElement = (QuestDataElement)data.dataElement;

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
    }

    private void SetObjectiveElement()
    {
        var data = Element.data;
        var dataElement = (ObjectiveDataElement)data.dataElement;

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
    }

    private void SetSceneInteractableElement()
    {
        var data = Element.data;
        var dataElement = (SceneInteractableDataElement)data.dataElement;

        header      = dataElement.interactableName;
        iconPath    = dataElement.objectGraphicIconPath;

        idText.text     = dataElement.id.ToString();
        headerText.text = header;

        if (properties.icon)
            IconTexture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetPartyMemberElement()
    {
        var data = Element.data;
        var dataElement = (PartyMemberDataElement)data.dataElement;

        header = dataElement.interactableName;
        iconPath = dataElement.objectGraphicIconPath;

        idText.text = dataElement.id.ToString();
        headerText.text = header;
        
        if (properties.icon)
            IconTexture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetInteractionElement()
    {
        var data = Element.data;
        var dataElement = (InteractionDataElement)data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            description         = dataElement.Description;

        } else {

            description         = dataElement.originalDescription;
        }

        idText.text             = dataElement.id.ToString();
        descriptionText.text    = description;
    }

    private void SetRegionElement()
    {
        var data = Element.data;
        var dataElement = (RegionDataElement)data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.Name;

        } else {

            header      = dataElement.originalName;
        }

        idText.text     = dataElement.id.ToString();
        headerText.text = header;
    }

    private void SetObjectGraphicElement()
    {
        var data = Element.data;
        var dataElement = (ObjectGraphicDataElement)data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.Name;
            iconPath    = dataElement.iconPath;

        } else {

            header      = dataElement.originalName;
            iconPath    = dataElement.originalIconPath;
        }

        idText.text     = dataElement.id.ToString();
        headerText.text = header;
        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetItemElement()
    {
        var data = Element.data;
        var dataElement = (ItemDataElement)data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.Name;
            iconPath    = dataElement.objectGraphicIconPath;

        } else {

            header      = dataElement.originalName;
            iconPath    = dataElement.originalObjectGraphicIconPath;
        }

        idText.text     = dataElement.id.ToString();
        headerText.text = header;
        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetInteractableElement()
    {
        var data = Element.data;
        var dataElement = (InteractableDataElement)data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.Name;
            iconPath    = dataElement.objectGraphicIconPath;

        } else {

            header      = dataElement.originalName;
            iconPath    = dataElement.originalObjectGraphicIconPath;
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
            iconParent.gameObject.SetActive(false);

        if (properties.childProperty != SelectionManager.Property.None)
            ElementChild.gameObject.SetActive(false);
    }
}
