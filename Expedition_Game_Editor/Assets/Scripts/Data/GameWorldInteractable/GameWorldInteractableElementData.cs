using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

    //public float currentPatience;

    public List<GameInteractionElementData> interactionDataList;
    
    private GameInteractionElementData activeInteraction;
    public GameInteractionElementData ActiveInteraction
    {
        get { return activeInteraction; }
        set
        {
            activeInteraction = value;

            activeInteraction.ActiveDestinationIndex = 0;
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
        var elementData = new GameWorldInteractableElementData();

        CloneGeneralData(elementData);

        return elementData;
    }
    #endregion
}
