using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInteractionDestinationElementData : GeneralData, IElementData
{
    public DataElement DataElement { get; set; }

    public GameInteractionDestinationElementData() : base()
    {
        DataType = Enums.DataType.GameInteractionDestination;
    }

    public int regionId;
    public int terrainId;
    public int terrainTileId;

    public float positionX;
    public float positionY;
    public float positionZ;

    public float positionVariance;

    public int rotationX;
    public int rotationY;
    public int rotationZ;

    public bool freeRotation;

    public int animation;
    public float patience;
    
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
        var elementData = new GameInteractionDestinationElementData();

        CloneGeneralData(elementData);

        return elementData;
    }
    #endregion
}
