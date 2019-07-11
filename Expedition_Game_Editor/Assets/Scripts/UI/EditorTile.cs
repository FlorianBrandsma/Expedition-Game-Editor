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

    public void InitializeElement()
    {
        //properties = element.ListManager.listProperties.GetComponent<TileProperties>();
    }

    public void SetElement()
    {
        switch (Element.route.data.DataController.DataType)
        {
            case Enums.DataType.Icon:           SetIconElement();           break;
            case Enums.DataType.Interactable:        SetElementElement();        break;
            case Enums.DataType.Terrain:        SetTerrainElement();        break;
            case Enums.DataType.Tile:           SetTileElement();           break;
            case Enums.DataType.TerrainTile:    SetTerrainTileElement();    break;
            case Enums.DataType.ObjectGraphic:  SetObjectGraphicElement();  break;
            case Enums.DataType.TerrainInteractable: SetTerrainElementElement(); break;
            case Enums.DataType.PhaseInteractable:   SetPhaseElementElement();   break;
            default: Debug.Log("CASE MISSING: " + Element.route.data.DataController.DataType); break;
        }
    }

    private void SetIconElement()
    {
        var dataElement = (IconDataElement)Element.route.data.DataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.Path;
        else
            iconPath = dataElement.originalPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);

        if(dataElement.baseIconPath != "")
            iconBase.texture = Resources.Load<Texture2D>(dataElement.baseIconPath);
    }

    private void SetElementElement()
    {
        var dataElement = (InteractableDataElement)Element.route.data.DataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.objectGraphicIconPath;
        else
            iconPath = dataElement.originalObjectGraphicIconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetTerrainElement()
    {
        var dataElement = (TerrainDataElement)Element.route.data.DataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.iconPath;
        else
            iconPath = dataElement.originalIconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetTileElement()
    {
        var dataElement = (TileDataElement)Element.route.data.DataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.icon;
        else
            iconPath = dataElement.icon;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetTerrainTileElement()
    {
        var dataElement = (TerrainTileDataElement)Element.route.data.DataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.iconPath;
        else
            iconPath = dataElement.originalIconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetObjectGraphicElement()
    {
        var dataElement = (ObjectGraphicDataElement)Element.route.data.DataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.iconPath;
        else
            iconPath = dataElement.originalIconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetTerrainElementElement()
    {
        var dataElement = (TerrainInteractableDataElement)Element.route.data.DataElement;

        if (Element.selectionProperty == SelectionManager.Property.Get)
            iconPath = dataElement.objectGraphicIconPath;
        else
            iconPath = dataElement.originalObjectGraphicIconPath;

        icon.texture = Resources.Load<Texture2D>(iconPath);
    }

    private void SetPhaseElementElement()
    {
        var dataElement = (PhaseInteractableDataElement)Element.route.data.DataElement;

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
