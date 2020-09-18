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
            dataList = TerrainTileDataManager.GetData(searchProperties),
            searchProperties = this.searchProperties
        };

        DataManager.ReplaceRouteData(this);
    }

    public void SetData(IElementData searchElementData, IElementData resultElementData)
    {
        var searchTerrainTileElementData = (TerrainTileElementData)searchElementData;

        switch (resultElementData.DataType)
        {
            case Enums.DataType.Tile:

                var resultTileElementData = (TileElementData)resultElementData;

                searchTerrainTileElementData.TileId     = resultTileElementData.Id;
                searchTerrainTileElementData.IconPath   = resultTileElementData.Icon;
                
                break;

            default: Debug.Log("CASE MISSING: " + resultElementData.DataType); break;
        }
    }

    public void ToggleElement(EditorElement editorElement) { }
}