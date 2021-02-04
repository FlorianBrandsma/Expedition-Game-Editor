using UnityEngine;
using System;

public class SaveBaseData
{
    public int Id                   { get; set; }
    
    public int GameId               { get; set; }
    public int RegionId             { get; set; }
    public int WorldInteractableId  { get; set; }

    public int Index                { get; set; }

    public float PositionX          { get; set; }
    public float PositionY          { get; set; }
    public float PositionZ          { get; set; }

    public int GameTime             { get; set; }
    public int PlayTime             { get; set; }
    public DateTime SaveTime        { get; set; }

    public virtual void GetOriginalValues(SaveData originalData)
    {
        Id                  = originalData.Id;

        GameId              = originalData.GameId;
        RegionId            = originalData.RegionId;
        WorldInteractableId = originalData.WorldInteractableId;

        Index               = originalData.Index;

        PositionX           = originalData.PositionX;
        PositionY           = originalData.PositionY;
        PositionZ           = originalData.PositionZ;

        GameTime            = originalData.GameTime;
        
        PlayTime            = originalData.PlayTime;
        SaveTime            = originalData.SaveTime;
    }

    public virtual void Clone(SaveData data)
    {
        data.Id                     = Id;

        data.GameId                 = GameId;
        data.RegionId               = RegionId;
        data.WorldInteractableId    = WorldInteractableId;

        data.Index                  = Index;

        data.PositionX              = PositionX;
        data.PositionY              = PositionY;
        data.PositionZ              = PositionZ;

        data.GameTime               = GameTime;
        
        data.PlayTime               = PlayTime;
        data.SaveTime               = SaveTime;
    }
}
