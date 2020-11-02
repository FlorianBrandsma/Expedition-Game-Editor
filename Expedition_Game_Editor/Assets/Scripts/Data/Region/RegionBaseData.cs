using UnityEngine;

public class RegionBaseData
{
    public int Id               { get; set; }
    
    public int ChapterRegionId  { get; set; }
    public int PhaseId          { get; set; }
    public int TileSetId        { get; set; }

    public int Index            { get; set; }

    public string Name          { get; set; }

    public int RegionSize       { get; set; }
    public int TerrainSize      { get; set; }

    public virtual void GetOriginalValues(RegionData originalData)
    {
        Id              = originalData.Id;

        ChapterRegionId = originalData.ChapterRegionId;
        PhaseId         = originalData.PhaseId;
        TileSetId       = originalData.TileSetId;

        Index           = originalData.Index;

        Name            = originalData.Name;

        RegionSize      = originalData.RegionSize;
        TerrainSize     = originalData.TerrainSize;
    }

    public virtual void Clone(RegionData data)
    {
        data.Id                 = Id;

        data.ChapterRegionId    = ChapterRegionId;
        data.PhaseId            = PhaseId;
        data.TileSetId          = TileSetId;

        data.Index              = Index;

        data.Name               = Name;

        data.RegionSize         = RegionSize;
        data.TerrainSize        = TerrainSize;
    }
}
