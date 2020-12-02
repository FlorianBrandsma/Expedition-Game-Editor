using UnityEngine;

public class GameScenePropData
{
    public int Id               { get; set; }

    public int TerrainTileId    { get; set; }

    public string ModelPath     { get; set; }

    public float PositionX      { get; set; }
    public float PositionY      { get; set; }
    public float PositionZ      { get; set; }

    public int RotationX        { get; set; }
    public int RotationY        { get; set; }
    public int RotationZ        { get; set; }

    public float Scale          { get; set; }

    public virtual void GetOriginalValues(GameScenePropData originalData)
    {
        Id              = originalData.Id;

        TerrainTileId   = originalData.TerrainTileId;

        ModelPath       = originalData.ModelPath;

        PositionX       = originalData.PositionX;
        PositionY       = originalData.PositionY;
        PositionZ       = originalData.PositionZ;

        RotationX       = originalData.RotationX;
        RotationY       = originalData.RotationY;
        RotationZ       = originalData.RotationZ;

        Scale           = originalData.Scale;
    }

    public GameScenePropData Clone()
    {
        var data = new GameScenePropData();

        data.Id             = Id;

        data.TerrainTileId  = TerrainTileId;

        data.ModelPath      = ModelPath;

        data.PositionX      = PositionX;
        data.PositionY      = PositionY;
        data.PositionZ      = PositionZ;

        data.RotationX      = RotationX;
        data.RotationY      = RotationY;
        data.RotationZ      = RotationZ;

        data.Scale          = Scale;

        return data;
    }

    public virtual void Clone(GameScenePropElementData elementData)
    {
        elementData.Id              = Id;
        
        elementData.TerrainTileId   = TerrainTileId;

        elementData.ModelPath       = ModelPath;

        elementData.PositionX       = PositionX;
        elementData.PositionY       = PositionY;
        elementData.PositionZ       = PositionZ;

        elementData.RotationX       = RotationX;
        elementData.RotationY       = RotationY;
        elementData.RotationZ       = RotationZ;

        elementData.Scale           = Scale;
    }
}
