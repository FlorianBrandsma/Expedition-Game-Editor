using UnityEngine;
using System;


public class GameInteractionElementData : GameInteractionData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public GameInteractionData OriginalData         { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.GameInteraction; } }

    public Enums.ExecuteType ExecuteType            { get; set; }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    public float CurrentPatience                    { get; set; }

    private int destinationIndex = -1;

    public int DestinationIndex
    {
        get { return destinationIndex; }
        set
        {
            destinationIndex = value;
            
            //Add a small bit of patience to every destination so the agent will move to its
            //intended destination after being halted for a scene
            CurrentPatience = ActiveDestination.Patience + 0.1f;
        }
    }

    public GameInteractionDestinationElementData ActiveDestination
    {
        get { return InteractionDestinationDataList[DestinationIndex]; }
    }
    
    #region Changed
    public bool Changed { get { return false; } }
    #endregion

    public void Add(DataRequest dataRequest) { }

    public void Update(DataRequest dataRequest) { }

    public void Remove(DataRequest dataRequest) { }

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
