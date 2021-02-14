using UnityEngine;
using System;

public class SaveElementData : SaveData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public SaveData OriginalData                    { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.Save; } }

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

    public bool ChangedIndex
    {
        get { return Index != OriginalData.Index; }
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

    public bool ChangedPlayTime
    {
        get { return PlayTime != OriginalData.PlayTime; }
    }

    public bool ChangedSaveTime
    {
        get { return SaveTime.CompareTo(OriginalData.SaveTime) != 0; }
    }

    public bool Changed
    {
        get
        {
            return  ChangedRegionId     || ChangedWorldInteractableId   || 
                    ChangedPositionX    || ChangedPositionY             || ChangedPositionZ || 
                    ChangedGameTime     || ChangedPlayTime              || ChangedSaveTime;
        }
    }
    #endregion

    public SaveElementData() { }

    public void Add(DataRequest dataRequest)
    {
        SaveDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        SaveDataManager.UpdateData(this, dataRequest);
    }

    public void UpdateIndex(DataRequest dataRequest)
    {
        SaveDataManager.UpdateIndex(this, dataRequest);
    }

    public void Remove(DataRequest dataRequest)
    {
        SaveDataManager.RemoveData(this, dataRequest);
    }

    public void RemoveIndex(DataRequest dataRequest)
    {
        SaveDataManager.RemoveIndex(this, dataRequest);
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
        var data = new SaveElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        data.ExecuteType = ExecuteType;

        base.Clone(data);

        return data;
    }
}
