using UnityEngine;
using System.Collections.Generic;

public class GameWorldElementData : GeneralData, IElementData
{
    public DataElement DataElement { get; set; }

    public GameWorldElementData() : base()
    {
        DataType = Enums.DataType.GameWorld;
    }

    public ChapterElementData chapterData;
    public PhaseElementData phaseData;

    public List<GamePartyMemberElementData> partyMemberList = new List<GamePartyMemberElementData>();

    //[Might have to go in terrain to preserve the possibility to generate terrains]
    //There's actually no real way to know what terrain a world interactable belongs to
    //Generated interactables/interactions can be bound to a terrain by id and removed from the list when necessary
    public List<GameWorldInteractableElementData> worldInteractableDataList = new List<GameWorldInteractableElementData>();

    public List<GameRegionElementData> regionDataList;

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
        var elementData = new GameWorldElementData();

        CloneGeneralData(elementData);

        return elementData;
    }
    #endregion
}
