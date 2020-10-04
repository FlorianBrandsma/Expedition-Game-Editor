using UnityEngine;
using System;
using System.Collections.Generic;

public class GameInteractionElementData : GameInteractionData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public GameInteractionData OriginalData         { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.GameInteraction; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    public float CurrentPatience                    { get; set; }

    public List<GameInteractionDestinationElementData> InteractionDestinationDataList { get; set; } = new List<GameInteractionDestinationElementData>();

    public int ActiveDestinationIndex
    {
        get { return DestinationIndex; }
        set
        {
            if (value >= InteractionDestinationDataList.Count)
                value = 0;

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

        InteractionDestinationDataList.ForEach(x => x.SetOriginalValues());

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
