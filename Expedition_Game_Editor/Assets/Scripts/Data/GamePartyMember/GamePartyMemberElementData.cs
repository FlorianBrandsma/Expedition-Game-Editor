using UnityEngine;

//Maybe remove this
public class GamePartyMemberElementData : GeneralData, IElementData
{
    public DataElement DataElement { get; set; }

    public GamePartyMemberElementData() : base()
    {
        DataType = Enums.DataType.GamePartyMember;
    }

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
