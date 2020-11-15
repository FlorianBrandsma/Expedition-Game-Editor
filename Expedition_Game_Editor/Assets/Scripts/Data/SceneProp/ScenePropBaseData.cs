using UnityEngine;

public class ScenePropBaseData
{
    public int Id               { get; set; }

    public int SceneId          { get; set; }
    public int ModelId          { get; set; }
    public int TerrainId        { get; set; }
    public int TerrainTileId    { get; set; }
    
    public float PositionX      { get; set; }
    public float PositionY      { get; set; }
    public float PositionZ      { get; set; }

    public int RotationX        { get; set; }
    public int RotationY        { get; set; }
    public int RotationZ        { get; set; }

    public virtual void GetOriginalValues(ScenePropData originalData)
    {
        Id              = originalData.Id;

        SceneId         = originalData.SceneId;
        ModelId         = originalData.ModelId;
        TerrainId       = originalData.TerrainId;
        TerrainTileId   = originalData.TerrainTileId;

        PositionX       = originalData.PositionX;
        PositionY       = originalData.PositionY;
        PositionZ       = originalData.PositionZ;

        RotationX       = originalData.RotationX;
        RotationY       = originalData.RotationY;
        RotationZ       = originalData.RotationZ;
    }

    public virtual void Clone(ScenePropData data)
    {
        data.Id             = Id;

        data.SceneId        = SceneId;
        data.ModelId        = ModelId;
        data.TerrainId      = TerrainId;
        data.TerrainTileId  = TerrainTileId;

        data.PositionX      = PositionX;
        data.PositionY      = PositionY;
        data.PositionZ      = PositionZ;

        data.RotationX      = RotationX;
        data.RotationY      = RotationY;
        data.RotationZ      = RotationZ;
    }
}

