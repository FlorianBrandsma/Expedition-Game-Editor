using UnityEngine;

public class UserBaseData
{
    public int Id               { get; set; }

    public int IconId           { get; set; }

    public string Username      { get; set; }
    public string Email         { get; set; }
    public string Password      { get; set; }

    public virtual void GetOriginalValues(UserData originalData)
    {
        Id          = originalData.Id;

        IconId      = originalData.IconId;

        Username    = originalData.Username;
        Email       = originalData.Email;
        Password    = originalData.Password;
    }

    public virtual void Clone(UserData data)
    {
        data.Id         = Id;

        data.IconId     = IconId;

        data.Username   = Username;
        data.Email      = Email;
        data.Password   = Password;
    }
}
