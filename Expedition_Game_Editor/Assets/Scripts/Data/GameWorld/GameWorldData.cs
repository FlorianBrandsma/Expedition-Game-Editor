using UnityEngine;
using System.Collections.Generic;

public class GameWorldData
{
    public int Id                           { get; set; }

    public ChapterElementData ChapterData   { get; set; }
    public PhaseElementData PhaseData       { get; set; }

    public List<GameRegionElementData> RegionDataList       { get; set; } = new List<GameRegionElementData>();
    public List<GamePartyMemberElementData> PartyMemberList { get; set; } = new List<GamePartyMemberElementData>();

    //[Might have to go in terrain to preserve the possibility to generate terrains]
    //There's actually no real way to know what terrain a world interactable belongs to
    //Generated interactables/interactions can be bound to a terrain by id and removed from the list when necessary
    public List<GameWorldInteractableElementData> WorldInteractableDataList { get; set; } = new List<GameWorldInteractableElementData>();

    public virtual void GetOriginalValues(GameWorldData originalData)
    {
        Id          = originalData.Id;

        ChapterData = originalData.ChapterData;
        PhaseData   = originalData.PhaseData;
    }

    public GameWorldData Clone()
    {
        var data = new GameWorldData();

        data.Id             = Id;

        data.ChapterData    = ChapterData;
        data.PhaseData      = PhaseData;
        
        return data;
    }

    public virtual void Clone(GameWorldElementData elementData)
    {
        elementData.Id          = Id;

        elementData.ChapterData = ChapterData;
        elementData.PhaseData   = PhaseData;
    }
}
