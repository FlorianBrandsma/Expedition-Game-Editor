using UnityEngine;

public class TileData : TileBaseData
{
    public string Icon { get; set; }

    public override void GetOriginalValues(TileData originalData)
    {
        Icon = originalData.Icon;
        
        base.GetOriginalValues(originalData);
    }

    public TileData Clone()
    {
        var data = new TileData();

        data.Icon = Icon;
        
        base.Clone(data);

        return data;
    }

    public virtual void Clone(TileElementData elementData)
    {
        elementData.Icon = Icon;
        
        base.Clone(elementData);
    }
}
