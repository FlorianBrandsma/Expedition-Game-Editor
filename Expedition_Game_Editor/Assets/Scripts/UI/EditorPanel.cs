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

    public SelectionElement edit_button;

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
        switch (element.data_type)
        {
            case DataManager.Type.Chapter:
                SetChapterElement();
                break;
            case DataManager.Type.Phase:
                SetPhaseElement();
                break;
        }
    }

    private void SetChapterElement()
    {
        ChapterDataElement data = element.data.Cast<ChapterDataElement>().FirstOrDefault();
        
        id.text             = data.id.ToString();
        header.text         = data.original_name;
        description.text    = data.original_description;

        if (properties.icon)
            icon.texture = Resources.Load<Texture2D>(data.icon);

        if (properties.edit)
            edit_button.SetElementData(new[] { data }, element.data_type);     
    }

    private void SetPhaseElement()
    {
        PhaseDataElement data = element.data.Cast<PhaseDataElement>().FirstOrDefault();

        id.text             = data.id.ToString();
        header.text         = data.original_name;
        description.text    = data.original_description;

        if (properties.icon)
            icon.texture = Resources.Load<Texture2D>(data.icon);

        if (properties.edit)
            edit_button.SetElementData(new[] { data }, element.data_type);
    }

    public void CloseElement()
    {
        content.offsetMin = new Vector2(5, content.offsetMin.y);
        content.offsetMax = new Vector2(-5, content.offsetMax.y);

        if (properties.icon)
            icon.gameObject.SetActive(false);

        if (properties.edit)
            edit_button.gameObject.SetActive(false);
    }
}
