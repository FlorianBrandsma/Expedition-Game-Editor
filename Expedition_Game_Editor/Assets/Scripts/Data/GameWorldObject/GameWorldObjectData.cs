using UnityEngine;

public class GameWorldObjectData
{
    public int Id               { get; set; }

    public int TerrainTileId    { get; set; }
    public int ModelId          { get; set; }

    public string ModelPath     { get; set; }

    public int Animation        { get; set; }

    public float PositionX      { get; set; }
    public float PositionY      { get; set; }
    public float PositionZ      { get; set; }

    public float RotationX      { get; set; }
    public float RotationY      { get; set; }
    public float RotationZ      { get; set; }

    public float Scale          { get; set; }

    public virtual void GetOriginalValues(GameWorldObjectData originalData)
    {
        Id              = originalData.Id;

        TerrainTileId   = originalData.TerrainTileId;
        ModelId         = originalData.ModelId;

        ModelPath       = originalData.ModelPath;

        Animation       = originalData.Animation;

        PositionX       = originalData.PositionX;
        PositionY       = originalData.PositionY;
        PositionZ       = originalData.PositionZ;

        RotationX       = originalData.RotationX;
        RotationY       = originalData.RotationY;
        RotationZ       = originalData.RotationZ;

        Scale           = originalData.Scale;
    }

    public GameWorldObjectData Clone()
    {
        var data = new GameWorldObjectData();
        
        data.Id             = Id;

        data.TerrainTileId  = TerrainTileId;
        data.ModelId        = ModelId;

        data.ModelPath      = ModelPath;

        data.Animation      = Animation;

        data.PositionX      = PositionX;
        data.PositionY      = PositionY;
        data.PositionZ      = PositionZ;

        data.RotationX      = RotationX;
        data.RotationY      = RotationY;
        data.RotationZ      = RotationZ;

        data.Scale          = Scale;

        return data;
    }

    public virtual void Clone(GameWorldObjectElementData elementData)
    {
        elementData.Id              = Id;

        elementData.TerrainTileId   = TerrainTileId;
        elementData.ModelId         = ModelId;

        elementData.ModelPath       = ModelPath;

        elementData.Animation       = Animation;

        elementData.PositionX       = PositionX;
        elementData.PositionY       = PositionY;
        elementData.PositionZ       = PositionZ;

        elementData.RotationX       = RotationX;
        elementData.RotationY       = RotationY;
        elementData.RotationZ       = RotationZ;

        elementData.Scale = Scale;
    }
}
