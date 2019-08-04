using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainTileSegment : MonoBehaviour, ISegment
{
    private RegionDataElement regionData;

    private TerrainController TerrainController { get { return GetComponent<TerrainController>(); } }
    private TerrainTileController TerrainTileController { get { return GetComponent<TerrainTileController>(); } }

    private SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor { get; set; }

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.editorController.PathController.dataEditor;
    }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.editorController.PathController.loaded) return;
        
        regionData = (RegionDataElement)SegmentController.Path.FindLastRoute(Enums.DataType.Region).data.dataElement;

        GetTerrainData();
        GetTerrainTileData();
    }

    private void GetTerrainData()
    {
        var searchParameters = new Search.Terrain();

        searchParameters.regionId = new List<int>() { regionData.id };

        TerrainController.DataList = TerrainController.GetData(new[] { searchParameters });
    }

    private void GetTerrainTileData()
    {
        var searchParameters = new Search.TerrainTile();

        searchParameters.regionId = new List<int>() { regionData.id };

        TerrainTileController.DataList = TerrainTileController.GetData(new[] { searchParameters });
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = TerrainController;
    }

    public void ApplySegment() { }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement)
    {
        selectionElement.data.dataElement.Update(); 
    }
}
