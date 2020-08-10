using UnityEngine;
using System.Collections.Generic;

public class GameWorldInteractableElementData : GeneralData, IElementData
{
    public DataElement DataElement { get; set; }

    public GameWorldInteractableElementData() : base()
    {
        DataType = Enums.DataType.GameWorldInteractable;
    }

    public int type;

    public int terrainTileId;
    public int objectGraphicId;

    public string objectGraphicPath;
    public string objectGraphicIconPath;
    
    public string interactableName;
    
    public int health;
    public int hunger;
    public int thirst;

    public float weight;
    public float speed;
    public float stamina;

    public float scaleMultiplier;

    public List<GameInteractionElementData> interactionDataList;

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
        var elementData = new GameWorldInteractableElementData();

        CloneGeneralData(elementData);

        return elementData;
    }
    #endregion
}
