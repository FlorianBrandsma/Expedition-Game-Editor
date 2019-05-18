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
        switch (element.route.data.DataController.DataType)
        {
            case Enums.DataType.Element:        SetElementElement();        break;
            case Enums.DataType.Terrain:        SetTerrainElement();        break;
            case Enums.DataType.Tile:           SetTileElement();           break;
            case Enums.DataType.TerrainTile:    SetTerrainTileElement();    break;
            case Enums.DataType.ObjectGraphic:  SetObjectGraphicElement();  break;
            case Enums.DataType.ChapterElement: SetChapterElementElement(); break;
            default: Debug.Log("CASE MISSING");                             break;
        }
    }

    private void SetElementElement()
    {
        var data = element.route.data.ElementData.Cast<ElementDataElement>().FirstOrDefault();

        icon.texture = Resources.Load<Texture2D>(data.originalObjectGraphicIcon);
    }

    private void SetTerrainElement()
    {
        var data = element.route.data.ElementData.Cast<TerrainDataElement>().FirstOrDefault();

        icon.texture = Resources.Load<Texture2D>(data.icon);
    }

    private void SetTileElement()
    {
        var data = element.route.data.ElementData.Cast<TileDataElement>().FirstOrDefault();

        icon.texture = Resources.Load<Texture2D>(data.icon);
    }

    private void SetTerrainTileElement()
    {
        var data = element.route.data.ElementData.Cast<TerrainTileDataElement>().FirstOrDefault();

        icon.texture = Resources.Load<Texture2D>(data.icon);
    }

    private void SetObjectGraphicElement()
    {
        //Required for initialization of the DataList. Better suited elsewhere
        element.route.data.DataController.DataList = element.route.data.ElementData.Cast<ObjectGraphicDataElement>().ToList();

        var objectGraphicDataElement = element.route.data.ElementData.Cast<ObjectGraphicDataElement>().FirstOrDefault();

        icon.texture = Resources.Load<Texture2D>(objectGraphicDataElement.Icon);
    }

    private void SetChapterElementElement()
    {
        var data = element.route.data.ElementData.Cast<ChapterElementDataElement>().FirstOrDefault();

        //There should be a distinction when an element is being set as result of a search, or by creating the list
        icon.texture = Resources.Load<Texture2D>(data.objectGraphicIcon);
    }

    public void CloseElement()
    {

    }
}
