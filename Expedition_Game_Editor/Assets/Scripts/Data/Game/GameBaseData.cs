using UnityEngine;

public class GameBaseData
{
    public int Id               { get; set; }

    public int ProjectId        { get; set; }

    public bool Preview         { get; set; }

    public float Rating         { get; set; }

    public virtual void GetOriginalValues(GameData originalData)
    {
        Id          = originalData.Id;

        ProjectId   = originalData.ProjectId;

        Preview     = originalData.Preview;

        Rating      = originalData.Rating;
    }

    public virtual void Clone(GameData data)
    {
        data.Id             = Id;

        data.ProjectId      = ProjectId;

        data.Preview        = Preview;

        data.Rating         = Rating;
    }
}
