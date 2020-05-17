using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class ExPanel : MonoBehaviour, IElement, IPoolable
{
    private SelectionElement.Data data;

    public Enums.ElementType elementType;
    public Enums.IconType iconType;
    public SelectionManager.Property childProperty;

    public Text idText;
    public Text headerText;
    public Text descriptionText;
    public RectTransform iconParent;
    public RawImage icon;
    public RawImage iconBase;
    public RectTransform content;
    public Image background;
    
    private string header;
    private string description;
    private string iconPath;

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
            if (iconType == Enums.IconType.None) return;

            InitializeIcon();

            if (iconType == Enums.IconType.Base)
                iconBase.texture = value;

            if (iconType == Enums.IconType.Icon)
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

    public void InitializeElement() { }

    public void InitializeChildElement()
    {
        if (childProperty != SelectionManager.Property.None)
            ChildButtonData = Element.data;
    }

    private void InitializeIcon()
    {
        content.offsetMin = new Vector2(iconParent.rect.width + 5, content.offsetMin.y);
        iconParent.gameObject.SetActive(true);
    }

    private void InitializeEdit()
    {
        ElementChild.InitializeElement(Element.DisplayManager, ElementChild.selectionType, childProperty);

        ElementChild.gameObject.SetActive(true);

        content.offsetMax = new Vector2(-ElementChild.GetComponent<RectTransform>().rect.width - 5, content.offsetMax.y);
    }

    public void SetElement()
    {
        data = Element.data;

        switch (Element.data.dataController.DataType)
        {
            case Enums.DataType.Chapter:            SetChapterElement();            break;
            case Enums.DataType.PartyMember:        SetPartyMemberElement();        break;
            case Enums.DataType.ChapterInteractable:SetChapterInteractableElement();break;
            case Enums.DataType.ChapterRegion:      SetChapterRegionElement();      break;
            case Enums.DataType.Phase:              SetPhaseElement();              break;
            case Enums.DataType.Quest:              SetQuestElement();              break;
            case Enums.DataType.Objective:          SetObjectiveElement();          break;
            case Enums.DataType.WorldInteractable:  SetWorldInteractableElement();  break;
            case Enums.DataType.Task:               SetTaskElement();               break;
            case Enums.DataType.Interaction:        SetInteractionElement();        break;
            case Enums.DataType.Outcome:            SetOutcomeElement();            break;
            case Enums.DataType.Region:             SetRegionElement();             break;
            case Enums.DataType.Atmosphere:         SetAtmosphereElement();         break;
            case Enums.DataType.ObjectGraphic:      SetObjectGraphicElement();      break;
            case Enums.DataType.Item:               SetItemElement();               break;
            case Enums.DataType.Interactable:       SetInteractableElement();       break;

            case Enums.DataType.GameSave:           SetGameSaveElement();           break;
            case Enums.DataType.ChapterSave:        SetChapterSaveElement();        break;
            case Enums.DataType.PhaseSave:          SetPhaseSaveElement();          break;
            case Enums.DataType.QuestSave:          SetQuestSaveElement();          break;
            case Enums.DataType.ObjectiveSave:      SetObjectiveSaveElement();      break;
            case Enums.DataType.TaskSave:           SetTaskSaveElement();           break;
            case Enums.DataType.InteractionSave:    SetInteractionSaveElement();    break;
            
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
        var dataElement = (ChapterDataElement)data.dataElement;

        if(Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.Name;
            description = dataElement.PublicNotes;
        } else {
            header      = dataElement.originalName;
            description = dataElement.originalPublicNotes;
        }

        idText.text             = dataElement.Id.ToString();
        headerText.text         = header;
        descriptionText.text    = description;
    }

    private void SetPartyMemberElement()
    {
        var dataElement = (PartyMemberDataElement)data.dataElement;

        header          = dataElement.interactableName;
        iconPath        = dataElement.objectGraphicIconPath;

        idText.text     = dataElement.Id.ToString();
        headerText.text = header;

        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetChapterInteractableElement()
    {
        var dataElement = (ChapterInteractableDataElement)data.dataElement;

        header          = dataElement.interactableName;
        iconPath        = dataElement.objectGraphicIconPath;

        idText.text     = dataElement.Id.ToString();
        headerText.text = header;

        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetChapterRegionElement()
    {
        var dataElement = (ChapterRegionDataElement)data.dataElement;

        header          = dataElement.name;

        idText.text     = dataElement.Id.ToString();
        headerText.text = header;

        iconPath        = dataElement.tileIconPath;
        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetPhaseElement()
    {
        var dataElement = (PhaseDataElement)data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.Name;
            description = dataElement.PublicNotes;
        } else {
            header      = dataElement.originalName;
            description = dataElement.originalPublicNotes;
        }

        idText.text             = dataElement.Id.ToString();
        headerText.text         = header;
        descriptionText.text    = description;
    }
    
    private void SetQuestElement()
    {
        var dataElement = (QuestDataElement)data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.Name;
            description = dataElement.PublicNotes;
        } else {
            header      = dataElement.originalName;
            description = dataElement.originalPublicNotes;
        }

        idText.text             = dataElement.Id.ToString();
        headerText.text         = header;
        descriptionText.text    = description;
    }

    private void SetObjectiveElement()
    {
        var dataElement = (ObjectiveDataElement)data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.Name;
            description = dataElement.PublicNotes;
        } else {
            header      = dataElement.originalName;
            description = dataElement.originalPublicNotes;
        }

        idText.text             = dataElement.Id.ToString();
        headerText.text         = header;
        descriptionText.text    = description;
    }

    private void SetWorldInteractableElement()
    {
        var dataElement = (WorldInteractableDataElement)data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header = dataElement.interactableName;
            iconPath = dataElement.objectGraphicIconPath;
        } else {
            header = dataElement.originalInteractableName;
            iconPath = dataElement.originalObjectGraphicIconPath;
        }

        Element.elementStatus = dataElement.elementStatus;

        idText.text = dataElement.Id.ToString();
        headerText.text = header;

        IconTexture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetTaskElement()
    {
        var dataElement = (TaskDataElement)data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.Name;
            description = dataElement.PublicNotes;
        } else {
            header      = dataElement.originalName;
            description = dataElement.originalPublicNotes;
        }

        idText.text             = dataElement.Id.ToString();
        headerText.text         = header;
        descriptionText.text    = description;
    }
    
    private void SetInteractionElement()
    {
        var dataElement = (InteractionDataElement)data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.Default ? "Default" : TimeManager.FormatTime(dataElement.StartTime, true) + " - " + TimeManager.FormatTime(dataElement.EndTime);
            description = dataElement.PublicNotes;
        } else {
            header      = dataElement.Default ? "Default" : TimeManager.FormatTime(dataElement.originalStartTime, true) + " - " + TimeManager.FormatTime(dataElement.originalEndTime);
            description = dataElement.originalPublicNotes;
        }

        idText.text             = dataElement.Id.ToString();
        headerText.text         = header;
        descriptionText.text    = description;
    }

    private void SetOutcomeElement()
    {
        var dataElement = (OutcomeDataElement)data.dataElement;

        header = Enum.GetName(typeof(Enums.OutcomeType), dataElement.type);
        description = (Enums.OutcomeType)dataElement.type == Enums.OutcomeType.Positive ? "Requirements passed" : "Requirements failed";

        idText.text = dataElement.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    private void SetRegionElement()
    {
        var dataElement = (RegionDataElement)data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.Name;
            iconPath    = dataElement.tileIconPath;
        } else {
            header      = dataElement.originalName;
            iconPath    = dataElement.originalTileIconPath;
        }
        
        idText.text     = dataElement.Id.ToString();
        headerText.text = header;
        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetAtmosphereElement()
    {
        var dataElement = (AtmosphereDataElement)data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.Default ? "Default" : TimeManager.FormatTime(dataElement.StartTime, true) + " - " + TimeManager.FormatTime(dataElement.EndTime);
            description = dataElement.PublicNotes;
        } else {
            header      = dataElement.Default ? "Default" : TimeManager.FormatTime(dataElement.originalStartTime, true) + " - " + TimeManager.FormatTime(dataElement.originalEndTime);
            description = dataElement.originalPublicNotes;
        }

        idText.text             = dataElement.Id.ToString();
        headerText.text         = header;
        descriptionText.text    = description;
    }

    private void SetObjectGraphicElement()
    {
        var dataElement = (ObjectGraphicDataElement)data.dataElement;
        
        header      = dataElement.Name;
        iconPath    = dataElement.iconPath;
        
        idText.text     = dataElement.Id.ToString();
        headerText.text = header;
        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetItemElement()
    {
        var dataElement = (ItemDataElement)data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.Name;
            iconPath    = dataElement.objectGraphicIconPath;
        } else {
            header      = dataElement.originalName;
            iconPath    = dataElement.originalObjectGraphicIconPath;
        }

        idText.text     = dataElement.Id.ToString();
        headerText.text = header;
        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetInteractableElement()
    {
        var dataElement = (InteractableDataElement)data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
        {
            header      = dataElement.Name;
            iconPath    = dataElement.objectGraphicIconPath;
        } else {
            header      = dataElement.originalName;
            iconPath    = dataElement.originalObjectGraphicIconPath;
        }

        idText.text     = dataElement.Id.ToString();
        headerText.text = header;
        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetGameSaveElement()
    {
        var dataElement = (GameSaveDataElement)data.dataElement;

        header = dataElement.name;

        idText.text = dataElement.Id.ToString();
        headerText.text = header;
    }

    private void SetChapterSaveElement()
    {
        var dataElement = (ChapterSaveDataElement)data.dataElement;

        header = dataElement.name;
        description = dataElement.publicNotes;

        idText.text = dataElement.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    private void SetPhaseSaveElement()
    {
        var dataElement = (PhaseSaveDataElement)data.dataElement;

        header = dataElement.name;
        description = dataElement.publicNotes;

        idText.text = dataElement.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    private void SetQuestSaveElement()
    {
        var dataElement = (QuestSaveDataElement)data.dataElement;

        header = dataElement.name;
        description = dataElement.publicNotes;

        idText.text = dataElement.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    private void SetObjectiveSaveElement()
    {
        var dataElement = (ObjectiveSaveDataElement)data.dataElement;

        header = dataElement.name;
        description = dataElement.publicNotes;

        idText.text = dataElement.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    private void SetTaskSaveElement()
    {
        var dataElement = (TaskSaveDataElement)data.dataElement;

        header = dataElement.name;
        description = dataElement.publicNotes;

        idText.text = dataElement.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    private void SetInteractionSaveElement()
    {
        var dataElement = (InteractionSaveDataElement)data.dataElement;

        header = dataElement.isDefault ? "Default" : TimeManager.FormatTime(dataElement.startTime, true) + " - " + TimeManager.FormatTime(dataElement.endTime);

        description = dataElement.publicNotes;

        idText.text = dataElement.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    public void CloseElement()
    {
        content.offsetMin = new Vector2(10, content.offsetMin.y);
        content.offsetMax = new Vector2(-10, content.offsetMax.y);

        headerText.text = string.Empty;
        header = string.Empty;

        idText.text = string.Empty;
        
        if (descriptionText != null)
        {
            descriptionText.text = string.Empty;
            description = string.Empty;
        }

        if (iconType != Enums.IconType.None)
        {
            iconParent.gameObject.SetActive(false);

            if(iconType == Enums.IconType.Base)
                iconBase.texture = Resources.Load<Texture2D>("Textures/Icons/Nothing");

            if (iconType == Enums.IconType.Icon)
                icon.texture = Resources.Load<Texture2D>("Textures/Icons/Objects/Nothing");
        }
    }

    public void ClosePoolable()
    {
        //gameObject.SetActive(false);
    }
}
