using UnityEngine;

public class TeamData : TeamBaseData
{
    public string IconPath { get; set; }

    public override void GetOriginalValues(TeamData originalData)
    {
        IconPath = originalData.IconPath;

        base.GetOriginalValues(originalData);
    }

    public TeamData Clone()
    {
        var data = new TeamData();

        data.IconPath = IconPath;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(TeamElementData elementData)
    {
        elementData.IconPath = IconPath;

        base.Clone(elementData);
    }
}
