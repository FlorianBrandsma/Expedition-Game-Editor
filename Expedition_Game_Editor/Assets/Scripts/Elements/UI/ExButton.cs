using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ExButton : MonoBehaviour, IElement, IPoolable
{
    public Enums.ElementType elementType;
    
    public Text label;
    public RawImage icon;

    public Button Button                    { get { return GetComponent<Button>(); } }
    private EditorElement EditorElement     { get { return GetComponent<EditorElement>(); } }

    public Color ElementColor               { set { } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return elementType; } }
    public int Id                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public IPoolable Instantiate()
    {
        return Instantiate(this);
    }

    public void InitializeElement()
    {
        icon.texture = Resources.Load<Texture2D>("Textures/Icons/UI/" + EditorElement.selectionProperty.ToString());
    }

    public void UpdateElement()
    {
        SetElement();
    }

    public void SetElement()
    {
        switch (EditorElement.DataElement.Data.dataController.DataType)
        {
            case Enums.DataType.Item:           SetItemElement();           break;
            case Enums.DataType.ChapterRegion:  SetChapterRegionElement();  break;

            default: Debug.Log("CASE MISSING: " + EditorElement.DataElement.Data.dataController.DataType); break;
        }
    }

    private void SetItemElement()
    {
        var data = (ItemElementData)EditorElement.DataElement.ElementData;

        label.text = data.OriginalData.Name;
    }

    private void SetChapterRegionElement()
    {
        var data = (ChapterRegionElementData)EditorElement.DataElement.ElementData;

        label.text = data.Name;
    }

    public void CloseElement() { }

    public void ClosePoolable()
    {
        gameObject.SetActive(false);
    }
}
