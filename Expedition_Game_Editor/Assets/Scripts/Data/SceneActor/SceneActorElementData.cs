using UnityEngine;
using System;

public class SceneActorElementData : SceneActorData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public SceneActorData OriginalData              { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.SceneActor; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedWorldInteractableId
    {
        get { return WorldInteractableId != OriginalData.WorldInteractableId; }
    }

    public bool ChangedSpeechMethod
    {
        get { return SpeechMethod != OriginalData.SpeechMethod; }
    }

    public bool ChangedSpeechText
    {
        get { return SpeechText != OriginalData.SpeechText; }
    }

    public bool ChangedShowTextBox
    {
        get { return ShowTextBox != OriginalData.ShowTextBox; }
    }

    public bool ChangedTargetSceneActorId
    {
        get { return TargetSceneActorId != OriginalData.TargetSceneActorId; }
    }

    public bool ChangedChangePosition
    {
        get { return ChangePosition != OriginalData.ChangePosition; }
    }

    public bool ChangedFreezePosition
    {
        get { return FreezePosition != OriginalData.FreezePosition; }
    }

    public bool ChangedPositionX
    {
        get { return !Mathf.Approximately(PositionX, OriginalData.PositionX); }
    }

    public bool ChangedPositionY
    {
        get { return !Mathf.Approximately(PositionY, OriginalData.PositionY); }
    }

    public bool ChangedPositionZ
    {
        get { return !Mathf.Approximately(PositionZ, OriginalData.PositionZ); }
    }

    public bool ChangedChangeRotation
    {
        get { return ChangeRotation != OriginalData.ChangeRotation; }
    }

    public bool ChangedFaceTarget
    {
        get { return FaceTarget != OriginalData.FaceTarget; }
    }

    public bool ChangedRotationX
    {
        get { return RotationX != OriginalData.RotationX; }
    }

    public bool ChangedRotationY
    {
        get { return RotationY != OriginalData.RotationY; }
    }

    public bool ChangedRotationZ
    {
        get { return RotationZ != OriginalData.RotationZ; }
    }
    
    public bool Changed
    {
        get
        {
            return  ChangedWorldInteractableId  || ChangedSpeechMethod      || ChangedSpeechText    || ChangedShowTextBox   || ChangedTargetSceneActorId    ||
                    ChangedChangePosition       || ChangedFreezePosition    || ChangedPositionX     || ChangedPositionY     || ChangedPositionZ             ||
                    ChangedChangeRotation       || ChangedFaceTarget        || ChangedRotationX     || ChangedRotationY     || ChangedRotationZ;
        }
    }
    #endregion

    public SceneActorElementData() { }

    public void Add(DataRequest dataRequest)
    {
        SceneActorDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        SceneActorDataManager.UpdateData(this, dataRequest);
    }

    public void Remove(DataRequest dataRequest)
    {
        SceneActorDataManager.RemoveData(this, dataRequest);
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
        var data = new SceneActorElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
