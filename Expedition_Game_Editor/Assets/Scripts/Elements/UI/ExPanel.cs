using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Globalization;

public class ExPanel : MonoBehaviour, IElement, IPoolable
{
    public Enums.ElementType elementType;
    public Enums.IconType iconType;
    public SelectionManager.Property childProperty;

    public Text idText;
    public Text indexText;
    public Text headerText;
    public Text descriptionText;
    public Text playTimeText;
    public Text saveTimeText;
    public RectTransform iconParent;
    public RawImage infoIcon;
    public RawImage icon;
    public RawImage iconBase;
    public RectTransform content;
    public Image background;

    private int id;
    private int index;
    private string header;
    private string description;
    private int playTime;
    private DateTime saveTime;
    private string infoIconPath;
    private string iconPath;
    private Texture iconTexture;

    private RectTransform IdRectTransform           { get { return idText.GetComponent<RectTransform>(); } }
    private RectTransform DescriptionRectTransform  { get { return descriptionText.GetComponent<RectTransform>(); } }
    private RectTransform HeaderRectTransform       { get { return headerText.GetComponent<RectTransform>(); } }

    public EditorElement EditorElement              { get { return GetComponent<EditorElement>(); } }
    private EditorElement ElementChild              { get { return EditorElement.child; } }

    public ListManager ListManager                  { get { return (ListManager)EditorElement.DataElement.DisplayManager; } }
    public ListProperties ListProperties            { get { return (ListProperties)ListManager.Display; } }

    public Color ElementColor
    {
        set { background.color = value; }
    }

    public Transform Transform              { get { return GetComponent<Transform>(); } }    
    public Enums.ElementType ElementType    { get { return elementType; } }
    public int PoolId                       { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }
    
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

        ElementChild.DataElement.InitializeElement(EditorElement.DataElement.DisplayManager, ElementChild.selectionType, childProperty, SelectionManager.Property.None, ElementChild.uniqueSelection);     
    }

    private void SetId(bool enable)
    {
        if (idText == null) return;

        if(enable)
            idText.text = id.ToString();
        
        idText.gameObject.SetActive(enable);
    }

    private void SetIndex(bool enable)
    {
        if (indexText == null) return;

        if(enable)
        {
            indexText.text = (index + 1).ToString();
            content.offsetMin = new Vector2(indexText.rectTransform.rect.width + 5, content.offsetMin.y);

        } else {

            content.offsetMin = new Vector2(5, content.offsetMin.y);
        }

        indexText.gameObject.SetActive(enable);
    }

    private void SetHeader(bool enable)
    {
        if (headerText == null) return;

        if(enable)
        {
            if (ElementType == Enums.ElementType.CompactPanel)
                HeaderRectTransform.offsetMin = new Vector2(HeaderRectTransform.offsetMin.x, IdRectTransform.rect.height);

            if (ElementType == Enums.ElementType.Panel || ElementType == Enums.ElementType.SavePanel)
                HeaderRectTransform.offsetMin = new Vector2(HeaderRectTransform.offsetMin.x, DescriptionRectTransform.offsetMax.y);

            headerText.alignment = TextAnchor.UpperLeft;

        } else {

            if (ElementType == Enums.ElementType.CompactPanel)
                HeaderRectTransform.offsetMin = new Vector2(HeaderRectTransform.offsetMin.x, 0);

            if (ElementType == Enums.ElementType.Panel || ElementType == Enums.ElementType.SavePanel)
                HeaderRectTransform.offsetMin = new Vector2(HeaderRectTransform.offsetMin.x, -content.rect.height);

            headerText.alignment = TextAnchor.MiddleCenter;

            if(EditorElement.ActiveSelectionProperty == SelectionManager.Property.Set)
            {
                header = "Remove selection";

            } else {

                header = "Add new";
            }
        }

        headerText.text = header;
    }

    private void SetDescription(bool enable)
    {
        if (descriptionText == null) return;

        if(enable)
            descriptionText.text = description;
        
        descriptionText.gameObject.SetActive(enable);
    }

    private void SetPlayTime(bool enable)
    {
        if (playTimeText == null) return;

        if (enable)
            playTimeText.text = TimeManager.TimeFromSeconds(playTime);

        playTimeText.gameObject.SetActive(enable);
    }

    private void SetSaveTime(bool enable)
    {
        if (saveTimeText == null) return;

        if (enable)
            saveTimeText.text = saveTime.ToString("H:mm:ss dd/MM/yyyy");
        
        saveTimeText.gameObject.SetActive(enable);
    }

    private void SetIcon(bool enable)
    {
        if (iconParent == null) return;

        if(enable)
        {
            var texture = Resources.Load<Texture2D>(iconPath);

            if(iconType == Enums.IconType.Base)
            {
                icon.texture = Resources.Load<Texture2D>("Textures/Icons/Objects/Nothing");
                iconBase.texture = texture;
                
            } else {

                icon.texture = texture;
                iconBase.texture = Resources.Load<Texture2D>("Textures/Icons/Nothing");
            }

            content.offsetMin = new Vector2(iconParent.rect.width + 5, content.offsetMin.y);

        } else {

            content.offsetMin = new Vector2(10, content.offsetMin.y);
        }

        iconParent.gameObject.SetActive(enable);
    }

    private void SetInfoIcon(bool enable)
    {
        if (infoIcon == null) return;

        if(enable)
        {
            var texture = Resources.Load<Texture2D>(infoIconPath);
            infoIcon.texture = texture;
        }

        infoIcon.gameObject.SetActive(enable);
    }

    private void SetChild(bool enable)
    {
        if (ElementChild == null) return;

        if(enable)
        {
            content.offsetMax = new Vector2(-ElementChild.GetComponent<RectTransform>().rect.width - 5, content.offsetMax.y);

        } else {

            content.offsetMax = new Vector2(-10, content.offsetMax.y);
        }

        ElementChild.gameObject.SetActive(enable);
    }

    public void UpdateElement()
    {
        SetElement();
    }

    public void SetElement()
    {
        id = EditorElement.DataElement.Id;

        if(ElementChild != null)
            ElementChild.DataElement.Id = EditorElement.DataElement.Id;
        
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
            case Enums.DataType.CameraFilter:           SetCameraFilterElement();           break;
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

        var enableContent = (id != -1 && id != 0);

        SetId(enableContent);
        SetIndex(enableContent);
        SetHeader(enableContent);
        SetDescription(enableContent && description != null);
        SetPlayTime(enableContent);
        SetSaveTime(enableContent);
        SetIcon(enableContent && iconPath != null);
        SetInfoIcon(infoIconPath != null);

        SetChild(childProperty != SelectionManager.Property.None && enableContent);
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
    }

    private void SetChapterInteractableElement()
    {
        var elementData = (ChapterInteractableElementData)EditorElement.DataElement.ElementData;

        header          = elementData.InteractableName;
        iconPath        = elementData.ModelIconPath;
    }

    private void SetChapterRegionElement()
    {
        var elementData = (ChapterRegionElementData)EditorElement.DataElement.ElementData;

        header          = elementData.Name;
        iconPath        = elementData.TileIconPath;
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
    }

    private void SetWorldInteractableElement()
    {
        var elementData = (WorldInteractableElementData)EditorElement.DataElement.ElementData;

        header = elementData.InteractableName;
        iconPath = elementData.ModelIconPath;

        EditorElement.elementStatus = elementData.ElementStatus;
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
    }

    private void SetInteractionDestinationElement()
    {
        var elementData = (InteractionDestinationElementData)EditorElement.DataElement.ElementData;

        header = elementData.LocationName;
        description = elementData.InteractableStatus;

        iconPath = elementData.TileIconPath;
        infoIconPath = "Textures/Icons/Status/SelectIcon";

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
    }

    private void SetSceneElement()
    {
        var elementData = (SceneElementData)EditorElement.DataElement.ElementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
        {
            header = elementData.Name;
            description = elementData.PublicNotes;
        } else {
            header = elementData.OriginalData.Name;
            description = elementData.OriginalData.PublicNotes;
        }
    }

    private void SetSceneShotElement()
    {
        var elementData = (SceneShotElementData)EditorElement.DataElement.ElementData;

        header = SceneShotManager.ShotDescription((Enums.SceneShotType)elementData.Type);
    }

    private void SetCameraFilterElement()
    {
        var elementData = (CameraFilterElementData)EditorElement.DataElement.ElementData;

        header = elementData.Name;
        iconPath = elementData.IconPath;
    }

    private void SetSceneActorElement()
    {
        var elementData = (SceneActorElementData)EditorElement.DataElement.ElementData;

        header = elementData.InteractableName;
        iconPath = elementData.ModelIconPath;
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
    }

    private void SetModelElement()
    {
        var elementData = (ModelElementData)EditorElement.DataElement.ElementData;

        header          = elementData.Name;
        iconPath        = elementData.IconPath;
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
    }

    private void SetSaveElement()
    {
        var elementData = (SaveElementData)EditorElement.DataElement.ElementData;

        index           = elementData.Index;

        header          = elementData.ChapterName;
        description     = elementData.LocationName;

        iconPath        = elementData.ModelIconPath;

        playTime        = elementData.PlayTime;
        saveTime        = elementData.SaveTime;
    }

    private void SetInteractableSaveElement()
    {
        var elementData = (InteractableSaveElementData)EditorElement.DataElement.ElementData;

        header = elementData.InteractableName;
        iconPath = elementData.ModelIconPath;
    }

    private void SetChapterSaveElement()
    {
        var elementData = (ChapterSaveElementData)EditorElement.DataElement.ElementData;

        header = elementData.Name;
        description = elementData.PublicNotes;
    }

    private void SetPhaseSaveElement()
    {
        var elementData = (PhaseSaveElementData)EditorElement.DataElement.ElementData;

        header = elementData.Name;
        description = elementData.PublicNotes;

        descriptionText.text = description;
    }

    private void SetQuestSaveElement()
    {
        var elementData = (QuestSaveElementData)EditorElement.DataElement.ElementData;

        header = elementData.Name;
        description = elementData.PublicNotes;
    }

    private void SetObjectiveSaveElement()
    {
        var elementData = (ObjectiveSaveElementData)EditorElement.DataElement.ElementData;

        header = elementData.Name;
        description = elementData.PublicNotes;
    }

    private void SetTaskSaveElement()
    {
        var elementData = (TaskSaveElementData)EditorElement.DataElement.ElementData;

        header = elementData.Name;
        description = elementData.PublicNotes;
    }

    private void SetInteractionSaveElement()
    {
        var elementData = (InteractionSaveElementData)EditorElement.DataElement.ElementData;

        header = elementData.Default ? "Default" : TimeManager.FormatTime(elementData.StartTime) + " - " + TimeManager.FormatTime(elementData.EndTime);
        description = elementData.PublicNotes;
    }

    public void CloseElement()
    {
        id = 0;
        iconPath = null;
        infoIconPath = null;
        header = null;
        description = null;

        if(playTimeText != null)
            playTimeText.text = string.Empty;
    }

    public void ClosePoolable() { }
}
