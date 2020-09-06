﻿using UnityEngine;
using System;

public class GameWorldInteractableElementData : GameWorldInteractableData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public GameWorldInteractableData OriginalData   { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.GameWorldInteractable; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    public GameInteractionElementData ActiveInteraction
    {
        get { return Interaction; }
        set
        {
            Interaction = value;

            Interaction.ActiveDestinationIndex = 0;
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
        var data = new GameWorldInteractableElementData();

        data.DataElement = DataElement;

        data.OriginalData = OriginalData.Clone();

        base.Clone(data);

        return data;
    }
}
