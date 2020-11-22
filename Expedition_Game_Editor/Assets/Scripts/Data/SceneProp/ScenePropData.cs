using UnityEngine;

public class ScenePropData : ScenePropBaseData
{
    public int InteractionId        { get; set; }
    public int TaskId               { get; set; }

    public string ModelPath         { get; set; }
    public string ModelIconPath     { get; set; }

    public string ModelName         { get; set; }

    public float Height             { get; set; }
    public float Width              { get; set; }
    public float Depth              { get; set; }

    public bool Default             { get; set; }

    public int StartTime            { get; set; }
    public int EndTime              { get; set; }

    public bool ContainsActiveTime  { get; set; }

    public override void GetOriginalValues(ScenePropData originalData)
    {
        InteractionId       = originalData.InteractionId;
        TaskId              = originalData.TaskId;

        ModelPath           = originalData.ModelPath;
        ModelIconPath       = originalData.ModelIconPath;

        ModelName           = originalData.ModelName;

        Height              = originalData.Height;
        Width               = originalData.Width;
        Depth               = originalData.Depth;

        Scale               = originalData.Scale;

        Default             = originalData.Default;

        StartTime           = originalData.StartTime;
        EndTime             = originalData.StartTime;

        ContainsActiveTime  = originalData.ContainsActiveTime;

        base.GetOriginalValues(originalData);
    }

    public ScenePropData Clone()
    {
        var data = new ScenePropData();

        data.InteractionId      = InteractionId;
        data.TaskId             = TaskId;

        data.ModelPath          = ModelPath;
        data.ModelIconPath      = ModelIconPath;

        data.ModelName          = ModelName;

        data.Height             = Height;
        data.Width              = Width;
        data.Depth              = Depth;

        data.Scale              = Scale;

        data.Default            = Default;

        data.StartTime          = StartTime;
        data.EndTime            = EndTime;

        data.ContainsActiveTime = ContainsActiveTime;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(ScenePropElementData elementData)
    {
        elementData.InteractionId       = InteractionId;
        elementData.TaskId              = TaskId;

        elementData.ModelPath           = ModelPath;
        elementData.ModelIconPath       = ModelIconPath;

        elementData.ModelName           = ModelName;

        elementData.Height              = Height;
        elementData.Width               = Width;
        elementData.Depth               = Depth;

        elementData.Scale               = Scale;

        elementData.Default             = Default;

        elementData.StartTime           = StartTime;
        elementData.EndTime             = EndTime;

        elementData.ContainsActiveTime  = ContainsActiveTime;

        base.Clone(elementData);
    }
}