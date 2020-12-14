using UnityEngine;

public class GameInteractionDestinationData
{
    public int Id                   { get; set; }

    public int RegionId             { get; set; }
    public int TerrainTileId        { get; set; }

    public float PositionX          { get; set; }
    public float PositionY          { get; set; }
    public float PositionZ          { get; set; }

    public float PositionVariance   { get; set; }

    public int RotationX            { get; set; }
    public int RotationY            { get; set; }
    public int RotationZ            { get; set; }

    public bool FreeRotation        { get; set; }

    public int Animation            { get; set; }
    public float Patience           { get; set; }

    public virtual void GetOriginalValues(GameInteractionDestinationData originalData)
    {
        Id                  = originalData.Id;

        RegionId            = originalData.RegionId;
        TerrainTileId       = originalData.TerrainTileId;

        PositionX           = originalData.PositionX;
        PositionY           = originalData.PositionY;
        PositionZ           = originalData.PositionZ;

        PositionVariance    = originalData.PositionVariance;

        RotationX           = originalData.RotationX;
        RotationY           = originalData.RotationY;
        RotationZ           = originalData.RotationZ;

        FreeRotation        = originalData.FreeRotation;

        Animation           = originalData.Animation;
        Patience            = originalData.Patience;
    }

    public GameInteractionDestinationData Clone()
    {
        var data = new GameInteractionDestinationData();
        
        data.Id                 = Id;

        data.RegionId           = RegionId;
        data.TerrainTileId      = TerrainTileId;

        data.PositionX          = PositionX;
        data.PositionY          = PositionY;
        data.PositionZ          = PositionZ;

        data.PositionVariance   = PositionVariance;

        data.RotationX          = RotationX;
        data.RotationY          = RotationY;
        data.RotationZ          = RotationZ;

        data.FreeRotation       = FreeRotation;

        data.Animation          = Animation;
        data.Patience           = Patience;

        return data;
    }

    public virtual void Clone(GameInteractionDestinationElementData elementData)
    {
        elementData.Id                  = Id;

        elementData.RegionId            = RegionId;
        elementData.TerrainTileId       = TerrainTileId;

        elementData.PositionX           = PositionX;
        elementData.PositionY           = PositionY;
        elementData.PositionZ           = PositionZ;

        elementData.PositionVariance    = PositionVariance;

        elementData.RotationX           = RotationX;
        elementData.RotationY           = RotationY;
        elementData.RotationZ           = RotationZ;

        elementData.FreeRotation        = FreeRotation;

        elementData.Animation           = Animation;
        elementData.Patience            = Patience;
    }
}
