using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainTileController : MonoBehaviour, IDataController
{
    public Search.Tile searchParameters;

    private TerrainTileDataManager terrainTileDataManager = new TerrainTileDataManager();

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

    public void InitializeController()
    {
        terrainTileDataManager.InitializeManager(this);
    }

    public List<IDataElement> GetData(IEnumerable searchParameters)
    {
        return terrainTileDataManager.GetTerrainTileDataElements(searchParameters);
    }

    public void SetData(SelectionElement searchElement, SelectionElement.Data resultData)
    {
        var searchElementData = (TerrainTileDataElement)searchElement.data.dataElement;

        var terrainTileDataElement = DataList.Cast<TerrainTileDataElement>().Where(x => x.id == searchElementData.id).FirstOrDefault();

        switch (resultData.dataController.DataType)
        {
            case Enums.DataType.Tile:

                var resultElementData = (TileDataElement)resultData.dataElement;

                terrainTileDataElement.TileId = resultElementData.id;
                terrainTileDataElement.iconPath = resultElementData.icon;
                
                break;
        }

        searchElement.data.dataElement = terrainTileDataElement;
    }

    public void ToggleElement(IDataElement dataElement) { }
}