using UnityEngine;

public class SceneData : SceneBaseData
{
    public int PhaseId          { get; set; }

    public string RegionName    { get; set; }

    public int RegionSize       { get; set; }
    public int TerrainSize      { get; set; }
    public float TileSize       { get; set; }

    public string TileIconPath  { get; set; }
    
    public override void GetOriginalValues(SceneData originalData)
    {
        PhaseId = originalData.PhaseId;

        RegionName = originalData.RegionName;

        RegionSize = originalData.RegionSize;
        TerrainSize = originalData.TerrainSize;
        TileSize = originalData.TileSize;

        TileIconPath = originalData.TileIconPath;
        
        base.GetOriginalValues(originalData);
    }

    public SceneData Clone()
    {
        var data = new SceneData();

        data.PhaseId = PhaseId;

        data.RegionName = RegionName;

        data.RegionSize = RegionSize;
        data.TerrainSize = TerrainSize;
        data.TileSize = TileSize;

        data.TileIconPath = TileIconPath;
        
        base.Clone(data);

        return data;
    }

    public virtual void Clone(SceneElementData elementData)
    {
        elementData.PhaseId = PhaseId;

        elementData.RegionName = RegionName;

        elementData.RegionSize = RegionSize;
        elementData.TerrainSize = TerrainSize;
        elementData.TileSize = TileSize;

        elementData.TileIconPath = TileIconPath;
        
        base.Clone(elementData);
    }
}
