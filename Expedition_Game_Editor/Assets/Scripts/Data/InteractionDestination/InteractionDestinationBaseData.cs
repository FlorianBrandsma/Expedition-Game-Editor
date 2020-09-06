using UnityEngine;

public class InteractionDestinationBaseData
{
    public int Id                   { get; set; }

    public int InteractionId        { get; set; }

    public int RegionId             { get; set; }
    public int TerrainId            { get; set; }
    public int TerrainTileId        { get; set; }

    public float PositionX          { get; set; }
    public float PositionY          { get; set; }
    public float PositionZ          { get; set; }

    public float PositionVariance   { get; set; }

    public bool FreeRotation        { get; set; }

    public int RotationX            { get; set; }
    public int RotationY            { get; set; }
    public int RotationZ            { get; set; }

    public int Animation            { get; set; }
    public float Patience           { get; set; }

    public virtual void GetOriginalValues(InteractionDestinationData originalData)
    {
        Id                  = originalData.Id;

        InteractionId       = originalData.InteractionId;

        RegionId            = originalData.RegionId;
        TerrainId           = originalData.TerrainId;
        TerrainTileId       = originalData.TerrainTileId;

        PositionX           = originalData.PositionX;
        PositionY           = originalData.PositionY;
        PositionZ           = originalData.PositionZ;

        PositionVariance    = originalData.PositionVariance;

        FreeRotation        = originalData.FreeRotation;

        RotationX           = originalData.RotationX;
        RotationY           = originalData.RotationY;
        RotationZ           = originalData.RotationZ;

        Animation           = originalData.Animation;
        Patience            = originalData.Patience;
    }

    public virtual void Clone(InteractionDestinationData data)
    {
        data.Id                 = Id;

        data.InteractionId      = InteractionId;

        data.RegionId           = RegionId;
        data.TerrainId          = TerrainId;
        data.TerrainTileId      = TerrainTileId;

        data.PositionX          = PositionX;
        data.PositionY          = PositionY;
        data.PositionZ          = PositionZ;

        data.PositionVariance   = PositionVariance;

        data.FreeRotation       = FreeRotation;

        data.RotationX          = RotationX;
        data.RotationY          = RotationY;
        data.RotationZ          = RotationZ;

        data.Animation          = Animation;
        data.Patience           = Patience;
    }
}
