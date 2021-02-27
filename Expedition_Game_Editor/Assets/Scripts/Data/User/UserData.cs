using UnityEngine;

public class UserData : UserBaseData
{
    public string IconPath { get; set; }

    public override void GetOriginalValues(UserData originalData)
    {
        IconPath = originalData.IconPath;

        base.GetOriginalValues(originalData);
    }

    public UserData Clone()
    {
        var data = new UserData();

        data.IconPath = IconPath;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(UserElementData elementData)
    {
        elementData.IconPath = IconPath;

        base.Clone(elementData);
    }
}
