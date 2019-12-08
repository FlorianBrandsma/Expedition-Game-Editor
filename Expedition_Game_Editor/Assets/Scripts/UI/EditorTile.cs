using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class EditorTile : MonoBehaviour, IElement
{
    public RawImage icon;
    public RawImage iconBase;

    private string iconPath;

    private SelectionElement Element { get { return GetComponent<SelectionElement>(); } }
    private TileProperties properties;

    public Color ElementColor { set { } }

    public void InitializeElement()
    {
        //properties = element.ListManager.listProperties.GetComponent<TileProperties>();
    }

    public void SetElement()
    {
        switch (Element.data.dataController.DataType)
        {
            case Enums.DataType.Icon:               SetIconElement();               break;
            case Enums.DataType.Interactable:       SetInteractableElement();       break;
            case Enums.DataType.Terrain:            SetTerrainElement();            break;
            case Enums.DataType.Tile:               SetTileElement();               break;
            case Enums.DataType.TerrainTile:        SetTerrainTileElement();        break;
            case Enums.DataType.ObjectGraphic:      SetObjectGraphicElement();      break;
            case Enums.DataType.SceneInteractable:  SetSceneInteractableElement();  break;
            case Enums.DataType.PhaseInteractable:  SetPhaseInteractableElement();  break;

            default: Debug.Log("CASE MISSING: " + Element.data.dataController.DataType); break;
        }
    }

    private void SetIconElement()
    {
        var dataElement = (IconDataElement)Element.data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.Path;
        else
            iconPath = dataElement.originalPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);

        if(dataElement.baseIconPath != "")
            iconBase.texture = Resources.Load<Texture2D>(dataElement.baseIconPath);
    }

    private void SetInteractableElement()
    {
        var dataElement = (InteractableDataElement)Element.data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.objectGraphicIconPath;
        else
            iconPath = dataElement.originalObjectGraphicIconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetTerrainElement()
    {
        var dataElement = (TerrainDataElement)Element.data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.iconPath;
        else
            iconPath = dataElement.originalIconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetTileElement()
    {
        var dataElement = (TileDataElement)Element.data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.icon;
        else
            iconPath = dataElement.icon;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetTerrainTileElement()
    {
        var dataElement = (TerrainTileDataElement)Element.data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.iconPath;
        else
            iconPath = dataElement.originalIconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetObjectGraphicElement()
    {
        var dataElement = (ObjectGraphicDataElement)Element.data.dataElement;
        
        iconPath = dataElement.iconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetSceneInteractableElement()
    {
        var dataElement = (SceneInteractableDataElement)Element.data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.objectGraphicIconPath;
        else
            iconPath = dataElement.originalObjectGraphicIconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetPhaseInteractableElement()
    {
        var dataElement = (PhaseInteractableDataElement)Element.data.dataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.objectGraphicIcon;
        else
            iconPath = dataElement.originalObjectGraphicIcon;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    public void CloseElement()
    {

    }
}
