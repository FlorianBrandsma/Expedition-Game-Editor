﻿using UnityEngine;
using System.Linq;

public class WorldObjectPositionCoordinateSegment : MonoBehaviour, ISegment
{
    private RegionElementData regionData;

    public ExInputNumber xInputField, yInputField, zInputField;
    public ExToggle bindToTile;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public WorldObjectEditor WorldObjectEditor  { get { return (WorldObjectEditor)DataEditor; } }

    #region Data properties
    private float PositionX
    {
        get { return WorldObjectEditor.PositionX; }
        set { WorldObjectEditor.PositionX = value; }
    }

    private float PositionY
    {
        get { return WorldObjectEditor.PositionY; }
        set { WorldObjectEditor.PositionY = value; }
    }

    private float PositionZ
    {
        get { return WorldObjectEditor.PositionZ; }
        set { WorldObjectEditor.PositionZ = value; }
    }

    private int TerrainId
    {
        get { return WorldObjectEditor.TerrainId; }
        set { WorldObjectEditor.TerrainId = value; }
    }

    private int TerrainTileId
    {
        get { return WorldObjectEditor.TerrainTileId; }
        set { WorldObjectEditor.TerrainTileId = value; }
    }
    #endregion
    
    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }
    
    public void InitializeData() { }

    public void InitializeSegment()
    {
        regionData = (RegionElementData)SegmentController.Path.FindLastRoute(Enums.DataType.Region).ElementData;

        var regionSize = new Vector2(regionData.RegionSize * regionData.TerrainSize * regionData.TileSize,
                                     regionData.RegionSize * regionData.TerrainSize * regionData.TileSize);

        xInputField.max = regionSize.x - 0.01f;
        zInputField.max = regionSize.y - 0.01f;
    }

    public void OpenSegment()
    {
        xInputField.Value = PositionX;
        yInputField.Value = PositionY;
        zInputField.Value = PositionZ;
        
        bindToTile.Toggle.isOn = TerrainTileId != 0;

        UpdateTile();

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData mergedElementData, IElementData resultElementData) { }

    public void UpdatePositionX()
    {
        PositionX = xInputField.Value;

        UpdateTile();

        DataEditor.UpdateEditor();
    }

    public void UpdatePositionY()
    {
        PositionY = yInputField.Value;
        
        DataEditor.UpdateEditor();
    }

    public void UpdatePositionZ()
    {
        PositionZ = zInputField.Value;

        UpdateTile();

        DataEditor.UpdateEditor();
    }

    public void UpdateTile()
    {
        if (bindToTile == null) return;
        
        if (bindToTile.Toggle.isOn)
        {
            TerrainId       = RegionManager.GetTerrainId(regionData, regionData.TerrainDataList.Cast<TerrainBaseData>().ToList(), regionData.TileSize, PositionX, PositionZ);
            TerrainTileId   = RegionManager.GetTerrainTileId(regionData, PositionX, PositionZ);

        } else {

            TerrainId       = 0;
            TerrainTileId   = 0;
        }

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
