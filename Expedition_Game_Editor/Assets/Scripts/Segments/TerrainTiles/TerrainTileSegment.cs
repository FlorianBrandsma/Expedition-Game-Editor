﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainTileSegment : MonoBehaviour, ISegment
{
    private RegionDataElement regionData;
    
    private TerrainController TerrainController { get { return GetComponent<TerrainController>(); } }
    private TerrainTileController TerrainTileController { get { return GetComponent<TerrainTileController>(); } }

    public SegmentController SegmentController { get { return GetComponent<SegmentController>(); } }

    public IEditor DataEditor { get; set; }

    public void InitializeDependencies() { }

    public void InitializeSegment()
    {
        InitializeData();
    }

    public void InitializeData()
    {
        if (SegmentController.Loaded) return;
        
        regionData = (RegionDataElement)SegmentController.Path.FindLastRoute(Enums.DataType.Region).data.dataElement;

        GetTerrainData();
        GetTerrainTileData();
    }

    private void GetTerrainData()
    {
        var searchParameters = new Search.Terrain();

        searchParameters.regionId = new List<int>() { regionData.Id };

        TerrainController.DataList = EditorManager.GetData(TerrainController, new[] { searchParameters });
    }

    private void GetTerrainTileData()
    {
        var searchParameters = new Search.TerrainTile();

        searchParameters.regionId = new List<int>() { regionData.Id };

        TerrainTileController.DataList = EditorManager.GetData(TerrainTileController, new[] { searchParameters });
    }

    public void OpenSegment()
    {
        if (GetComponent<IDisplay>() != null)
            GetComponent<IDisplay>().DataController = TerrainController;
    }

    public void CloseSegment() { }

    public void SetSearchResult(SelectionElement selectionElement)
    {
        selectionElement.data.dataElement.Update(); 
    }
}
