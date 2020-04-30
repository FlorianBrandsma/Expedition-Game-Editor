using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ExButton : MonoBehaviour, IElement, IPoolable
{
    public Enums.ElementType elementType;
    
    public Text label;
    public RawImage icon;

    public Button Button                    { get { return GetComponent<Button>(); } }
    private SelectionElement Element        { get { return GetComponent<SelectionElement>(); } }

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
        icon.texture = Resources.Load<Texture2D>("Textures/Icons/UI/" + Element.selectionProperty.ToString());
    }

    public void SetElement()
    {
        switch (Element.data.dataController.DataType)
        {
            case Enums.DataType.Item:           SetItemElement();           break;
            case Enums.DataType.ChapterRegion:  SetChapterRegionElement();  break;
            default: Debug.Log("CASE MISSING: " + Element.data.dataController.DataType); break;
        }
    }

    private void SetItemElement()
    {
        var data = (ItemDataElement)Element.data.dataElement;

        label.text = data.originalName;
    }

    private void SetChapterRegionElement()
    {
        var data = (ChapterRegionDataElement)Element.data.dataElement;

        label.text = data.name;
    }

    public void CloseElement() { }

    public void ClosePoolable()
    {
        gameObject.SetActive(false);
    }
}
