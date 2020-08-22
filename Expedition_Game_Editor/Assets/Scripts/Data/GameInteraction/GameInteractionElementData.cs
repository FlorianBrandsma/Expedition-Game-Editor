using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GameInteractionElementData : GeneralData, IElementData
{
    public DataElement DataElement { get; set; }

    public GameInteractionElementData() : base()
    {
        DataType = Enums.DataType.GameInteraction;
    }

    public GameWorldInteractableElementData gameWorldInteractableData;

    public int taskId;

    public bool isDefault;

    public bool containsActiveTime;

    public int startTime;
    public int endTime;

    public bool triggerAutomatically;
    public bool beNearDestination;
    public bool faceAgent;
    public bool facePartyLeader;
    public bool hideInteractionIndicator;

    public float interactionRange;

    public int delayMethod;
    public int delayDuration;
    public bool hideDelayIndicator;

    public bool cancelDelayOnInput;
    public bool cancelDelayOnMovement;
    public bool cancelDelayOnHit;

    public int objectiveId;
    public int worldInteractableId;

    public float currentPatience;

    public bool arrived;

    private int activeDestinationIndex = -1;
    
    public List<GameInteractionDestinationElementData> interactionDestinationDataList;

    public int ActiveDestinationIndex
    {
        get { return activeDestinationIndex; }
        set
        {
            arrived = false;

            if (value >= interactionDestinationDataList.Count)
                value = 0;

            currentPatience = interactionDestinationDataList[value].patience;
            
            activeDestinationIndex = value;
        }
    }

    public GameInteractionDestinationElementData ActiveInteractionDestination
    {
        get
        {
            return interactionDestinationDataList[ActiveDestinationIndex];
        }
    }

    #region ElementData
    public bool Changed { get { return false; } }
    public void Create() { }
    public void Update() { }
    public void UpdateSearch() { }
    public void UpdateIndex() { }
    public virtual void SetOriginalValues() { }
    public void GetOriginalValues() { }
    public virtual void ClearChanges() { }
    public void Delete() { }
    public IElementData Clone()
    {
        var elementData = new GameInteractionElementData();

        CloneGeneralData(elementData);

        return elementData;
    }
    #endregion
}
