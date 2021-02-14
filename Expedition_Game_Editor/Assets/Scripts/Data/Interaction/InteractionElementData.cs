using UnityEngine;
using System;

public class InteractionElementData : InteractionData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public InteractionData OriginalData             { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Interaction; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

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

    public bool ChangedArrivalType
    {
        get { return ArrivalType != OriginalData.ArrivalType; }
    }

    public bool ChangedTriggerAutomatically
    {
        get { return TriggerAutomatically != OriginalData.TriggerAutomatically; }
    }

    public bool ChangedBeNearDestination
    {
        get { return BeNearDestination != OriginalData.BeNearDestination; }
    }

    public bool ChangedFaceInteractable
    {
        get { return FaceInteractable != OriginalData.FaceInteractable; }
    }

    public bool ChangedFaceControllable
    {
        get { return FaceControllable != OriginalData.FaceControllable; }
    }

    public bool ChangedHideInteractionIndicator
    {
        get { return HideInteractionIndicator != OriginalData.HideInteractionIndicator; }
    }

    public bool ChangedInteractionRange
    {
        get { return !Mathf.Approximately(InteractionRange, OriginalData.InteractionRange); }
    }

    public bool ChangedDelayMethod
    {
        get { return DelayMethod != OriginalData.DelayMethod; }
    }

    public bool ChangedDelayDuration
    {
        get { return !Mathf.Approximately(DelayDuration, OriginalData.DelayDuration); }
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

    public bool ChangedEditorNotes
    {
        get { return EditorNotes != OriginalData.EditorNotes; }
    }

    public bool ChangedGameNotes
    {
        get { return GameNotes != OriginalData.GameNotes; }
    }

    public bool Changed
    {
        get
        {
            return  ChangedStartTime            || ChangedEndTime                   || ChangedArrivalType           ||
                    ChangedTriggerAutomatically || ChangedBeNearDestination         || ChangedFaceInteractable      ||
                    ChangedFaceControllable     || ChangedHideInteractionIndicator  || ChangedInteractionRange      ||
                    ChangedDelayMethod          || ChangedDelayDuration             ||
                    ChangedHideDelayIndicator   || ChangedCancelDelayOnInput        || ChangedCancelDelayOnMovement ||
                    ChangedCancelDelayOnHit     ||
                    ChangedEditorNotes          || ChangedGameNotes;
        }
    }
    #endregion

    public InteractionElementData() { }

    public void Add(DataRequest dataRequest)
    {
        InteractionDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        InteractionDataManager.UpdateData(this, dataRequest);
    }

    public void Remove(DataRequest dataRequest)
    {
        InteractionDataManager.RemoveData(this, dataRequest);
    }

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

        data.ExecuteType = ExecuteType;

        base.Clone(data);

        return data;
    }
}
