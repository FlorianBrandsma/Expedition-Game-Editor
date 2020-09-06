using UnityEngine;

public class ChapterRegionData : ChapterRegionBaseData
{
    public string Name          { get; set; }

    public string TileIconPath  { get; set; }

    public override void GetOriginalValues(ChapterRegionData originalData)
    {
        Name            = originalData.Name;

        TileIconPath    = originalData.TileIconPath;

        base.GetOriginalValues(originalData);
    }

    public ChapterRegionData Clone()
    {
        var data = new ChapterRegionData();
        
        data.Name           = Name;

        data.TileIconPath   = TileIconPath;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(ChapterRegionElementData elementData)
    {
        elementData.Name            = Name;

        elementData.TileIconPath    = TileIconPath;

        base.Clone(elementData);
    }
}
