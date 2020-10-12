﻿using UnityEngine;
using System;


public class GameInteractionElementData : GameInteractionData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public GameInteractionData OriginalData         { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.GameInteraction; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    public float CurrentPatience                    { get; set; }
    
    public int ActiveDestinationIndex
    {
        get { return DestinationIndex; }
        set
        {
            CurrentPatience = InteractionDestinationDataList[value].Patience;

            DestinationIndex = value;
        }
    }

    public GameInteractionDestinationElementData ActiveDestination
    {
        get
        {
            return InteractionDestinationDataList[ActiveDestinationIndex];
        }
    }
    
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
        var data = new GameInteractionElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
