using UnityEngine;

public class PlayerSaveBaseData
{
    public int Id                   { get; set; }

    public int SaveId               { get; set; }
    public int RegionId             { get; set; }
    public int WorldInteractableId  { get; set; }

    public float PositionX          { get; set; }
    public float PositionY          { get; set; }
    public float PositionZ          { get; set; }

    public float Scale              { get; set; }

    public int GameTime             { get; set; }
    public int PlayedTime           { get; set; }

    public virtual void GetOriginalValues(PlayerSaveData originalData)
    {
        Id                  = originalData.Id;

        SaveId              = originalData.SaveId;
        RegionId            = originalData.RegionId;
        WorldInteractableId = originalData.WorldInteractableId;

        PositionX           = originalData.PositionX;
        PositionY           = originalData.PositionY;
        PositionZ           = originalData.PositionZ;

        Scale               = originalData.Scale;

        GameTime            = originalData.GameTime;
        PlayedTime          = originalData.PlayedTime;
    }

    public virtual void Clone(PlayerSaveData data)
    {
        data.Id                     = Id;

        data.SaveId                 = SaveId;
        data.RegionId               = RegionId;
        data.WorldInteractableId    = WorldInteractableId;

        data.PositionX              = PositionX;
        data.PositionY              = PositionY;
        data.PositionZ              = PositionZ;

        data.Scale                  = Scale;

        data.GameTime               = GameTime;
        data.PlayedTime             = PlayedTime;
    }
}
