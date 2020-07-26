using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInteractionElementData : GeneralData, IElementData
{
    public DataElement DataElement { get; set; }

    public GameInteractionElementData() : base()
    {
        DataType = Enums.DataType.GameInteraction;
    }

    public int taskId;
    public int regionId;
    public int terrainTileId;

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

    public float positionX;
    public float positionY;
    public float positionZ;

    public float rotationX;
    public float rotationY;
    public float rotationZ;

    public float scaleMultiplier;

    public int animation;

    public int objectiveId;
    public int worldInteractableId;

    public float height;
    public float width;
    public float depth;

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
