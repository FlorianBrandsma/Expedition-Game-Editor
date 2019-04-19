using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EditorTile : MonoBehaviour, IElement
{
    private SelectionElement element { get { return GetComponent<SelectionElement>(); } }
    private TileProperties properties;

    public RawImage icon;

    public void InitializeElement()
    {
        properties = element.listManager.listProperties.GetComponent<TileProperties>();
    }

    public void SetElement()
    {
        switch (element.route.data_type)
        {
            case Enums.DataType.Element:    SetElementElement();    break;
            case Enums.DataType.Terrain:    SetTerrainElement();    break;
            case Enums.DataType.Tile:       SetTileElement();       break;
            default: Debug.Log("CASE MISSING");                     break;
        }
    }

    private void SetElementElement()
    {
        ElementDataElement data = element.route.data.Cast<ElementDataElement>().FirstOrDefault();

        icon.texture = Resources.Load<Texture2D>(data.icon);
    }

    private void SetTerrainElement()
    {

    }

    private void SetTileElement()
    {

    }

    public void CloseElement()
    {

    }
}
