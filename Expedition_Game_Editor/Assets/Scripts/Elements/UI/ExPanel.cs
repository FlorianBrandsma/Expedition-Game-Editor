using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class ExPanel : MonoBehaviour, IElement, IPoolable
{
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

    public EditorElement EditorElement  { get { return GetComponent<EditorElement>(); } }
    private EditorElement ElementChild  { get { return EditorElement.child; } }

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

    private DataElement.Data ChildButtonData
    {
        get { return ElementChild.DataElement.data; }
        set
        {
            InitializeEdit();
            ElementChild.DataElement.data = value;
        }
    }

    public IPoolable Instantiate()
    {
        var newElement = Instantiate(this);

        SelectionElementManager.Add(newElement.EditorElement);

        return newElement;
    }

    public void InitializeElement() { }

    public void InitializeChildElement()
    {
        if (childProperty != SelectionManager.Property.None)
            ChildButtonData = EditorElement.DataElement.data;
    }

    private void InitializeIcon()
    {
        content.offsetMin = new Vector2(iconParent.rect.width + 5, content.offsetMin.y);
        iconParent.gameObject.SetActive(true);
    }

    private void InitializeEdit()
    {
        ElementChild.DataElement.InitializeElement(EditorElement.DataElement.DisplayManager, ElementChild.selectionType, childProperty);

        ElementChild.gameObject.SetActive(true);

        content.offsetMax = new Vector2(-ElementChild.GetComponent<RectTransform>().rect.width - 5, content.offsetMax.y);
    }

    public void UpdateElement()
    {
        SetElement();
    }

    public void SetElement()
    {
        switch (EditorElement.DataElement.data.dataController.DataType)
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

            case Enums.DataType.Save:               SetSaveElement();               break;
            case Enums.DataType.ChapterSave:        SetChapterSaveElement();        break;
            case Enums.DataType.PhaseSave:          SetPhaseSaveElement();          break;
            case Enums.DataType.QuestSave:          SetQuestSaveElement();          break;
            case Enums.DataType.ObjectiveSave:      SetObjectiveSaveElement();      break;
            case Enums.DataType.TaskSave:           SetTaskSaveElement();           break;
            case Enums.DataType.InteractionSave:    SetInteractionSaveElement();    break;
            
            default: Debug.Log("CASE MISSING: " + EditorElement.DataElement.data.dataController.DataType);  break;
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
        var data = EditorElement.DataElement.data;
        var dataElement = (ChapterDataElement)data.dataElement;

        if(EditorElement.selectionProperty == SelectionManager.Property.Get)
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
        var data = EditorElement.DataElement.data;
        var dataElement = (PartyMemberDataElement)data.dataElement;

        header          = dataElement.interactableName;
        iconPath        = dataElement.objectGraphicIconPath;

        idText.text     = dataElement.Id.ToString();
        headerText.text = header;

        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetChapterInteractableElement()
    {
        var data = EditorElement.DataElement.data;
        var dataElement = (ChapterInteractableDataElement)data.dataElement;

        header          = dataElement.interactableName;
        iconPath        = dataElement.objectGraphicIconPath;

        idText.text     = dataElement.Id.ToString();
        headerText.text = header;

        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetChapterRegionElement()
    {
        var data = EditorElement.DataElement.data;
        var dataElement = (ChapterRegionDataElement)data.dataElement;

        header          = dataElement.name;

        idText.text     = dataElement.Id.ToString();
        headerText.text = header;

        iconPath        = dataElement.tileIconPath;
        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetPhaseElement()
    {
        var data = EditorElement.DataElement.data;
        var dataElement = (PhaseDataElement)data.dataElement;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
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
        var data = EditorElement.DataElement.data;
        var dataElement = (QuestDataElement)data.dataElement;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
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
        var data = EditorElement.DataElement.data;
        var dataElement = (ObjectiveDataElement)data.dataElement;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
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
        var data = EditorElement.DataElement.data;
        var dataElement = (WorldInteractableDataElement)data.dataElement;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
        {
            header = dataElement.interactableName;
            iconPath = dataElement.objectGraphicIconPath;
        } else {
            header = dataElement.originalInteractableName;
            iconPath = dataElement.originalObjectGraphicIconPath;
        }

        EditorElement.elementStatus = dataElement.elementStatus;

        idText.text = dataElement.Id.ToString();
        headerText.text = header;

        IconTexture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetTaskElement()
    {
        var data = EditorElement.DataElement.data;
        var dataElement = (TaskDataElement)data.dataElement;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
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
        var data = EditorElement.DataElement.data;
        var dataElement = (InteractionDataElement)data.dataElement;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
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
        var data = EditorElement.DataElement.data;
        var dataElement = (OutcomeDataElement)data.dataElement;

        header = Enum.GetName(typeof(Enums.OutcomeType), dataElement.type);
        description = (Enums.OutcomeType)dataElement.type == Enums.OutcomeType.Positive ? "Requirements passed" : "Requirements failed";

        idText.text = dataElement.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    private void SetRegionElement()
    {
        var data = EditorElement.DataElement.data;
        var dataElement = (RegionDataElement)data.dataElement;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
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
        var data = EditorElement.DataElement.data;
        var dataElement = (AtmosphereDataElement)data.dataElement;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
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
        var data = EditorElement.DataElement.data;
        var dataElement = (ObjectGraphicDataElement)data.dataElement;
        
        header      = dataElement.Name;
        iconPath    = dataElement.iconPath;
        
        idText.text     = dataElement.Id.ToString();
        headerText.text = header;
        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetItemElement()
    {
        var data = EditorElement.DataElement.data;
        var dataElement = (ItemDataElement)data.dataElement;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
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
        var data = EditorElement.DataElement.data;
        var dataElement = (InteractableDataElement)data.dataElement;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
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

    private void SetSaveElement()
    {
        var data = EditorElement.DataElement.data;
        var dataElement = (SaveDataElement)data.dataElement;

        header = dataElement.name;

        idText.text = dataElement.Id.ToString();
        headerText.text = header;
    }

    private void SetChapterSaveElement()
    {
        var data = EditorElement.DataElement.data;
        var dataElement = (ChapterSaveDataElement)data.dataElement;

        header = dataElement.name;
        description = dataElement.publicNotes;

        idText.text = dataElement.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    private void SetPhaseSaveElement()
    {
        var data = EditorElement.DataElement.data;
        var dataElement = (PhaseSaveDataElement)data.dataElement;

        header = dataElement.name;
        description = dataElement.publicNotes;

        idText.text = dataElement.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    private void SetQuestSaveElement()
    {
        var data = EditorElement.DataElement.data;
        var dataElement = (QuestSaveDataElement)data.dataElement;

        header = dataElement.name;
        description = dataElement.publicNotes;

        idText.text = dataElement.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    private void SetObjectiveSaveElement()
    {
        var data = EditorElement.DataElement.data;
        var dataElement = (ObjectiveSaveDataElement)data.dataElement;

        header = dataElement.name;
        description = dataElement.publicNotes;

        idText.text = dataElement.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    private void SetTaskSaveElement()
    {
        var data = EditorElement.DataElement.data;
        var dataElement = (TaskSaveDataElement)data.dataElement;

        header = dataElement.name;
        description = dataElement.publicNotes;

        idText.text = dataElement.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    private void SetInteractionSaveElement()
    {
        var data = EditorElement.DataElement.data;
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
