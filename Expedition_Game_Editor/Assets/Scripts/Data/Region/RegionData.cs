using UnityEngine;

public class RegionData : RegionBaseData
{
    public Enums.RegionType Type    { get; set; }

    public float TileSize           { get; set; }
    public string TileIconPath      { get; set; }

    public string TileSetName       { get; set; }

    public Vector2 StartPosition    { get; set; }
    
    public override void GetOriginalValues(RegionData originalData)
    {
        Type            = originalData.Type;

        TileSize        = originalData.TileSize;
        TileIconPath    = originalData.TileIconPath;

        TileSetName     = originalData.TileSetName;

        StartPosition   = originalData.StartPosition;

        base.GetOriginalValues(originalData);
    }

    public RegionData Clone()
    {
        var data = new RegionData();
        
        data.Type =             Type;

        data.TileSize =         TileSize;
        data.TileIconPath =     TileIconPath;

        data.TileSetName =      TileSetName;

        data.StartPosition =    StartPosition;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(RegionElementData elementData)
    {
        elementData.Type            = Type;

        elementData.TileSize        = TileSize;
        elementData.TileIconPath    = TileIconPath;

        elementData.TileSetName     = TileSetName;

        elementData.StartPosition   = StartPosition;

        base.Clone(elementData);
    }
}
