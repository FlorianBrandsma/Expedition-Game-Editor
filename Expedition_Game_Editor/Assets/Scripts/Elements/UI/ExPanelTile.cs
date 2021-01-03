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

    private int id;
    private string header;
    private string iconPath;
    private Texture iconTexture;

    private PanelTileProperties properties;

    private RectTransform IdRectTransform       { get { return idText.GetComponent<RectTransform>(); } }
    private RectTransform HeaderRectTransform   { get { return headerText.GetComponent<RectTransform>(); } }

    public EditorElement EditorElement  { get { return GetComponent<EditorElement>(); } }
    private EditorElement ElementChild  { get { return EditorElement.child; } }

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
        ElementChild.DataElement.InitializeElement(EditorElement.DataElement.DisplayManager, ElementChild.selectionType, properties.childProperty, SelectionManager.Property.None, ElementChild.uniqueSelection);

        ElementChild.gameObject.SetActive(true);

        content.offsetMax = new Vector2(-ElementChild.GetComponent<RectTransform>().rect.width, content.offsetMax.y);
    }

    private void SetId(bool enable)
    {
        if (idText == null) return;

        idText.text = id.ToString();
        
        idText.gameObject.SetActive(enable);
    }

    private void SetHeader(bool enable)
    {
        if (headerText == null) return;

        if(enable)
        {
            HeaderRectTransform.offsetMin = new Vector2(HeaderRectTransform.offsetMin.x, IdRectTransform.rect.height);

            headerText.alignment = TextAnchor.UpperLeft;

        } else {

            HeaderRectTransform.offsetMin = new Vector2(HeaderRectTransform.offsetMin.x, 0);

            headerText.alignment = TextAnchor.MiddleCenter;

            header = "Add new";
        }

        headerText.text = header;
    }

    private void SetIcon(bool enable)
    {
        if (iconParent == null) return;

        if(enable)
        {
            icon.texture = Resources.Load<Texture2D>(iconPath);

            content.offsetMin = new Vector2(iconParent.rect.width + 5, content.offsetMin.y);

        } else {

            content.offsetMin = new Vector2(10, content.offsetMin.y);
        }

        iconParent.gameObject.SetActive(enable);
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

        if (ElementChild != null)
            ElementChild.DataElement.Id = EditorElement.DataElement.Id;

        switch (EditorElement.DataElement.Data.dataController.DataType)
        {
            case Enums.DataType.WorldInteractable:  SetWorldInteractableElement();  break;
            case Enums.DataType.WorldObject:        SetWorldObjectElement();        break;
            case Enums.DataType.SceneActor:         SetSceneActorElement();         break;
            case Enums.DataType.SceneProp:          SetScenePropElement();          break;

            default: Debug.Log("CASE MISSING: " + EditorElement.DataElement.Data.dataController.DataType); break;
        }

        SetId(id != 0);
        SetHeader(id != 0);
        SetIcon(id != 0 && iconPath != null);

        SetChild(properties.childProperty != SelectionManager.Property.None && EditorElement.DataElement.Id != 0);
    }

    private void SetWorldInteractableElement()
    {
        var elementData = (WorldInteractableElementData)EditorElement.DataElement.ElementData;

        header = elementData.InteractableName;
        iconPath = elementData.ModelIconPath;
    }

    private void SetWorldObjectElement()
    {
        var elementData = (WorldObjectElementData)EditorElement.DataElement.ElementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
        {
            header = elementData.ModelName;
            iconPath = elementData.ModelIconPath;
        } else {
            header = elementData.OriginalData.ModelName;
            iconPath = elementData.OriginalData.ModelIconPath;
        }
    }

    private void SetSceneActorElement()
    {
        var elementData = (SceneActorElementData)EditorElement.DataElement.ElementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
        {
            header = elementData.InteractableName;
            iconPath = elementData.ModelIconPath;
        } else {
            header = elementData.OriginalData.InteractableName;
            iconPath = elementData.OriginalData.ModelIconPath;
        }
    }

    private void SetScenePropElement()
    {
        var elementData = (ScenePropElementData)EditorElement.DataElement.ElementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
        {
            header = elementData.ModelName;
            iconPath = elementData.ModelIconPath;
        } else {
            header = elementData.OriginalData.ModelName;
            iconPath = elementData.OriginalData.ModelIconPath;
        }
    }

    public void CloseElement()
    {
        id = 0;
        header = null;
        idText.text = string.Empty;
    }

    public void ClosePoolable() { }
}
