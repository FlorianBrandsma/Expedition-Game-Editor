using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EditorPanel : MonoBehaviour, IElement
{
    private SelectionElement element { get { return GetComponent<SelectionElement>(); } }
    private PanelProperties properties;

    public Text id;
    public Text header;
    public Text description;
    public RawImage icon;

    private SelectionElement edit_button { get { return element.child; } }

    public RectTransform content;

    public void InitializeElement()
    {
        properties = element.listManager.listProperties.GetComponent<PanelProperties>();
        
        if (properties.icon)
        {
            content.offsetMin = new Vector2(icon.rectTransform.rect.width, content.offsetMin.y);
            icon.gameObject.SetActive(true);
        }

        if (properties.edit)
        {
            edit_button.parent_element = element;

            edit_button.InitializeElement(element.listManager, SelectionManager.Property.Edit);
          
            edit_button.gameObject.SetActive(true);

            content.offsetMax = new Vector2(-edit_button.GetComponent<RectTransform>().rect.width, content.offsetMax.y);
        }
    }

    public void SetElement()
    {
        switch (element.route.data.controller.data_type)
        {
            case Enums.DataType.Chapter:        SetChapterElement();        break;
            case Enums.DataType.Phase:          SetPhaseElement();          break;
            case Enums.DataType.Quest:          SetQuestElement();          break;
            case Enums.DataType.Step:           SetStepElement();           break;
            case Enums.DataType.StepElement:    SetStepElementElement();    break;
            case Enums.DataType.Task:           SetTaskElement();           break;
            case Enums.DataType.Region:         SetRegionElement();         break;
            default: Debug.Log("YOU ARE MISSING THE DATATYPE");             break;
        }

        if(header.text == string.Empty)
            description.rectTransform.offsetMax = new Vector2(description.rectTransform.offsetMax.x, 0);
         else
            description.rectTransform.offsetMax = new Vector2(description.rectTransform.offsetMax.x, -header.rectTransform.rect.height);
    }

    private void SetChapterElement()
    {
        Data data = element.route.data;
        ChapterDataElement data_element = data.element.Cast<ChapterDataElement>().FirstOrDefault();
        
        id.text             = data_element.id.ToString();
        header.text         = data_element.original_name;
        description.text    = data_element.original_description;

        if (properties.icon)
            icon.texture = Resources.Load<Texture2D>(data_element.icon);

        if (properties.edit)
            edit_button.route.data = data;
    }

    private void SetPhaseElement()
    {
        Data data = element.route.data;
        PhaseDataElement data_element = data.element.Cast<PhaseDataElement>().FirstOrDefault();

        id.text = data_element.id.ToString();
        header.text = data_element.original_name;
        description.text = data_element.original_description;

        if (properties.icon)
            icon.texture = Resources.Load<Texture2D>(data_element.icon);

        if (properties.edit)
            edit_button.route.data = data;
    }

    private void SetQuestElement()
    {
        Data data = element.route.data;
        QuestDataElement data_element = data.element.Cast<QuestDataElement>().FirstOrDefault();

        id.text = data_element.id.ToString();
        header.text = data_element.original_name;

        if (properties.edit)
            edit_button.route.data = data;
    }

    private void SetStepElement()
    {
        Data data = element.route.data;
        StepDataElement data_element = data.element.Cast<StepDataElement>().FirstOrDefault();

        id.text = data_element.id.ToString();
        header.text = data_element.original_name;

        if (properties.icon)
            icon.texture = Resources.Load<Texture2D>(data_element.icon);

        if (properties.edit)
            edit_button.route.data = data;
    }

    private void SetStepElementElement()
    {
        Data data = element.route.data;
        StepElementDataElement data_element = data.element.Cast<StepElementDataElement>().FirstOrDefault();

        id.text = data_element.id.ToString();
        header.text = data_element.name;

        if (properties.icon)
            icon.texture = Resources.Load<Texture2D>(data_element.icon);

        if (properties.edit)
            edit_button.route.data = data;
    }

    private void SetTaskElement()
    {
        Data data = element.route.data;
        TaskDataElement data_element = data.element.Cast<TaskDataElement>().FirstOrDefault();

        id.text = data_element.id.ToString();
        description.text = data_element.original_description;

        if (properties.edit)
            edit_button.route.data = data;
    }

    private void SetRegionElement()
    {
        Data data = element.route.data;
        RegionDataElement data_element = data.element.Cast<RegionDataElement>().FirstOrDefault();

        id.text = data_element.id.ToString();
        header.text = data_element.original_name;

        if (properties.edit)
            edit_button.route.data = data;
    }

    public void CloseElement()
    {
        content.offsetMin = new Vector2(10, content.offsetMin.y);
        content.offsetMax = new Vector2(-10, content.offsetMax.y);

        header.text = string.Empty;
        id.text = string.Empty;
        description.text = string.Empty;

        if (properties.icon)
            icon.gameObject.SetActive(false);

        if (properties.edit)
            edit_button.gameObject.SetActive(false);
    }
}
