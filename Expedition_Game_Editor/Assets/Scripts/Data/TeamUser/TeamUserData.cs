using UnityEngine;

public class TeamUserData : TeamUserBaseData
{
    public string IconPath { get; set; }

    public string Username { get; set; }

    public override void GetOriginalValues(TeamUserData originalData)
    {
        IconPath = originalData.IconPath;

        Username = originalData.Username;

        base.GetOriginalValues(originalData);
    }

    public TeamUserData Clone()
    {
        var data = new TeamUserData();

        data.IconPath = IconPath;

        data.Username = Username;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(TeamUserElementData elementData)
    {
        elementData.IconPath = IconPath;

        elementData.Username = Username;

        base.Clone(elementData);
    }
}
