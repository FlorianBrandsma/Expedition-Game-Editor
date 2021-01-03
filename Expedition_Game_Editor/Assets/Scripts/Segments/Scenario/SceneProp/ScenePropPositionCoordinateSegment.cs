using UnityEngine;
using System.Linq;

public class ScenePropPositionCoordinateSegment : MonoBehaviour, ISegment
{
    private RegionElementData regionData;

    public ExInputNumber xInputField, yInputField, zInputField;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public ScenePropEditor ScenePropEditor      { get { return (ScenePropEditor)DataEditor; } }

    #region Data properties
    private float PositionX
    {
        get { return ScenePropEditor.PositionX; }
        set { ScenePropEditor.PositionX = value; }
    }

    private float PositionY
    {
        get { return ScenePropEditor.PositionY; }
        set { ScenePropEditor.PositionY = value; }
    }

    private float PositionZ
    {
        get { return ScenePropEditor.PositionZ; }
        set { ScenePropEditor.PositionZ = value; }
    }

    private int TerrainTileId
    {
        get { return ScenePropEditor.TerrainTileId; }
        set { ScenePropEditor.TerrainTileId = value; }
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
        TerrainTileId = RegionManager.GetTerrainTileId(regionData, PositionX, PositionZ);

        DataEditor.UpdateEditor();
    }

    public void UpdateSegment() { }

    public void CloseSegment() { }
}
