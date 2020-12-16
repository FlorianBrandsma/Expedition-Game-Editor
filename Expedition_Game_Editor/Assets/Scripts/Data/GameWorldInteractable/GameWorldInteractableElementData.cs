using UnityEngine;
using System;

public class GameWorldInteractableElementData : GameWorldInteractableData, IElementData
{
    public DataElement DataElement                  { get; set; }

    public GameWorldInteractableData OriginalData   { get; set; }

    public Enums.DataType DataType                  { get { return Enums.DataType.GameWorldInteractable; } }

    public Enums.SelectionStatus SelectionStatus    { get; set; }
    public bool UniqueSelection                     { get; set; }

    public string DebugName { get { return Enum.GetName(typeof(Enums.DataType), DataType); } }

    public AgentState AgentState            { get; set; }
    public DestinationType DestinationType  { get; set; }
    public Vector3 CurrentPosition          { get; set; }

    public Vector3 DestinationPosition      { get; set; }
    public Vector3 ArrivalRotation          { get; set; }
    public bool AllowRotation               { get; set; }
    public bool ArriveInstantly             { get; set; }

    public float TravelTime                 { get; set; }
    
    public bool Backtracing                 { get; set; }

    public GameInteractionElementData ActiveInteraction
    {
        get { return Interaction; }
        set
        {
            if (Interaction == value) return;

            Interaction = value;
            
            Backtracing = false;
            MovementManager.SetDestination(this, 0);
         
            InteractionManager.CancelInteractionDelay(this);

            if (InteractionManager.interactionTarget == this)
            {
                InteractionManager.CancelInteraction();
            }
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
