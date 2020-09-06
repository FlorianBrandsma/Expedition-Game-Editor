using UnityEngine;

public class TileSetBaseData
{
    public int Id           { get; set; }

    public string Name      { get; set; }

    public float TileSize   { get; set; }

    public virtual void GetOriginalValues(TileSetBaseData originalData)
    {
        Id          = originalData.Id;

        Name        = originalData.Name;

        TileSize    = originalData.TileSize;
    }

    public virtual void Clone(TileSetBaseData data)
    {
        data.Id         = Id;

        data.Name       = Name;

        data.TileSize   = TileSize;
    }
}
