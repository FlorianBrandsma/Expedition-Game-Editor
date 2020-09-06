using UnityEngine;

public class TileBaseData
{
    public int Id           { get; set; }

    public int TileSetId    { get; set; }
    public string IconPath  { get; set; }

    public virtual void GetOriginalValues(TileData originalData)
    {
        Id          = originalData.Id;

        TileSetId   = originalData.TileSetId;
        IconPath    = originalData.IconPath;
    }

    public virtual void Clone(TileData data)
    {
        data.Id         = Id;

        data.TileSetId  = TileSetId;
        data.IconPath   = IconPath;
    }
}
