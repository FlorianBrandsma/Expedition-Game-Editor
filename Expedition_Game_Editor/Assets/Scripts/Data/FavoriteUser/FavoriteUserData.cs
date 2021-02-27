using UnityEngine;

public class FavoriteUserData : FavoriteUserBaseData
{
    public string IconPath { get; set; }

    public string Username { get; set; }

    public override void GetOriginalValues(FavoriteUserData originalData)
    {
        IconPath = originalData.IconPath;

        Username = originalData.Username;

        base.GetOriginalValues(originalData);
    }

    public FavoriteUserData Clone()
    {
        var data = new FavoriteUserData();

        data.IconPath = IconPath;

        data.Username = Username;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(FavoriteUserElementData elementData)
    {
        elementData.IconPath = IconPath;

        elementData.Username = Username;

        base.Clone(elementData);
    }
}
