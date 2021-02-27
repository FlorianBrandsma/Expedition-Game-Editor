using UnityEngine;

public class TeamBaseData
{
    public int Id               { get; set; }

    public int IconId           { get; set; }

    public string Name          { get; set; }
    public string Description   { get; set; }

    public virtual void GetOriginalValues(TeamData originalData)
    {
        Id          = originalData.Id;

        IconId      = originalData.IconId;

        Name        = originalData.Name;
        Description = originalData.Description;
    }

    public virtual void Clone(TeamData data)
    {
        data.Id             = Id;

        data.IconId         = IconId;

        data.Name           = Name;
        data.Description    = Description;
    }
}
