using UnityEngine;

public class SceneShotPositionCoordinateSegment : MonoBehaviour, ISegment
{
    public ExToggle changePositionToggle;
    public ExInputNumber xInputField, yInputField, zInputField;
    
    public SegmentController SegmentController  { get { return GetComponent<SegmentController>(); } }
    public IEditor DataEditor                   { get; set; }

    public SceneShotEditor SceneShotEditor      { get { return (SceneShotEditor)DataEditor; } }

    #region Data properties
    private bool ChangePosition
    {
        get { return SceneShotEditor.ChangePosition; }
        set { SceneShotEditor.ChangePosition = value; }
    }

    private float PositionX
    {
        get { return SceneShotEditor.PositionX; }
        set { SceneShotEditor.PositionX = value; }
    }

    private float PositionY
    {
        get { return SceneShotEditor.PositionY; }
        set { SceneShotEditor.PositionY = value; }
    }

    private float PositionZ
    {
        get { return SceneShotEditor.PositionZ; }
        set { SceneShotEditor.PositionZ = value; }
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
        zInputField.max = regionSize.y;
    }

    public void OpenSegment()
    {
        changePositionToggle.Toggle.isOn = ChangePosition;

        xInputField.Value = PositionX;
        yInputField.Value = PositionY;
        zInputField.Value = PositionZ;

        gameObject.SetActive(true);
    }

    public void SetSearchResult(IElementData elementData) { }

    public void UpdatePositionX()
    {
        PositionX = xInputField.Value;
        
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
        
        DataEditor.UpdateEditor();
    }

    public void UpdateChangePosition()
    {
        ChangePosition = changePositionToggle.Toggle.isOn;
        
        DataEditor.UpdateEditor();
    }

    public void CloseSegment() { }
}
