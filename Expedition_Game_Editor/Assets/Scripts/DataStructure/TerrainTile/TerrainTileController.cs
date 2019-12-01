using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainTileController : MonoBehaviour, IDataController
{
    public Search.Tile searchParameters;

    public IDataManager DataManager { get; set; }
    
    public IDisplay Display                     { get { return GetComponent<IDisplay>(); } }
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }

    public Enums.DataType DataType              { get { return Enums.DataType.TerrainTile; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    public List<IDataElement> DataList          { get; set; }

    public IEnumerable SearchParameters
    {
        get { return new[] { searchParameters }; }
        set { searchParameters = value.Cast<Search.Tile>().FirstOrDefault(); }
    }

    public TerrainTileController()
    {
        DataManager = new TerrainTileDataManager(this);
    }

    public void SetData(SelectionElement searchElement, IDataElement resultData)
    {
        var terrainTileData = (TerrainTileDataElement)searchElement.data.dataElement;

        switch (((GeneralData)resultData).DataType)
        {
            case Enums.DataType.Tile:

                var resultElementData = (TileDataElement)resultData;

                terrainTileData.TileId = resultElementData.Id;
                terrainTileData.iconPath = resultElementData.icon;
                
                break;
        }
    }

    public void ToggleElement(IDataElement dataElement) { }
}