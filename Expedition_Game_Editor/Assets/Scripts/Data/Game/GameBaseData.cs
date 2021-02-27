using UnityEngine;

public class GameBaseData
{
    public int Id               { get; set; }

    public int ProjectId        { get; set; }

    public int Rating           { get; set; }

    public virtual void GetOriginalValues(GameData originalData)
    {
        Id          = originalData.Id;

        ProjectId   = originalData.ProjectId;

        Rating      = originalData.Rating;
    }

    public virtual void Clone(GameData data)
    {
        data.Id             = Id;

        data.ProjectId      = ProjectId;

        data.Rating         = Rating;
    }
}
