using UnityEngine;

public class SaveBaseData
{
    public int Id       { get; set; }
    
    public int GameId   { get; set; }

    public int Index    { get; set; }

    public virtual void GetOriginalValues(SaveData originalData)
    {
        Id      = originalData.Id;

        GameId  = originalData.GameId;

        Index   = originalData.Index;
    }

    public virtual void Clone(SaveData data)
    {
        data.Id     = Id;

        data.GameId = GameId;

        data.Index  = Index;
    }
}
