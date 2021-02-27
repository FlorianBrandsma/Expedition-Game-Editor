using UnityEngine;
using UnityEngine.UI;

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
    public int PoolId                           { get; set; }
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
        switch (EditorElement.DataElement.Data.dataController.DataType)
        {
            case Enums.DataType.Icon:               SetIconElement();               break;
            case Enums.DataType.Interactable:       SetInteractableElement();       break;
            case Enums.DataType.Terrain:            SetTerrainElement();            break;
            case Enums.DataType.Tile:               SetTileElement();               break;
            case Enums.DataType.TerrainTile:        SetTerrainTileElement();        break;
            case Enums.DataType.Model:              SetModelElement();              break;
            case Enums.DataType.WorldInteractable:  SetWorldInteractableElement();  break;

            case Enums.DataType.Game:               SetGameElement();               break;

            default: Debug.Log("CASE MISSING: " + EditorElement.DataElement.Data.dataController.DataType); break;
        }
    }

    private void SetIconElement()
    {
        var elementData = (IconElementData)EditorElement.DataElement.ElementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
            iconPath = elementData.Path;
        else
            iconPath = elementData.OriginalData.Path;

        icon.texture = Resources.Load<Texture2D>(iconPath);

        if(elementData.BaseIconPath != "")
            iconBase.texture = Resources.Load<Texture2D>(elementData.BaseIconPath);
    }

    private void SetInteractableElement()
    {
        var elementData = (InteractableElementData)EditorElement.DataElement.ElementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
            iconPath = elementData.ModelIconPath;
        else
            iconPath = elementData.OriginalData.ModelIconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetTerrainElement()
    {
        var elementData = (TerrainElementData)EditorElement.DataElement.ElementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
            iconPath = elementData.IconPath;
        else
            iconPath = elementData.OriginalData.IconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetTileElement()
    {
        var elementData = (TileElementData)EditorElement.DataElement.ElementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
            iconPath = elementData.Icon;
        else
            iconPath = elementData.Icon;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetTerrainTileElement()
    {
        var elementData = (TerrainTileElementData)EditorElement.DataElement.ElementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
            iconPath = elementData.IconPath;
        else
            iconPath = elementData.OriginalData.IconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetModelElement()
    {
        var elementData = (ModelElementData)EditorElement.DataElement.ElementData;
        
        iconPath = elementData.IconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetWorldInteractableElement()
    {
        var elementData = (WorldInteractableElementData)EditorElement.DataElement.ElementData;

        if (EditorElement.selectionProperty == SelectionManager.Property.Get)
            iconPath = elementData.ModelIconPath;
        else
            iconPath = elementData.OriginalData.ModelIconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetGameElement()
    {
        var elementData = (GameElementData)EditorElement.DataElement.ElementData;

        iconPath = elementData.IconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    public void CloseElement() { }

    public void ClosePoolable()
    {
        //gameObject.SetActive(false);
    }
}
