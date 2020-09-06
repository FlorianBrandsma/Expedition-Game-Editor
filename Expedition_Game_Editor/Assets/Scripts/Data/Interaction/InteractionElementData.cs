using UnityEngine;
using System;

public class InteractionElementData : InteractionData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public InteractionData OriginalData             { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Interaction; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }
    
    #region Changed
    public bool ChangedStartTime
    {
        get { return StartTime != OriginalData.StartTime; }
    }

    public bool ChangedEndTime
    {
        get { return EndTime != OriginalData.EndTime; }
    }

    public bool ChangedTriggerAutomatically
    {
        get { return TriggerAutomatically != OriginalData.TriggerAutomatically; }
    }

    public bool ChangedBeNearDestination
    {
        get { return BeNearDestination != OriginalData.BeNearDestination; }
    }

    public bool ChangedFaceAgent
    {
        get { return FaceAgent != OriginalData.FaceAgent; }
    }

    public bool ChangedFacePartyLeader
    {
        get { return FacePartyLeader != OriginalData.FacePartyLeader; }
    }

    public bool ChangedHideInteractionIndicator
    {
        get { return HideInteractionIndicator != OriginalData.HideInteractionIndicator; }
    }

    public bool ChangedInteractionRange
    {
        get { return InteractionRange != OriginalData.InteractionRange; }
    }

    public bool ChangedDelayMethod
    {
        get { return DelayMethod != OriginalData.DelayMethod; }
    }

    public bool ChangedDelayDuration
    {
        get { return DelayDuration != OriginalData.DelayDuration; }
    }

    public bool ChangedHideDelayIndicator
    {
        get { return HideDelayIndicator != OriginalData.HideDelayIndicator; }
    }

    public bool ChangedCancelDelayOnInput
    {
        get { return CancelDelayOnInput != OriginalData.CancelDelayOnInput; }
    }

    public bool ChangedCancelDelayOnMovement
    {
        get { return CancelDelayOnMovement != OriginalData.CancelDelayOnMovement; }
    }

    public bool ChangedCancelDelayOnHit
    {
        get { return CancelDelayOnHit != OriginalData.CancelDelayOnHit; }
    }

    public bool ChangedPublicNotes
    {
        get { return PublicNotes != OriginalData.PublicNotes; }
    }

    public bool ChangedPrivateNotes
    {
        get { return PrivateNotes != OriginalData.PrivateNotes; }
    }

    public bool Changed
    {
        get
        {
            return ChangedStartTime             || ChangedEndTime                   ||
                    ChangedTriggerAutomatically || ChangedBeNearDestination         || ChangedFaceAgent             ||
                    ChangedFacePartyLeader      || ChangedHideInteractionIndicator  || ChangedInteractionRange      ||
                    ChangedDelayMethod          || ChangedDelayDuration             ||
                    ChangedHideDelayIndicator   || ChangedCancelDelayOnInput        || ChangedCancelDelayOnMovement ||
                    ChangedCancelDelayOnHit     ||
                    ChangedPublicNotes          || ChangedPrivateNotes;
        }
    }
    #endregion

    public void Update() { }

    public void UpdateSearch() { }

    public void SetOriginalValues()
    {
        OriginalData = base.Clone();

        ClearChanges();
    }

    public void ClearChanges()
    {
        if (!Changed) return;

        GetOriginalValues();
    }

    public void GetOriginalValues()
    {
        base.GetOriginalValues(OriginalData);
    }

    public new IElementData Clone()
    {
        var data = new InteractionElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
