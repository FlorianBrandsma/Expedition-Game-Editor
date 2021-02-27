using UnityEngine;

public class FavoriteUserBaseData
{
    public int Id               { get; set; }

    public int UserId           { get; set; }
    public int FavoriteUserId   { get; set; }

    public string Note          { get; set; }

    public virtual void GetOriginalValues(FavoriteUserData originalData)
    {
        Id                  = originalData.Id;

        UserId              = originalData.UserId;
        FavoriteUserId      = originalData.FavoriteUserId;
        
        Note                = originalData.Note;
    }

    public virtual void Clone(FavoriteUserData data)
    {
        data.Id                 = Id;

        data.UserId             = UserId;
        data.FavoriteUserId     = FavoriteUserId;

        data.Note               = Note;
    }
}
