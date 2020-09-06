using UnityEngine;
using System;

public class PlayerSaveElementData : PlayerSaveData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public PlayerSaveData OriginalData              { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.PlayerSave; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedRegionId
    {
        get { return RegionId != OriginalData.RegionId; }
    }

    public bool ChangedPartyMemberId
    {
        get { return PartyMemberId != OriginalData.PartyMemberId; }
    }

    //public bool ChangedPlayedSeconds
    //{
    //    get { return PlayedSeconds != OriginalData.PlayedSeconds; }
    //}

    public bool Changed
    {
        get
        {
            return ChangedRegionId || ChangedPartyMemberId;
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
        var data = new PlayerSaveElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
