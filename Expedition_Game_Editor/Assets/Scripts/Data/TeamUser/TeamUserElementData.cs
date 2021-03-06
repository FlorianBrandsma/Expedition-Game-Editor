﻿using UnityEngine;
using System;

public class TeamUserElementData : TeamUserData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public TeamUserData OriginalData                { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.TeamUser; } }
    
    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool ChangedRole
    {
        get { return Role != OriginalData.Role; }
    }
    
    public bool Changed
    {
        get { return ChangedRole; }
    }
    #endregion

    public TeamUserElementData() { }

    public void Add(DataRequest dataRequest)
    {
        TeamUserDataManager.AddData(this, dataRequest);
    }

    public void Update(DataRequest dataRequest)
    {
        TeamUserDataManager.UpdateData(this, dataRequest);
    }
    
    public void Remove(DataRequest dataRequest)
    {
        TeamUserDataManager.RemoveData(this, dataRequest);
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
        var data = new TeamUserElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        data.ExecuteType = ExecuteType;

        base.Clone(data);

        return data;
    }
}
