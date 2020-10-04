using UnityEngine;
using System.Collections.Generic;

public class GameWorldData
{
    public int Id                           { get; set; }

    public ChapterElementData ChapterData   { get; set; }
    public PhaseElementData PhaseData       { get; set; }
    
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
