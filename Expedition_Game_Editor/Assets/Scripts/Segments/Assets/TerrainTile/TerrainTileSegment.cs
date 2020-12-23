using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainTileSegment : MonoBehaviour, ISegment
{
    private RegionElementData regionData;
    
    private TerrainDataController       TerrainController       { get { return GetComponent<TerrainDataController>(); } }
    private TerrainTileDataController   TerrainTileController   { get { return GetComponent<TerrainTileDataController>(); } }

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public void InitializeDependencies() { }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;

        regionData = (RegionElementData)SegmentController.Path.FindLastRoute(Enums.DataType.Region).ElementData;

        GetTerrainData();
        GetTerrainTileData();
    }

    public void InitializeSegment()
    {
        InitializeData();
    }
    
    private void GetTerrainData()
    {
        var searchProperties = new SearchProperties(Enums.DataType.Terrain);

        var searchParameters = searchProperties.searchParameters.Cast<Search.Terrain>().First();
        searchParameters.regionId = new List<int>() { regionData.Id };

        TerrainController.GetData(searchProperties);
    }

    private void GetTerrainTileData()
    {
        var searchProperties = new SearchProperties(Enums.DataType.TerrainTile);

        var searchParameters = searchProperties.searchParameters.Cast<Search.TerrainTile>().First();
        searchParameters.regionId = new List<int>() { regionData.Id };

        TerrainTileController.GetData(searchProperties);
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = TerrainController;
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData)
    {
        var dataRequest = new DataRequest();

        mergedElementData.Update(dataRequest);
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
