using UnityEngine;
using System;

public class PlayerSaveElementData : PlayerSaveData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public PlayerSaveData OriginalData              { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.PlayerSave; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedRegionId
    {
        get { return RegionId != OriginalData.RegionId; }
    }

    public bool ChangedWorldInteractableId
    {
        get { return WorldInteractableId != OriginalData.WorldInteractableId; }
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

    public bool ChangedGameTime
    {
        get { return GameTime != OriginalData.GameTime; }
    }

    //public bool ChangedPlayedSeconds
    //{
    //    get { return PlayedSeconds != OriginalData.PlayedSeconds; }
    //}

    public bool Changed
    {
        get
        {
            return ChangedRegionId || ChangedWorldInteractableId || ChangedPositionX || ChangedPositionY || ChangedPositionZ || ChangedGameTime;
        }
    }
    #endregion

    public PlayerSaveElementData() { }

    public void Add(DataRequest dataRequest) { }

    public void Update(DataRequest dataRequest)
    {
        PlayerSaveDataManager.UpdateData(this, dataRequest);
    }

    public void Remove(DataRequest dataRequest)
    {
        PlayerSaveDataManager.RemoveData(this, dataRequest);
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
        var data = new PlayerSaveElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
