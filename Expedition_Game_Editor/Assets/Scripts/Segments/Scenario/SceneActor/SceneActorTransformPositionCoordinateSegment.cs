﻿using UnityEngine;
using System.Linq;

public class SceneActorTransformPositionCoordinateSegment : MonoBehaviour, ISegment
{
    private RegionElementData regionData;

    public ExInputNumber xInputField, yInputField, zInputField;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public SceneActorEditor SceneActorEditor    { get { return (SceneActorEditor)DataEditor; } }

    #region Data properties
    private bool ChangePosition
    {
        get { return SceneActorEditor.ChangePosition; }
    }

    private bool FreezePosition
    {
        get { return SceneActorEditor.FreezePosition; }
    }

    private float PositionX
    {
        get { return SceneActorEditor.PositionX; }
        set { SceneActorEditor.PositionX = value; }
    }

    private float PositionY
    {
        get { return SceneActorEditor.PositionY; }
        set { SceneActorEditor.PositionY = value; }
    }

    private float PositionZ
    {
        get { return SceneActorEditor.PositionZ; }
        set { SceneActorEditor.PositionZ = value; }
    }

    private int TerrainId
    {
        get { return SceneActorEditor.TerrainId; }
        set { SceneActorEditor.TerrainId = value; }
    }

    private int TerrainTileId
    {
        get { return SceneActorEditor.TerrainTileId; }
        set { SceneActorEditor.TerrainTileId = value; }
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

        UpdateTile();

        UpdateSegment();

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

    private void UpdateTile()
    {
        TerrainId = RegionManager.GetTerrainId(regionData, regionData.TerrainDataList.Cast<TerrainBaseData>().ToList(), regionData.TileSize, PositionX, PositionZ);
        TerrainTileId = RegionManager.GetTerrainTileId(regionData, PositionX, PositionZ);
    }

    public void UpdateSegment()
    {
        EnableInputFields(ChangePosition);
    }

    private void EnableInputFields(bool enable)
    {
        xInputField.EnableElement(enable);
        yInputField.EnableElement(enable);
        zInputField.EnableElement(enable);
    }

    public void CloseSegment() { }
}
