using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class EditorTile : MonoBehaviour, IElement
{
    private SelectionElement element { get { return GetComponent<SelectionElement>(); } }
    private TileProperties properties;

    public RawImage icon;

    public void InitializeElement()
    {
        //properties = element.ListManager.listProperties.GetComponent<TileProperties>();
    }

    public void SetElement()
    {
        switch (element.route.data.DataController.DataType)
        {
            case Enums.DataType.Element:        SetElementElement();        break;
            case Enums.DataType.Terrain:        SetTerrainElement();        break;
            case Enums.DataType.Tile:           SetTileElement();           break;
            case Enums.DataType.TerrainTile:    SetTerrainTileElement();    break;
            case Enums.DataType.ObjectGraphic:  SetObjectGraphicElement();  break;
            case Enums.DataType.TerrainElement: SetTerrainElementElement(); break;
            case Enums.DataType.PhaseElement:   SetPhaseElementElement();   break;
            default: Debug.Log("CASE MISSING");                             break;
        }
    }

    private void SetElementElement()
    {
        var data = (ElementDataElement)element.route.data.DataElement;

        if(data.originalObjectGraphicIcon == null)
            icon.texture = Resources.Load<Texture2D>(data.objectGraphicIcon);
        else
            icon.texture = Resources.Load<Texture2D>(data.originalObjectGraphicIcon);
    }

    private void SetTerrainElement()
    {
        var data = (TerrainDataElement)element.route.data.DataElement;

        icon.texture = Resources.Load<Texture2D>(data.icon);
    }

    private void SetTileElement()
    {
        var data = (TileDataElement)element.route.data.DataElement;

        icon.texture = Resources.Load<Texture2D>(data.icon);
    }

    private void SetTerrainTileElement()
    {
        var data = (TerrainTileDataElement)element.route.data.DataElement;

        icon.texture = Resources.Load<Texture2D>(data.icon);
    }

    private void SetObjectGraphicElement()
    {
        //Required for initialization of the DataList. Better suited elsewhere
        element.route.data.DataController.DataList = new List<IDataElement>() { element.route.data.DataElement };

        var objectGraphicDataElement = (ObjectGraphicDataElement)element.route.data.DataElement;
        icon.texture = Resources.Load<Texture2D>(objectGraphicDataElement.Icon);
    }

    private void SetTerrainElementElement()
    {
        var data = (TerrainElementDataElement)element.route.data.DataElement;

        //There should be a distinction when an element is being set as result of a search, or by creating the list
        icon.texture = Resources.Load<Texture2D>(data.objectGraphicIcon);
    }

    private void SetPhaseElementElement()
    {
        var data = (PhaseElementDataElement)element.route.data.DataElement;

        icon.texture = Resources.Load<Texture2D>(data.objectGraphicIcon);
    }

    public void CloseElement()
    {

    }
}
