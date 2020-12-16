using UnityEngine;

public class SceneActorData : SceneActorBaseData
{
    public int InteractionId        { get; set; }
    public int TaskId               { get; set; }
    public int ModelId              { get; set; }

    public string ModelPath         { get; set; }
    public string ModelIconPath     { get; set; }

    public string InteractableName  { get; set; }

    public float Height             { get; set; }
    public float Width              { get; set; }
    public float Depth              { get; set; }

    public float Scale              { get; set; }

    public bool Default             { get; set; }

    public int StartTime            { get; set; }
    public int EndTime              { get; set; }

    public bool ContainsActiveTime  { get; set; }

    public int SpeechTextLimit      { get; set; }

    public override void GetOriginalValues(SceneActorData originalData)
    {
        InteractionId       = originalData.InteractionId;
        TaskId              = originalData.TaskId;
        ModelId             = originalData.ModelId;

        ModelPath           = originalData.ModelPath;
        ModelIconPath       = originalData.ModelIconPath;

        InteractableName    = originalData.InteractableName;

        Height              = originalData.Height;
        Width               = originalData.Width;
        Depth               = originalData.Depth;

        Scale               = originalData.Scale;

        Default             = originalData.Default;

        StartTime           = originalData.StartTime;
        EndTime             = originalData.StartTime;

        ContainsActiveTime  = originalData.ContainsActiveTime;

        SpeechTextLimit     = originalData.SpeechTextLimit;

        base.GetOriginalValues(originalData);
    }

    public SceneActorData Clone()
    {
        var data = new SceneActorData();

        data.InteractionId      = InteractionId;
        data.TaskId             = TaskId;
        data.ModelId            = ModelId;

        data.ModelPath          = ModelPath;
        data.ModelIconPath      = ModelIconPath;

        data.InteractableName   = InteractableName;

        data.Height             = Height;
        data.Width              = Width;
        data.Depth              = Depth;

        data.Scale              = Scale;

        data.Default            = Default;

        data.StartTime          = StartTime;
        data.EndTime            = EndTime;

        data.ContainsActiveTime = ContainsActiveTime;

        data.SpeechTextLimit    = SpeechTextLimit;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(SceneActorElementData elementData)
    {
        elementData.InteractionId       = InteractionId;
        elementData.TaskId              = TaskId;
        elementData.ModelId             = ModelId;

        elementData.ModelPath           = ModelPath;
        elementData.ModelIconPath       = ModelIconPath;

        elementData.InteractableName    = InteractableName;

        elementData.Height              = Height;
        elementData.Width               = Width;
        elementData.Depth               = Depth;

        elementData.Scale               = Scale;

        elementData.Default             = Default;

        elementData.StartTime           = StartTime;
        elementData.EndTime             = EndTime;

        elementData.ContainsActiveTime  = ContainsActiveTime;

        elementData.SpeechTextLimit     = SpeechTextLimit;

        base.Clone(elementData);
    }
}