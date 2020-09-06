using UnityEngine;

public class WorldInteractableData : WorldInteractableBaseData
{
    public Enums.ElementStatus ElementStatus { get; set; }

    public int TerrainTileId        { get; set; }

    public int ModelId              { get; set; }

    public bool Default             { get; set; }
    public int TaskGroup            { get; set; }

    public string ModelPath         { get; set; }

    public string InteractableName  { get; set; }
    public string ModelIconPath     { get; set; }

    public float PositionX          { get; set; }
    public float PositionY          { get; set; }
    public float PositionZ          { get; set; }

    public int RotationX            { get; set; }
    public int RotationY            { get; set; }
    public int RotationZ            { get; set; }

    public float Height             { get; set; }
    public float Width              { get; set; }
    public float Depth              { get; set; }

    public float Scale              { get; set; }

    public int Animation            { get; set; }

    public int StartTime            { get; set; }
    public int EndTime              { get; set; }

    public bool ContainsActiveTime  { get; set; }

    public override void GetOriginalValues(WorldInteractableData originalData)
    {
        ElementStatus       = originalData.ElementStatus;

        TerrainTileId       = originalData.TerrainTileId;

        ModelId             = originalData.ModelId;

        Default             = originalData.Default;
        TaskGroup           = originalData.TaskGroup;

        ModelPath           = originalData.ModelPath;

        InteractableName    = originalData.InteractableName;
        ModelIconPath       = originalData.ModelIconPath;

        PositionX           = originalData.PositionX;
        PositionY           = originalData.PositionY;
        PositionZ           = originalData.PositionZ;

        RotationX           = originalData.RotationX;
        RotationY           = originalData.RotationY;
        RotationZ           = originalData.RotationZ;

        Height              = originalData.Height;
        Width               = originalData.Width;
        Depth               = originalData.Depth;

        Scale               = originalData.Scale;

        Animation           = originalData.Animation;

        StartTime           = originalData.StartTime;
        EndTime             = originalData.EndTime;

        ContainsActiveTime  = originalData.ContainsActiveTime;

        base.GetOriginalValues(originalData);
    }

    public WorldInteractableData Clone()
    {
        var data = new WorldInteractableData();
        
        data.ElementStatus      = ElementStatus;

        data.TerrainTileId      = TerrainTileId;

        data.ModelId            = ModelId;

        data.Default            = Default;
        data.TaskGroup          = TaskGroup;

        data.ModelPath          = ModelPath;

        data.InteractableName   = InteractableName;
        data.ModelIconPath      = ModelIconPath;

        data.PositionX          = PositionX;
        data.PositionY          = PositionY;
        data.PositionZ          = PositionZ;

        data.RotationX          = RotationX;
        data.RotationY          = RotationY;
        data.RotationZ          = RotationZ;

        data.Height             = Height;
        data.Width              = Width;
        data.Depth              = Depth;

        data.Scale              = Scale;

        data.Animation          = Animation;

        data.StartTime          = StartTime;
        data.EndTime            = EndTime;

        data.ContainsActiveTime = ContainsActiveTime;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(WorldInteractableElementData elementData)
    {
        elementData.ElementStatus       = ElementStatus;

        elementData.TerrainTileId       = TerrainTileId;

        elementData.ModelId             = ModelId;

        elementData.Default             = Default;
        elementData.TaskGroup           = TaskGroup;

        elementData.ModelPath           = ModelPath;

        elementData.InteractableName    = InteractableName;
        elementData.ModelIconPath       = ModelIconPath;

        elementData.PositionX           = PositionX;
        elementData.PositionY           = PositionY;
        elementData.PositionZ           = PositionZ;

        elementData.RotationX           = RotationX;
        elementData.RotationY           = RotationY;
        elementData.RotationZ           = RotationZ;

        elementData.Height              = Height;
        elementData.Width               = Width;
        elementData.Depth               = Depth;

        elementData.Scale               = Scale;

        elementData.Animation           = Animation;

        elementData.StartTime           = StartTime;
        elementData.EndTime             = EndTime;

        elementData.ContainsActiveTime  = ContainsActiveTime;

        base.Clone(elementData);
    }
}
