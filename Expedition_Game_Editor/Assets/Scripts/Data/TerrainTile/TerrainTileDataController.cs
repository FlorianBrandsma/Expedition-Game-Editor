using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainTileDataController : MonoBehaviour, IDataController
{
    public SearchProperties searchProperties;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    
    public Data Data                            { get; set; }
    public Enums.DataType DataType              { get { return Enums.DataType.TerrainTile; } }
    public Enums.DataCategory DataCategory      { get { return Enums.DataCategory.None; } }
    
    public SearchProperties SearchProperties
    {
        get { return searchProperties; }
        set { searchProperties = value; }
    }
    
    public void InitializeController()
    {
        SearchProperties.Initialize();
    }

    public void GetData()
    {
        GetData(searchProperties);
    }

    public void GetData(SearchProperties searchProperties)
    {
        Data = new Data()
        {
            dataController = this,
            dataList = TerrainTileDataManager.GetData(searchProperties)
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(DataElement searchElement, IElementData resultData)
    {
        var terrainTileData = (TerrainTileElementData)searchElement.ElementData;

        switch (resultData.DataType)
        {
            case Enums.DataType.Tile:

                var resultElementData = (TileElementData)resultData;

                terrainTileData.TileId = resultElementData.Id;
                terrainTileData.IconPath = resultElementData.Icon;
                
                break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}