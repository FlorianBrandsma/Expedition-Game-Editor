using UnityEngine;

public class TeamData : TeamBaseData
{
    public string IconPath { get; set; }

    public int MemberCount { get; set; }

    public override void GetOriginalValues(TeamData originalData)
    {
        IconPath    = originalData.IconPath;

        MemberCount = originalData.MemberCount;

        base.GetOriginalValues(originalData);
    }

    public TeamData Clone()
    {
        var data = new TeamData();

        data.IconPath       = IconPath;

        data.MemberCount    = MemberCount;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(TeamElementData elementData)
    {
        elementData.IconPath    = IconPath;

        elementData.MemberCount = MemberCount;

        base.Clone(elementData);
    }
}
