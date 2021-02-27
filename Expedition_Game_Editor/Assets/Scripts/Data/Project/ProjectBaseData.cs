using UnityEngine;

public class ProjectBaseData
{
    public int Id               { get; set; }

    public int TeamId           { get; set; }

    public int IconId           { get; set; }

    public string Name          { get; set; }
    public string Description   { get; set; }
    
    public virtual void GetOriginalValues(ProjectData originalData)
    {
        Id          = originalData.Id;

        TeamId      = originalData.TeamId;

        IconId      = originalData.IconId;

        Name        = originalData.Name;
        Description = originalData.Description;
    }

    public virtual void Clone(ProjectData data)
    {
        data.Id             = Id;

        data.TeamId         = TeamId;

        data.IconId         = IconId;
        
        data.Name           = Name;
        data.Description    = Description;
    }
}
