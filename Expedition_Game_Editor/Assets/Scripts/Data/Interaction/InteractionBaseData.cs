using UnityEngine;

public class InteractionBaseData
{
    public int Id                           { get; set; }

    public int TaskId                       { get; set; }

    public bool Default                     { get; set; }

    public int StartTime                    { get; set; }
    public int EndTime                      { get; set; }

    public int ArrivalType                  { get; set; }

    public bool TriggerAutomatically        { get; set; }
    public bool BeNearDestination           { get; set; }
    public bool FaceInteractable            { get; set; }
    public bool FaceControllable            { get; set; }
    public bool HideInteractionIndicator    { get; set; }

    public float InteractionRange           { get; set; }

    public int DelayMethod                  { get; set; }
    public float DelayDuration              { get; set; }
    public bool HideDelayIndicator          { get; set; }

    public bool CancelDelayOnInput          { get; set; }
    public bool CancelDelayOnMovement       { get; set; }
    public bool CancelDelayOnHit            { get; set; }

    public string EditorNotes               { get; set; }
    public string GameNotes              { get; set; }

    public virtual void GetOriginalValues(InteractionData originalData)
    {
        Id                          = originalData.Id;

        TaskId                      = originalData.TaskId;

        Default                     = originalData.Default;

        StartTime                   = originalData.StartTime;
        EndTime                     = originalData.EndTime;

        ArrivalType                 = originalData.ArrivalType;

        TriggerAutomatically        = originalData.TriggerAutomatically;
        BeNearDestination           = originalData.BeNearDestination;
        FaceInteractable            = originalData.FaceInteractable;
        FaceControllable            = originalData.FaceControllable;
        HideInteractionIndicator    = originalData.HideInteractionIndicator;

        InteractionRange            = originalData.InteractionRange;

        DelayMethod                 = originalData.DelayMethod;
        DelayDuration               = originalData.DelayDuration;
        HideDelayIndicator          = originalData.HideDelayIndicator;

        CancelDelayOnInput          = originalData.CancelDelayOnInput;
        CancelDelayOnMovement       = originalData.CancelDelayOnMovement;
        CancelDelayOnHit            = originalData.CancelDelayOnHit;

        EditorNotes                 = originalData.EditorNotes;
        GameNotes                   = originalData.GameNotes;
    }

    public virtual void Clone(InteractionData data)
    {
        data.Id                         = Id;

        data.TaskId                     = TaskId;

        data.Default                    = Default;

        data.StartTime                  = StartTime;
        data.EndTime                    = EndTime;

        data.ArrivalType                = ArrivalType;

        data.TriggerAutomatically       = TriggerAutomatically;
        data.BeNearDestination          = BeNearDestination;
        data.FaceInteractable           = FaceInteractable;
        data.FaceControllable           = FaceControllable;
        data.HideInteractionIndicator   = HideInteractionIndicator;

        data.InteractionRange           = InteractionRange;

        data.DelayMethod                = DelayMethod;
        data.DelayDuration              = DelayDuration;
        data.HideDelayIndicator         = HideDelayIndicator;

        data.CancelDelayOnInput         = CancelDelayOnInput;
        data.CancelDelayOnMovement      = CancelDelayOnMovement;
        data.CancelDelayOnHit           = CancelDelayOnHit;

        data.EditorNotes                = EditorNotes;
        data.GameNotes                  = GameNotes;
    }
}
