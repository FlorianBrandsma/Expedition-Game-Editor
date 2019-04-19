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
        switch (element.route.data_type)
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
    }

    private void SetChapterElement()
    {
        ChapterDataElement data = element.route.data.Cast<ChapterDataElement>().FirstOrDefault();
        
        id.text             = data.id.ToString();
        header.text         = data.original_name;
        description.text    = data.original_description;

        if (properties.icon)
            icon.texture = Resources.Load<Texture2D>(data.icon);

        if (properties.edit)
            edit_button.SetElementData(new[] { data }, element.route.data_type);
    }

    private void SetPhaseElement()
    {
        PhaseDataElement data = element.route.data.Cast<PhaseDataElement>().FirstOrDefault();

        id.text             = data.id.ToString();
        header.text         = data.original_name;
        description.text    = data.original_description;

        if (properties.icon)
            icon.texture = Resources.Load<Texture2D>(data.icon);

        if (properties.edit)
            edit_button.SetElementData(new[] { data }, element.route.data_type);
    }

    private void SetQuestElement()
    {
        QuestDataElement data = element.route.data.Cast<QuestDataElement>().FirstOrDefault();

        id.text = data.id.ToString();
        header.text = data.original_name;

        if (properties.edit)
            edit_button.SetElementData(new[] { data }, element.route.data_type);
    }

    private void SetStepElement()
    {
        StepDataElement data = element.route.data.Cast<StepDataElement>().FirstOrDefault();

        id.text = data.id.ToString();
        header.text = data.original_name;

        if (properties.icon)
            icon.texture = Resources.Load<Texture2D>(data.icon);

        if (properties.edit)
            edit_button.SetElementData(new[] { data }, element.route.data_type);
    }

    private void SetStepElementElement()
    {
        StepElementDataElement data = element.route.data.Cast<StepElementDataElement>().FirstOrDefault();

        header.text = data.table;
        id.text = data.id.ToString();

        if (properties.icon)
            icon.texture = Resources.Load<Texture2D>(data.icon);

        if (properties.edit)
            edit_button.SetElementData(new[] { data }, element.route.data_type);
    }

    private void SetTaskElement()
    {
        TaskDataElement data = element.route.data.Cast<TaskDataElement>().FirstOrDefault();

        header.text = data.table;
        id.text = data.id.ToString();
        description.text = data.original_description;
    }

    private void SetRegionElement()
    {
        RegionDataElement data = element.route.data.Cast<RegionDataElement>().FirstOrDefault();

        id.text = data.id.ToString();
        header.text = data.original_name;

        if (properties.edit)
            edit_button.SetElementData(new[] { data }, element.route.data_type);
    }

    public void CloseElement()
    {
        content.offsetMin = new Vector2(5, content.offsetMin.y);
        content.offsetMax = new Vector2(-5, content.offsetMax.y);

        header.text = string.Empty;
        id.text = string.Empty;
        description.text = string.Empty;

        if (properties.icon)
            icon.gameObject.SetActive(false);

        if (properties.edit)
            edit_button.gameObject.SetActive(false);
    }
}
