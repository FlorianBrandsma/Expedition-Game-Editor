using UnityEngine;
using System.Collections.Generic;

public class GameInteractionData
{
    public GameWorldInteractableElementData GameWorldInteractableData { get; set; }

    public int Id                           { get; set; }

    public int TaskId                       { get; set; }

    public bool Default                     { get; set; }

    public bool ContainsActiveTime          { get; set; }

    public int StartTime                    { get; set; }
    public int EndTime                      { get; set; }

    public Enums.ArrivalType ArrivalType    { get; set; }

    public bool TriggerAutomatically        { get; set; }
    public bool BeNearDestination           { get; set; }
    public bool FaceInteractable            { get; set; }
    public bool FacePartyLeader             { get; set; }
    public bool HideInteractionIndicator    { get; set; }

    public float InteractionRange           { get; set; }

    public Enums.DelayMethod DelayMethod    { get; set; }
    public float DelayDuration              { get; set; }
    public bool HideDelayIndicator          { get; set; }

    public bool CancelDelayOnInput          { get; set; }
    public bool CancelDelayOnMovement       { get; set; }
    public bool CancelDelayOnHit            { get; set; }

    public int ObjectiveId                  { get; set; }
    public int WorldInteractableId          { get; set; }
    
    public int DestinationIndex             { get; set; } = -1;

    public List<GameInteractionDestinationElementData> InteractionDestinationDataList { get; set; } = new List<GameInteractionDestinationElementData>();

    public virtual void GetOriginalValues(GameInteractionData originalData)
    {
        GameWorldInteractableData   = originalData.GameWorldInteractableData;

        Id                          = originalData.Id;

        TaskId                      = originalData.TaskId;

        Default                     = originalData.Default;

        ContainsActiveTime          = originalData.ContainsActiveTime;

        StartTime                   = originalData.StartTime;
        EndTime                     = originalData.EndTime;

        TriggerAutomatically        = originalData.TriggerAutomatically;
        BeNearDestination           = originalData.BeNearDestination;
        FaceInteractable            = originalData.FaceInteractable;
        FacePartyLeader             = originalData.FacePartyLeader;
        HideInteractionIndicator    = originalData.HideInteractionIndicator;

        InteractionRange            = originalData.InteractionRange;

        DelayMethod                 = originalData.DelayMethod;
        DelayDuration               = originalData.DelayDuration;
        HideDelayIndicator          = originalData.HideDelayIndicator;

        CancelDelayOnInput          = originalData.CancelDelayOnInput;
        CancelDelayOnMovement       = originalData.CancelDelayOnMovement;
        CancelDelayOnHit            = originalData.CancelDelayOnHit;

        ObjectiveId                 = originalData.ObjectiveId;
        WorldInteractableId         = originalData.WorldInteractableId;

        DestinationIndex            = originalData.DestinationIndex;
    }

    public GameInteractionData Clone()
    {
        var data = new GameInteractionData();
        
        data.GameWorldInteractableData  = GameWorldInteractableData;

        data.Id                         = Id;

        data.TaskId                     = TaskId;

        data.Default                    = Default;

        data.ContainsActiveTime         = ContainsActiveTime;

        data.StartTime                  = StartTime;
        data.EndTime                    = EndTime;

        data.TriggerAutomatically       = TriggerAutomatically;
        data.BeNearDestination          = BeNearDestination;
        data.FaceInteractable           = FaceInteractable;
        data.FacePartyLeader            = FacePartyLeader;
        data.HideInteractionIndicator   = HideInteractionIndicator;

        data.InteractionRange           = InteractionRange;

        data.DelayMethod                = DelayMethod;
        data.DelayDuration              = DelayDuration;
        data.HideDelayIndicator         = HideDelayIndicator;

        data.CancelDelayOnInput         = CancelDelayOnInput;
        data.CancelDelayOnMovement      = CancelDelayOnMovement;
        data.CancelDelayOnHit           = CancelDelayOnHit;

        data.ObjectiveId                = ObjectiveId;
        data.WorldInteractableId        = WorldInteractableId;

        data.DestinationIndex           = DestinationIndex;

        InteractionDestinationDataList.ForEach(x => x.SetOriginalValues());

        return data;
    }

    public virtual void Clone(GameInteractionElementData elementData)
    {
        elementData.GameWorldInteractableData   = GameWorldInteractableData;

        elementData.Id                          = Id;

        elementData.TaskId                      = TaskId;

        elementData.Default                     = Default;

        elementData.ContainsActiveTime          = ContainsActiveTime;

        elementData.StartTime                   = StartTime;
        elementData.EndTime                     = EndTime;

        elementData.TriggerAutomatically        = TriggerAutomatically;
        elementData.BeNearDestination           = BeNearDestination;
        elementData.FaceInteractable            = FaceInteractable;
        elementData.FacePartyLeader             = FacePartyLeader;
        elementData.HideInteractionIndicator    = HideInteractionIndicator;

        elementData.InteractionRange            = InteractionRange;

        elementData.DelayMethod                 = DelayMethod;
        elementData.DelayDuration               = DelayDuration;
        elementData.HideDelayIndicator          = HideDelayIndicator;

        elementData.CancelDelayOnInput          = CancelDelayOnInput;
        elementData.CancelDelayOnMovement       = CancelDelayOnMovement;
        elementData.CancelDelayOnHit            = CancelDelayOnHit;

        elementData.ObjectiveId                 = ObjectiveId;
        elementData.WorldInteractableId         = WorldInteractableId;

        elementData.DestinationIndex            = DestinationIndex;
    }
}
