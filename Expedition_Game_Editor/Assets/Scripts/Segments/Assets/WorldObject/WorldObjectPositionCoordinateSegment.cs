using UnityEngine;
using System.Linq;

public class WorldObjectPositionCoordinateSegment : MonoBehaviour, ISegment
{
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
        var regionData = (RegionElementData)SegmentController.Path.FindLastRoute(Enums.DataType.Region).ElementData;

        var regionSize = new Vector2(regionData.RegionSize * regionData.TerrainSize * regionData.TileSize,
                                     regionData.RegionSize * regionData.TerrainSize * regionData.TileSize);

        xInputField.max = regionSize.x;
        yInputField.max = regionSize.y;
    }

    public void OpenSegment()
    {
        xInputField.Value = PositionX;
        yInputField.Value = PositionY;
        zInputField.Value = PositionZ;
        
        bindToTile.Toggle.isOn = TerrainTileId != 0;
        
        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdatePositionX()
    {
        PositionX = xInputField.Value;

        UpdateTile();

        DataEditor.UpdateEditor();
    }

    public void UpdatePositionY()
    {
        PositionY = yInputField.Value;

        UpdateTile();

        DataEditor.UpdateEditor();
    }

    public void UpdatePositionZ()
    {
        PositionZ = zInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdateTile()
    {
        if (bindToTile == null) return;

        if (bindToTile.Toggle.isOn)
        {
            var regionId = SegmentController.Path.FindLastRoute(Enums.DataType.Region).ElementData.Id;

            var terrainId = Fixtures.GetTerrain(regionId, PositionX, PositionZ);
            TerrainTileId = Fixtures.GetTerrainTile(terrainId, PositionX, PositionZ);

        } else {
            
            TerrainTileId = 0;
        }

        DataEditor.UpdateEditor();
    }

    public void CloseSegment() { }
}
