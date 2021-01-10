using UnityEngine;
using System;

public class SceneShotElementData : SceneShotData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public SceneShotData OriginalData               { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.SceneShot; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedChangePosition
    {
        get { return ChangePosition != OriginalData.ChangePosition; }
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

    public bool ChangedPositionTargetSceneActorId
    {
        get { return PositionTargetSceneActorId != OriginalData.PositionTargetSceneActorId; }
    }

    public bool ChangedChangeRotation
    {
        get { return ChangeRotation != OriginalData.ChangeRotation; }
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
    
    public bool ChangedRotationTargetSceneActorId
    {
        get { return RotationTargetSceneActorId != OriginalData.RotationTargetSceneActorId; }
    }

    public bool ChangedCameraFilterId
    {
        get { return CameraFilterId != OriginalData.CameraFilterId; }
    }

    public bool Changed
    {
        get
        {
            return  ChangedChangePosition || ChangedPositionX || ChangedPositionY || ChangedPositionZ || ChangedPositionTargetSceneActorId ||
                    ChangedChangeRotation || ChangedRotationX || ChangedRotationY || ChangedRotationZ || ChangedRotationTargetSceneActorId ||
                    ChangedCameraFilterId;
        }
    }
    #endregion

    public SceneShotElementData() { }

    public void Add(DataRequest dataRequest)
    {
        SceneShotDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        SceneShotDataManager.UpdateData(this, dataRequest);
    }

    public void Remove(DataRequest dataRequest)
    {
        SceneShotDataManager.RemoveData(this, dataRequest);
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
        var data = new SceneShotElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
