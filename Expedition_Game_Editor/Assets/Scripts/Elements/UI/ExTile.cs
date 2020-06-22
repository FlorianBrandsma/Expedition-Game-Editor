using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ExTile : MonoBehaviour, IElement, IPoolable
{
    public Enums.ElementType elementType;

    public RawImage icon;
    public RawImage iconBase;

    private string iconPath;

    public EditorElement EditorElement      { get { return GetComponent<EditorElement>(); } }

    public Color ElementColor               { set { } }

    public Transform Transform              { get { return GetComponent<Transform>(); } }
    public Enums.ElementType ElementType    { get { return elementType; } }
    public int Id                           { get; set; }
    public bool IsActive                    { get { return gameObject.activeInHierarchy; } }

    public IPoolable Instantiate()
    {
        var newElement = Instantiate(this);

        SelectionElementManager.Add(newElement.EditorElement);

        return newElement;
    }

    public void InitializeElement() { }

    public void UpdateElement()
    {
        SetElement();
    }

    public void SetElement()
    {
        switch (EditorElement.DataElement.data.dataController.DataType)
        {
            case Enums.DataType.Icon:               SetIconElement();               break;
            case Enums.DataType.Interactable:       SetInteractableElement();       break;
            case Enums.DataType.Terrain:            SetTerrainElement();            break;
            case Enums.DataType.Tile:               SetTileElement();               break;
            case Enums.DataType.TerrainTile:        SetTerrainTileElement();        break;
            case Enums.DataType.ObjectGraphic:      SetObjectGraphicElement();      break;
            case Enums.DataType.WorldInteractable:  SetWorldInteractableElement();  break;

            default: Debug.Log("CASE MISSING: " + EditorElement.DataElement.data.dataController.DataType); break;
        }
    }

    private void SetIconElement()
    {
        var elementData = (IconElementData)EditorElement.DataElement.data.elementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
            iconPath = elementData.Path;
        else
            iconPath = elementData.originalPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);

        if(elementData.baseIconPath != "")
            iconBase.texture = Resources.Load<Texture2D>(elementData.baseIconPath);
    }

    private void SetInteractableElement()
    {
        var elementData = (InteractableElementData)EditorElement.DataElement.data.elementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
            iconPath = elementData.objectGraphicIconPath;
        else
            iconPath = elementData.originalObjectGraphicIconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetTerrainElement()
    {
        var elementData = (TerrainElementData)EditorElement.DataElement.data.elementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
            iconPath = elementData.iconPath;
        else
            iconPath = elementData.originalIconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetTileElement()
    {
        var elementData = (TileElementData)EditorElement.DataElement.data.elementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
            iconPath = elementData.icon;
        else
            iconPath = elementData.icon;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetTerrainTileElement()
    {
        var elementData = (TerrainTileElementData)EditorElement.DataElement.data.elementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
            iconPath = elementData.iconPath;
        else
            iconPath = elementData.originalIconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetObjectGraphicElement()
    {
        var elementData = (ObjectGraphicElementData)EditorElement.DataElement.data.elementData;
        
        iconPath = elementData.iconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetWorldInteractableElement()
    {
        var elementData = (WorldInteractableElementData)EditorElement.DataElement.data.elementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
            iconPath = elementData.objectGraphicIconPath;
        else
            iconPath = elementData.originalObjectGraphicIconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    public void CloseElement() { }

    public void ClosePoolable()
    {
        //gameObject.SetActive(false);
    }
}
