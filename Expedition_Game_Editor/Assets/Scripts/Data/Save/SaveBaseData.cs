using UnityEngine;

public class SaveBaseData
{
    public int Id       { get; set; }
    
    public int GameId   { get; set; }

    public virtual void GetOriginalValues(SaveData originalData)
    {
        Id      = originalData.Id;

        GameId  = originalData.GameId;
    }

    public virtual void Clone(SaveData data)
    {
        data.Id     = Id;

        data.GameId = GameId;
    }
}
