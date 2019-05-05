using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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
        switch (element.route.data.controller.DataType)
        {
            case Enums.DataType.Element:        SetElementElement();        break;
            case Enums.DataType.Terrain:        SetTerrainElement();        break;
            case Enums.DataType.Tile:           SetTileElement();           break;
            case Enums.DataType.TerrainTile:    SetTerrainTileElement();    break;
            case Enums.DataType.ObjectGraphic:  SetObjectGraphicElement();  break;
            default: Debug.Log("CASE MISSING");                             break;
        }
    }

    private void SetElementElement()
    {
        ElementDataElement data = element.route.data.element.Cast<ElementDataElement>().FirstOrDefault();

        icon.texture = Resources.Load<Texture2D>(data.originalObjectGraphicIcon);
    }

    private void SetTerrainElement()
    {
        TerrainDataElement data = element.route.data.element.Cast<TerrainDataElement>().FirstOrDefault();

        icon.texture = Resources.Load<Texture2D>(data.icon);
    }

    private void SetTileElement()
    {
        TileDataElement data = element.route.data.element.Cast<TileDataElement>().FirstOrDefault();

        icon.texture = Resources.Load<Texture2D>(data.icon);
    }

    private void SetTerrainTileElement()
    {
        TerrainTileDataElement data = element.route.data.element.Cast<TerrainTileDataElement>().FirstOrDefault();

        icon.texture = Resources.Load<Texture2D>(data.icon);
    }

    private void SetObjectGraphicElement()
    {
        element.route.data.controller.DataList = element.route.data.element.Cast<ObjectGraphicDataElement>().ToList();

        var objectGraphicDataElement = element.route.data.element.Cast<ObjectGraphicDataElement>().FirstOrDefault();

        icon.texture = Resources.Load<Texture2D>(objectGraphicDataElement.Icon);
    }

    public void CloseElement()
    {

    }
}
