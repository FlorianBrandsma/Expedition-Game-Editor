using UnityEngine;

public class TerrainTileBaseData
{
    public int Id           { get; set; }

    public int TerrainId    { get; set; }
    public int TileId       { get; set; }

    public int Index        { get; set; }

    public virtual void GetOriginalValues(TerrainTileData originalData)
    {
        Id          = originalData.Id;

        TerrainId   = originalData.TerrainId;
        TileId      = originalData.TileId;

        Index       = originalData.Index;
    }

    public virtual void Clone(TerrainTileData data)
    {
        data.Id         = Id;

        data.TerrainId  = TerrainId;
        data.TileId     = TileId;

        data.Index      = Index;
    }
}
