using UnityEngine;

public class InteractionDestinationDestinationCoordinateSegment : MonoBehaviour, ISegment
{
    public ExInputNumber xInputField, yInputField, zInputField;

    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public InteractionDestinationEditor InteractionDestinationEditor { get { return (InteractionDestinationEditor)DataEditor; } }

    #region Data properties
    private float PositionX
    {
        get { return InteractionDestinationEditor.PositionX; }
        set { InteractionDestinationEditor.PositionX = value; }
    }

    private float PositionY
    {
        get { return InteractionDestinationEditor.PositionY; }
        set { InteractionDestinationEditor.PositionY = value; }
    }

    private float PositionZ
    {
        get { return InteractionDestinationEditor.PositionZ; }
        set { InteractionDestinationEditor.PositionZ = value; }
    }

    private int TerrainId
    {
        get { return InteractionDestinationEditor.TerrainId; }
        set { InteractionDestinationEditor.TerrainId = value; }
    }

    private int TerrainTileId
    {
        get { return InteractionDestinationEditor.TerrainTileId; }
        set { InteractionDestinationEditor.TerrainTileId = value; }
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
        var regionId = SegmentController.Path.FindLastRoute(Enums.DataType.Region).ElementData.Id;

        TerrainId = Fixtures.GetTerrain(regionId, PositionX, PositionZ);
        TerrainTileId = Fixtures.GetTerrainTile(TerrainId, PositionX, PositionZ);

        DataEditor.UpdateEditor();
    }

    public void CloseSegment() { }
}