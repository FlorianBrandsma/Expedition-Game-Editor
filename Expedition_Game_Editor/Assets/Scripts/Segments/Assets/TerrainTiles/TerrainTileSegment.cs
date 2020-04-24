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
        var searchProperties = new SearchProperties(Enums.DataType.Terrain);

        var searchParameters = searchProperties.searchParameters.Cast<Search.Terrain>().First();
        searchParameters.regionId = new List<int>() { regionData.Id };

        TerrainController.DataList = EditorManager.GetData(TerrainController, searchProperties);
    }

    private void GetTerrainTileData()
    {
        var searchProperties = new SearchProperties(Enums.DataType.TerrainTile);

        var searchParameters = searchProperties.searchParameters.Cast<Search.TerrainTile>().First();
        searchParameters.regionId = new List<int>() { regionData.Id };

        TerrainTileController.DataList = EditorManager.GetData(TerrainTileController, searchProperties);
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