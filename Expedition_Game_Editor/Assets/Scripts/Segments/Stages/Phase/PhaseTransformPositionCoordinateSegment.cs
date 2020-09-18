using UnityEngine;

public class PhaseTransformPositionCoordinateSegment : MonoBehaviour, ISegment
{
    public ExInputNumber xInputField, yInputField, zInputField;
    
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public PhaseEditor PhaseEditor              { get { return (PhaseEditor)DataEditor; } }

    #region Data properties
    private float DefaultPositionX
    {
        get { return PhaseEditor.DefaultPositionX; }
        set { PhaseEditor.DefaultPositionX = value; }
    }

    private float DefaultPositionY
    {
        get { return PhaseEditor.DefaultPositionY; }
        set { PhaseEditor.DefaultPositionY = value; }
    }

    private float DefaultPositionZ
    {
        get { return PhaseEditor.DefaultPositionZ; }
        set { PhaseEditor.DefaultPositionZ = value; }
    }

    private int DefaultTime
    {
        get { return PhaseEditor.DefaultTime; }
        set { PhaseEditor.DefaultTime = value; }
    }

    private int TerrainTileId
    {
        get { return PhaseEditor.TerrainTileId; }
        set { PhaseEditor.TerrainTileId = value; }
    }
    #endregion

    public void InitializeDependencies()
    {
        DataEditor = SegmentController.EditorController.PathController.DataEditor;

        if (!DataEditor.EditorSegments.Contains(SegmentController))
            DataEditor.EditorSegments.Add(SegmentController);
    }
    
    public void InitializeData()
    {
        if (DataEditor.Loaded) return;

        TimeManager.instance.ActiveTime = DefaultTime;
    }

    public void InitializeSegment()
    {
        var regionData = (RegionElementData)SegmentController.Path.FindLastRoute(Enums.DataType.Region).ElementData;

        var regionSize = new Vector2(regionData.RegionSize * regionData.TerrainSize * regionData.TileSize,
                                     regionData.RegionSize * regionData.TerrainSize * regionData.TileSize);

        xInputField.max = regionSize.x;
        yInputField.max = regionSize.y;

        UpdateTime();
    }
    
    public void OpenSegment()
    {
        xInputField.Value = DefaultPositionX;
        yInputField.Value = DefaultPositionY;
        zInputField.Value = DefaultPositionZ;

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdatePositionX()
    {
        DefaultPositionX = xInputField.Value;

        UpdateTile();

        DataEditor.UpdateEditor();
    }

    public void UpdatePositionY()
    {
        DefaultPositionY = yInputField.Value;

        UpdateTile();

        DataEditor.UpdateEditor();
    }

    public void UpdatePositionZ()
    {
        DefaultPositionZ = zInputField.Value;

        DataEditor.UpdateEditor();
    }

    public void UpdateTile()
    {
        var regionId = SegmentController.Path.FindLastRoute(Enums.DataType.Region).ElementData.Id;

        var terrainId = Fixtures.GetTerrain(regionId, DefaultPositionX, DefaultPositionZ);
        TerrainTileId = Fixtures.GetTerrainTile(terrainId, DefaultPositionX, DefaultPositionZ);

        DataEditor.UpdateEditor();
    }

    public void UpdateTime()
    {
        DefaultTime = TimeManager.instance.ActiveTime;

        DataEditor.UpdateEditor();
    }

    public void CloseSegment() { }   
}