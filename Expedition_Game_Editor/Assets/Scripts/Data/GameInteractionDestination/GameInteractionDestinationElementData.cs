﻿using UnityEngine;
using System;

public class GameInteractionDestinationElementData : GameInteractionDestinationData, IElementData
{
    public DataElement DataElement                      { get; set; }

    public GameInteractionDestinationData OriginalData  { get; set; }

    public Enums.DataType DataType                      { get { return Enums.DataType.GameInteractionDestination; } }

    public Enums.SelectionStatus SelectionStatus        { get; set; }
    public bool UniqueSelection                         { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    #region Changed
    public bool Changed { get { return false; } }
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
        var data = new GameInteractionDestinationElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
