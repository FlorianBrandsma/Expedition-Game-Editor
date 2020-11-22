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
    public Text timeText;
    public RectTransform iconParent;
    public RawImage infoIcon;
    public RawImage icon;
    public RawImage iconBase;
    public RectTransform content;
    public Image background;
    
    private string header;
    private string description;
    private string infoIconPath;
    private string iconPath;
    private string iconBasePath;

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
    
    private Texture InfoIconTexture
    {
        get { return infoIcon.texture; }
        set
        {
            InitializeIcon();

            infoIcon.gameObject.SetActive(true);
            infoIcon.texture = value;
        }
    }

    private Texture IconTexture
    {
        get { return icon.texture; }
        set
        {
            InitializeIcon();

            icon.texture = value;
        }
    }

    private Texture IconBaseTexture
    {
        get { return iconBase.texture; }
        set
        {
            InitializeIcon();

            iconBase.texture = value;
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
        if (childProperty == SelectionManager.Property.None) return;
        
        ElementChild.DataElement.Data   = EditorElement.DataElement.Data;
        ElementChild.DataElement.Id     = EditorElement.DataElement.Id;
        
        ElementChild.DataElement.Path   = EditorElement.DataElement.Path;

        InitializeEdit();
    }

    private void InitializeIcon()
    {
        content.offsetMin = new Vector2(iconParent.rect.width + 5, content.offsetMin.y);
        iconParent.gameObject.SetActive(true);
    }

    private void InitializeEdit()
    {
        ElementChild.DataElement.InitializeElement(EditorElement.DataElement.DisplayManager, ElementChild.selectionType, childProperty, ElementChild.uniqueSelection);

        ElementChild.gameObject.SetActive(true);

        content.offsetMax = new Vector2(-ElementChild.GetComponent<RectTransform>().rect.width - 5, content.offsetMax.y);
    }

    public void UpdateElement()
    {
        SetElement();
    }

    public void SetElement()
    {
        switch (EditorElement.DataElement.Data.dataController.DataType)
        {
            case Enums.DataType.Chapter:                SetChapterElement();                break;
            case Enums.DataType.ChapterInteractable:    SetChapterInteractableElement();    break;
            case Enums.DataType.ChapterRegion:          SetChapterRegionElement();          break;
            case Enums.DataType.Phase:                  SetPhaseElement();                  break;
            case Enums.DataType.Quest:                  SetQuestElement();                  break;
            case Enums.DataType.Objective:              SetObjectiveElement();              break;
            case Enums.DataType.WorldInteractable:      SetWorldInteractableElement();      break;
            case Enums.DataType.Task:                   SetTaskElement();                   break;
            case Enums.DataType.Interaction:            SetInteractionElement();            break;
            case Enums.DataType.InteractionDestination: SetInteractionDestinationElement(); break;
            case Enums.DataType.Outcome:                SetOutcomeElement();                break;

            case Enums.DataType.Scene:                  SetSceneElement();                  break;
            case Enums.DataType.SceneShot:              SetSceneShotElement();              break;
            case Enums.DataType.SceneActor:             SetSceneActorElement();             break;

            case Enums.DataType.Region:                 SetRegionElement();                 break;
            case Enums.DataType.Atmosphere:             SetAtmosphereElement();             break;
            case Enums.DataType.Model:                  SetModelElement();                  break;
            case Enums.DataType.Item:                   SetItemElement();                   break;
            case Enums.DataType.Interactable:           SetInteractableElement();           break;

            case Enums.DataType.Save:                   SetSaveElement();                   break;
            case Enums.DataType.InteractableSave:       SetInteractableSaveElement();       break;
            case Enums.DataType.ChapterSave:            SetChapterSaveElement();            break;
            case Enums.DataType.PhaseSave:              SetPhaseSaveElement();              break;
            case Enums.DataType.QuestSave:              SetQuestSaveElement();              break;
            case Enums.DataType.ObjectiveSave:          SetObjectiveSaveElement();          break;
            case Enums.DataType.TaskSave:               SetTaskSaveElement();               break;
            case Enums.DataType.InteractionSave:        SetInteractionSaveElement();        break;
            
            default: Debug.Log("CASE MISSING: " + EditorElement.DataElement.Data.dataController.DataType);  break;
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
        var elementData = (ChapterElementData)EditorElement.DataElement.ElementData;

        if(EditorElement.selectionProperty == SelectionManager.Property.Get)
        {
            header      = elementData.Name;
            description = elementData.PublicNotes;
        } else {
            header      = elementData.OriginalData.Name;
            description = elementData.OriginalData.PublicNotes;
        }

        idText.text             = elementData.Id.ToString();
        headerText.text         = header;
        descriptionText.text    = description;
    }

    private void SetChapterInteractableElement()
    {
        var elementData = (ChapterInteractableElementData)EditorElement.DataElement.ElementData;

        header          = elementData.InteractableName;
        iconPath        = elementData.ModelIconPath;

        idText.text     = elementData.Id.ToString();
        headerText.text = header;

        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetChapterRegionElement()
    {
        var elementData = (ChapterRegionElementData)EditorElement.DataElement.ElementData;

        header          = elementData.Name;

        idText.text     = elementData.Id.ToString();
        headerText.text = header;

        iconPath        = elementData.TileIconPath;
        IconBaseTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetPhaseElement()
    {
        var elementData = (PhaseElementData)EditorElement.DataElement.ElementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
        {
            header      = elementData.Name;
            description = elementData.PublicNotes;
        } else {
            header      = elementData.OriginalData.Name;
            description = elementData.OriginalData.PublicNotes;
        }

        idText.text             = elementData.Id.ToString();
        headerText.text         = header;
        descriptionText.text    = description;
    }
    
    private void SetQuestElement()
    {
        var elementData = (QuestElementData)EditorElement.DataElement.ElementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
        {
            header      = elementData.Name;
            description = elementData.PublicNotes;
        } else {
            header      = elementData.OriginalData.Name;
            description = elementData.OriginalData.PublicNotes;
        }

        idText.text             = elementData.Id.ToString();
        headerText.text         = header;
        descriptionText.text    = description;
    }

    private void SetObjectiveElement()
    {
        var elementData = (ObjectiveElementData)EditorElement.DataElement.ElementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
        {
            header      = elementData.Name;
            description = elementData.PublicNotes;
        } else {
            header      = elementData.OriginalData.Name;
            description = elementData.OriginalData.PublicNotes;
        }

        idText.text             = elementData.Id.ToString();
        headerText.text         = header;
        descriptionText.text    = description;
    }

    private void SetWorldInteractableElement()
    {
        var elementData = (WorldInteractableElementData)EditorElement.DataElement.ElementData;

        header = elementData.InteractableName;
        
        EditorElement.elementStatus = elementData.ElementStatus;

        idText.text = elementData.Id.ToString();
        headerText.text = header;

        iconPath = elementData.ModelIconPath;
        IconTexture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetTaskElement()
    {
        var elementData = (TaskElementData)EditorElement.DataElement.ElementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
        {
            header      = elementData.Name;
            description = elementData.PublicNotes;
        } else {
            header      = elementData.OriginalData.Name;
            description = elementData.OriginalData.PublicNotes;
        }

        idText.text             = elementData.Id.ToString();
        headerText.text         = header;
        descriptionText.text    = description;
    }
    
    private void SetInteractionElement()
    {
        var elementData = (InteractionElementData)EditorElement.DataElement.ElementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
        {
            header      = elementData.Default ? "Default" : TimeManager.FormatTime(elementData.StartTime) + " - " + TimeManager.FormatTime(elementData.EndTime);
            description = elementData.PublicNotes;
        } else {
            header      = elementData.Default ? "Default" : TimeManager.FormatTime(elementData.OriginalData.StartTime) + " - " + TimeManager.FormatTime(elementData.OriginalData.EndTime);
            description = elementData.OriginalData.PublicNotes;
        }

        idText.text             = elementData.Id.ToString();
        headerText.text         = header;
        descriptionText.text    = description;
    }

    private void SetInteractionDestinationElement()
    {
        var elementData = (InteractionDestinationElementData)EditorElement.DataElement.ElementData;

        header = elementData.LocationName;
        description = elementData.InteractableStatus;
        
        idText.text = elementData.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;

        iconBasePath = elementData.TileIconPath;
        IconBaseTexture = Resources.Load<Texture2D>(iconBasePath);

        infoIconPath = "Textures/Icons/Status/SelectIcon";
        InfoIconTexture = Resources.Load<Texture2D>(infoIconPath);
        
        var offset = 0.6f;
        var iconScale = ((iconParent.rect.width * offset) / elementData.TileSize);
        
        infoIcon.transform.localPosition = elementData.LocalPosition * iconScale;
        
        infoIcon.color = Color.green;
    }

    private void SetOutcomeElement()
    {
        var elementData = (OutcomeElementData)EditorElement.DataElement.ElementData;

        header = Enum.GetName(typeof(Enums.OutcomeType), elementData.Type);
        description = elementData.PublicNotes;

        idText.text = elementData.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    private void SetSceneElement()
    {
        var elementData = (SceneElementData)EditorElement.DataElement.ElementData;

        header = elementData.Name;
        description = elementData.PublicNotes;

        idText.text = elementData.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    private void SetSceneShotElement()
    {
        var elementData = (SceneShotElementData)EditorElement.DataElement.ElementData;

        header = SceneShotManager.ShotDescription((Enums.SceneShotType)elementData.Type);

        idText.text = elementData.Id.ToString();
        headerText.text = header;
    }

    private void SetSceneActorElement()
    {
        var elementData = (SceneActorElementData)EditorElement.DataElement.ElementData;

        header = elementData.InteractableName;
        iconPath = elementData.ModelIconPath;

        idText.text = elementData.Id.ToString();
        headerText.text = header;
        IconTexture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetRegionElement()
    {
        var elementData = (RegionElementData)EditorElement.DataElement.ElementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get ||EditorElement.selectionProperty == SelectionManager.Property.OpenSceneRegion)
        {
            header      = elementData.Name;
            iconPath    = elementData.TileIconPath;
        } else {
            header      = elementData.OriginalData.Name;
            iconPath    = elementData.OriginalData.TileIconPath;
        }
        
        idText.text     = elementData.Id.ToString();
        headerText.text = header;
        IconBaseTexture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetAtmosphereElement()
    {
        var elementData = (AtmosphereElementData)EditorElement.DataElement.ElementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
        {
            header      = elementData.Default ? "Default" : TimeManager.FormatTime(elementData.StartTime) + " - " + TimeManager.FormatTime(elementData.EndTime);
            description = elementData.PublicNotes;
        } else {
            header      = elementData.Default ? "Default" : TimeManager.FormatTime(elementData.OriginalData.StartTime) + " - " + TimeManager.FormatTime(elementData.OriginalData.EndTime);
            description = elementData.OriginalData.PublicNotes;
        }

        idText.text             = elementData.Id.ToString();
        headerText.text         = header;
        descriptionText.text    = description;
    }

    private void SetModelElement()
    {
        var elementData = (ModelElementData)EditorElement.DataElement.ElementData;

        header          = elementData.Name;
        iconPath        = elementData.IconPath;
        
        idText.text     = elementData.Id.ToString();
        headerText.text = header;
        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetItemElement()
    {
        var elementData = (ItemElementData)EditorElement.DataElement.ElementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
        {
            header      = elementData.Name;
            iconPath    = elementData.ModelIconPath;
        } else {
            header      = elementData.OriginalData.Name;
            iconPath    = elementData.OriginalData.ModelIconPath;
        }

        idText.text     = elementData.Id.ToString();
        headerText.text = header;
        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetInteractableElement()
    {
        var elementData = (InteractableElementData)EditorElement.DataElement.ElementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
        {
            header      = elementData.Name;
            iconPath    = elementData.ModelIconPath;
        } else {
            header      = elementData.OriginalData.Name;
            iconPath    = elementData.OriginalData.ModelIconPath;
        }

        idText.text     = elementData.Id.ToString();
        headerText.text = header;
        IconTexture     = Resources.Load<Texture2D>(iconPath);
    }

    private void SetSaveElement()
    {
        var elementData = (SaveElementData)EditorElement.DataElement.ElementData;

        header      = elementData.Name;
        description = elementData.LocationName;

        iconPath    = elementData.ModelIconPath;
        
        headerText.text         = header;
        descriptionText.text    = description;

        IconTexture     = Resources.Load<Texture2D>(iconPath);

        idText.text     = elementData.Id.ToString();
        timeText.text   = elementData.Time;
    }

    private void SetInteractableSaveElement()
    {
        var elementData = (InteractableSaveElementData)EditorElement.DataElement.ElementData;

        header = elementData.InteractableName;
        
        idText.text = elementData.Id.ToString();
        headerText.text = header;

        iconPath = elementData.ModelIconPath;
        IconTexture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetChapterSaveElement()
    {
        var elementData = (ChapterSaveElementData)EditorElement.DataElement.ElementData;

        header = elementData.Name;
        description = elementData.PublicNotes;

        idText.text = elementData.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    private void SetPhaseSaveElement()
    {
        var elementData = (PhaseSaveElementData)EditorElement.DataElement.ElementData;

        header = elementData.Name;
        description = elementData.PublicNotes;

        idText.text = elementData.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    private void SetQuestSaveElement()
    {
        var elementData = (QuestSaveElementData)EditorElement.DataElement.ElementData;

        header = elementData.Name;
        description = elementData.PublicNotes;

        idText.text = elementData.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    private void SetObjectiveSaveElement()
    {
        var elementData = (ObjectiveSaveElementData)EditorElement.DataElement.ElementData;

        header = elementData.Name;
        description = elementData.PublicNotes;

        idText.text = elementData.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    private void SetTaskSaveElement()
    {
        var elementData = (TaskSaveElementData)EditorElement.DataElement.ElementData;

        header = elementData.Name;
        description = elementData.PublicNotes;

        idText.text = elementData.Id.ToString();
        headerText.text = header;
        descriptionText.text = description;
    }

    private void SetInteractionSaveElement()
    {
        var elementData = (InteractionSaveElementData)EditorElement.DataElement.ElementData;

        header = elementData.Default ? "Default" : TimeManager.FormatTime(elementData.StartTime) + " - " + TimeManager.FormatTime(elementData.EndTime);

        description = elementData.PublicNotes;

        idText.text = elementData.Id.ToString();
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

        if(timeText != null)
            timeText.text = string.Empty;
        
        iconParent.gameObject.SetActive(false);

        iconBase.texture = Resources.Load<Texture2D>("Textures/Icons/Nothing");
        icon.texture = Resources.Load<Texture2D>("Textures/Icons/Objects/Nothing");
        
        if (infoIcon != null)
            infoIcon.gameObject.SetActive(false);
    }

    public void ClosePoolable()
    {
        //gameObject.SetActive(false);
    }
}
