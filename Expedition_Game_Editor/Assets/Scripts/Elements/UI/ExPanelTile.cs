using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ExPanelTile : MonoBehaviour, IElement, IPoolable
{
    public Enums.ElementType elementType;

    public Text idText;
    public Text headerText;
    public RectTransform iconParent;
    public RawImage icon;
    public RectTransform content;
    public Image background;

    private string header;
    private string iconPath;

    private PanelTileProperties properties;

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
            InitializeIcon();
            icon.texture = value;
        }
    }

    private Data ChildButtonData
    {
        get { return ElementChild.DataElement.Data; }
        set
        {
            InitializeEdit();
            ElementChild.DataElement.Data = value;
        }
    }

    public IPoolable Instantiate()
    {
        var newElement = Instantiate(this);

        SelectionElementManager.Add(newElement.EditorElement);

        return newElement;
    }

    public void InitializeElement()
    {
        properties = (PanelTileProperties)EditorElement.DataElement.DisplayManager.Display.Properties;
    }

    public void InitializeChildElement()
    {
        if (properties.childProperty == SelectionManager.Property.None) return;

        ElementChild.DataElement.Data   = EditorElement.DataElement.Data;
        ElementChild.DataElement.Id     = EditorElement.DataElement.Id;

        ElementChild.DataElement.Path   = EditorElement.DataElement.Path;

        InitializeEdit();
    }

    private void InitializeIcon()
    {
        content.offsetMin = new Vector2(iconParent.rect.width, content.offsetMin.y);
        iconParent.gameObject.SetActive(true);
    }

    private void InitializeEdit()
    {
        ElementChild.DataElement.InitializeElement(EditorElement.DataElement.DisplayManager, ElementChild.selectionType, properties.childProperty);

        ElementChild.gameObject.SetActive(true);

        content.offsetMax = new Vector2(-ElementChild.GetComponent<RectTransform>().rect.width, content.offsetMax.y);
    }

    public void UpdateElement()
    {
        SetElement();
    }

    public void SetElement()
    {
        switch (EditorElement.DataElement.Data.dataController.DataType)
        {
            case Enums.DataType.WorldInteractable:  SetWorldInteractableElement();  break;
            case Enums.DataType.WorldObject:        SetWorldObjectElement();        break;
            case Enums.DataType.SceneActor:         SetSceneActorElement();         break;
            case Enums.DataType.SceneProp:          SetScenePropElement();          break;

            default: Debug.Log("CASE MISSING: " + EditorElement.DataElement.Data.dataController.DataType); break;
        }
    }

    private void SetWorldInteractableElement()
    {
        var elementData = (WorldInteractableElementData)EditorElement.DataElement.ElementData;

        idText.text = elementData.Id.ToString();
        headerText.text = elementData.InteractableName;

        if (properties.icon)
            IconTexture = Resources.Load<Texture2D>(elementData.ModelIconPath);
    }

    private void SetWorldObjectElement()
    {
        var elementData = (WorldObjectElementData)EditorElement.DataElement.ElementData;

        idText.text = elementData.Id.ToString();
        headerText.text = elementData.ModelName;

        if (properties.icon)
            IconTexture = Resources.Load<Texture2D>(elementData.ModelIconPath);
    }

    private void SetSceneActorElement()
    {
        var elementData = (SceneActorElementData)EditorElement.DataElement.ElementData;

        idText.text = elementData.Id.ToString();
        headerText.text = elementData.InteractableName;

        if (properties.icon)
            IconTexture = Resources.Load<Texture2D>(elementData.ModelIconPath);
    }

    private void SetScenePropElement()
    {
        var elementData = (ScenePropElementData)EditorElement.DataElement.ElementData;

        idText.text = elementData.Id.ToString();
        headerText.text = elementData.ModelName;

        if (properties.icon)
            IconTexture = Resources.Load<Texture2D>(elementData.ModelIconPath);
    }

    public void CloseElement()
    {
        content.offsetMin = new Vector2(5, content.offsetMin.y);
        content.offsetMax = new Vector2(-5, content.offsetMax.y);

        headerText.text = string.Empty;
        idText.text = string.Empty;

        if (properties.icon)
            iconParent.gameObject.SetActive(false);

        if (properties.childProperty != SelectionManager.Property.None)
            ElementChild.gameObject.SetActive(false);
    }

    public void ClosePoolable()
    {
        //gameObject.SetActive(false);
    }
}
