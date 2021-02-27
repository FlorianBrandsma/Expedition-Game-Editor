using UnityEngine;

public class TeamUserBaseData
{
    public int Id               { get; set; }

    public int TeamId           { get; set; }
    public int UserId           { get; set; }

    public int Role             { get; set; }

    public int Status           { get; set; }
    
    public virtual void GetOriginalValues(TeamUserData originalData)
    {
        Id          = originalData.Id;

        TeamId      = originalData.TeamId;
        UserId      = originalData.UserId;

        Role        = originalData.Role;

        Status    = originalData.Status;
    }

    public virtual void Clone(TeamUserData data)
    {
        data.Id         = Id;

        data.TeamId     = TeamId;
        data.UserId     = UserId;

        data.Role       = Role;

        data.Status   = Status;
    }
}
