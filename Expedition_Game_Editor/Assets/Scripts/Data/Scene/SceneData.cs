using UnityEngine;

public class SceneData : SceneBaseData
{
    public int InteractionId        { get; set; }

    public string RegionName        { get; set; }

    public string TileIconPath      { get; set; }
    
    public override void GetOriginalValues(SceneData originalData)
    {
        InteractionId   = originalData.InteractionId;

        RegionName      = originalData.RegionName;

        TileIconPath    = originalData.TileIconPath;
        
        base.GetOriginalValues(originalData);
    }

    public SceneData Clone()
    {
        var data = new SceneData();

        data.InteractionId  = InteractionId;

        data.RegionName     = RegionName;

        data.TileIconPath   = TileIconPath;
        
        base.Clone(data);

        return data;
    }

    public virtual void Clone(SceneElementData elementData)
    {
        elementData.InteractionId   = InteractionId;

        elementData.RegionName      = RegionName;

        elementData.TileIconPath    = TileIconPath;
        
        base.Clone(elementData);
    }
}
