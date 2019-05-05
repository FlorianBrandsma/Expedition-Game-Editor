using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EditorPanel : MonoBehaviour, IElement
{
    public Text id;
    public Text header;
    public Text description;
    public RawImage icon;
    public RectTransform content;

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

        if (Element.elementType == Enums.ElementType.SearchPanel)
            properties.edit = false;
    }

    private void InitializeIcon()
    {
        content.offsetMin = new Vector2(icon.rectTransform.rect.width, content.offsetMin.y);
        icon.gameObject.SetActive(true);
    }

    private void InitializeEdit()
    {
        EditButton.ParentElement = Element;

        EditButton.InitializeElement(Element.ListManager, EditButton.selectionProperty);

        EditButton.gameObject.SetActive(true);

        content.offsetMax = new Vector2(-EditButton.GetComponent<RectTransform>().rect.width, content.offsetMax.y);
    }

    public void SetElement()
    {
        switch (Element.route.data.controller.DataType)
        {
            case Enums.DataType.Chapter:        SetChapterElement();        break;
            case Enums.DataType.Phase:          SetPhaseElement();          break;
            case Enums.DataType.Quest:          SetQuestElement();          break;
            case Enums.DataType.Step:           SetStepElement();           break;
            case Enums.DataType.StepElement:    SetStepElementElement();    break;
            case Enums.DataType.Task:           SetTaskElement();           break;
            case Enums.DataType.Region:         SetRegionElement();         break;
            case Enums.DataType.ObjectGraphic:  SetObjectGraphicElement();  break;
            case Enums.DataType.Element:        SetElementElement();        break;
            default: Debug.Log("YOU ARE MISSING THE DATATYPE");             break;
        }

        if (description == null) return;

        if (header.text == string.Empty)
            description.rectTransform.offsetMax = new Vector2(description.rectTransform.offsetMax.x, 0);
        else
            description.rectTransform.offsetMax = new Vector2(description.rectTransform.offsetMax.x, -header.rectTransform.rect.height);
    }

    private void SetChapterElement()
    {
        Data data = Element.route.data;
        ChapterDataElement dataElement = data.element.Cast<ChapterDataElement>().FirstOrDefault();
        
        id.text             = dataElement.id.ToString();
        header.text         = dataElement.originalName;
        description.text    = dataElement.originalDescription;

        if (properties.edit)
            EditButtonData = data;
    }

    private void SetPhaseElement()
    {
        Data data = Element.route.data;
        PhaseDataElement dataElement = data.element.Cast<PhaseDataElement>().FirstOrDefault();

        id.text = dataElement.id.ToString();
        header.text = dataElement.original_name;
        description.text = dataElement.original_description;

        if (properties.icon)
            IconTexture = Resources.Load<Texture2D>(dataElement.icon);

        if (properties.edit)
            EditButtonData = data;
    }

    private void SetQuestElement()
    {
        Data data = Element.route.data;
        QuestDataElement dataElement = data.element.Cast<QuestDataElement>().FirstOrDefault();

        id.text = dataElement.id.ToString();
        header.text = dataElement.original_name;

        if (properties.edit)
            EditButtonData = data;
    }

    private void SetStepElement()
    {
        Data data = Element.route.data;
        StepDataElement dataElement = data.element.Cast<StepDataElement>().FirstOrDefault();

        id.text = dataElement.id.ToString();
        header.text = dataElement.original_name;

        if (properties.icon)
            IconTexture = Resources.Load<Texture2D>(dataElement.icon);

        if (properties.edit)
            EditButtonData = data;
    }

    private void SetStepElementElement()
    {
        Data data = Element.route.data;
        StepElementDataElement dataElement = data.element.Cast<StepElementDataElement>().FirstOrDefault();

        id.text = dataElement.id.ToString();
        header.text = dataElement.name;

        if (properties.icon)
            IconTexture = Resources.Load<Texture2D>(dataElement.icon);

        if (properties.edit)
            EditButtonData = data;
    }

    private void SetTaskElement()
    {
        Data data = Element.route.data;
        TaskDataElement dataElement = data.element.Cast<TaskDataElement>().FirstOrDefault();

        id.text = dataElement.id.ToString();
        description.text = dataElement.original_description;

        if (properties.edit)
            EditButtonData = data;
    }

    private void SetRegionElement()
    {
        Data data = Element.route.data;
        RegionDataElement dataElement = data.element.Cast<RegionDataElement>().FirstOrDefault();

        id.text = dataElement.id.ToString();
        header.text = dataElement.original_name;

        if (properties.edit)
            EditButtonData = data;
    }

    private void SetObjectGraphicElement()
    {
        Data data = Element.route.data;
        ObjectGraphicDataElement dataElement = data.element.Cast<ObjectGraphicDataElement>().FirstOrDefault();

        id.text = dataElement.id.ToString();
        header.text = dataElement.originalName;

        IconTexture = Resources.Load<Texture2D>(dataElement.originalIcon);
    }

    private void SetElementElement()
    {
        Data data = Element.route.data;
        ElementDataElement dataElement = data.element.Cast<ElementDataElement>().FirstOrDefault();

        id.text = dataElement.id.ToString();
        header.text = dataElement.originalName;

        IconTexture = Resources.Load<Texture2D>(dataElement.originalObjectGraphicIcon);
    }

    public void CloseElement()
    {
        content.offsetMin = new Vector2(10, content.offsetMin.y);
        content.offsetMax = new Vector2(-10, content.offsetMax.y);

        header.text = string.Empty;
        id.text = string.Empty;

        if (description != null)
            description.text = string.Empty;

        if (properties.icon)
            icon.gameObject.SetActive(false);

        if (properties.edit)
            EditButton.gameObject.SetActive(false);
    }
}
