using UnityEngine;

public class ProjectData : ProjectBaseData
{
    public string TeamName { get; set; }

    public string IconPath { get; set; }

    public override void GetOriginalValues(ProjectData originalData)
    {
        TeamName = originalData.TeamName;

        IconPath = originalData.IconPath;

        base.GetOriginalValues(originalData);
    }

    public ProjectData Clone()
    {
        var data = new ProjectData();

        data.TeamName = TeamName;

        data.IconPath = IconPath;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(ProjectElementData elementData)
    {
        elementData.TeamName = TeamName;

        elementData.IconPath = IconPath;

        base.Clone(elementData);
    }
}
