using UnityEngine;

public class WorldObjectBaseData
{
    public int Id               { get; set; }

    public int ModelId          { get; set; }
    public int RegionId         { get; set; }
    public int TerrainId        { get; set; }
    public int TerrainTileId    { get; set; }

    public float PositionX      { get; set; }
    public float PositionY      { get; set; }
    public float PositionZ      { get; set; }

    public int RotationX        { get; set; }
    public int RotationY        { get; set; }
    public int RotationZ        { get; set; }

    public float Scale          { get; set; }

    public int Animation        { get; set; }

    public virtual void GetOriginalValues(WorldObjectData originalData)
    {
        Id              = originalData.Id;

        ModelId         = originalData.ModelId;
        RegionId        = originalData.RegionId;
        TerrainId       = originalData.TerrainId;
        TerrainTileId   = originalData.TerrainTileId;

        PositionX       = originalData.PositionX;
        PositionY       = originalData.PositionY;
        PositionZ       = originalData.PositionZ;

        RotationX       = originalData.RotationX;
        RotationY       = originalData.RotationY;
        RotationZ       = originalData.RotationZ;

        Scale           = originalData.Scale;

        Animation       = originalData.Animation;
    }

    public virtual void Clone(WorldObjectData data)
    {
        data.Id             = Id;

        data.ModelId        = ModelId;
        data.RegionId       = RegionId;
        data.TerrainId      = TerrainId;
        data.TerrainTileId  = TerrainTileId;

        data.PositionX      = PositionX;
        data.PositionY      = PositionY;
        data.PositionZ      = PositionZ;

        data.RotationX      = RotationX;
        data.RotationY      = RotationY;
        data.RotationZ      = RotationZ;

        data.Scale          = Scale;

        data.Animation      = Animation;
    }
}
