using UnityEngine;

public class GameData : GameBaseData
{
    public bool Installed       { get; set; }

    public string IconPath      { get; set; }

    public string Name          { get; set; }
    public string Description   { get; set; }
    
    public override void GetOriginalValues(GameData originalData)
    {
        Installed   = originalData.Installed;

        IconPath    = originalData.IconPath;

        Name        = originalData.Name;
        Description = originalData.Description;

        base.GetOriginalValues(originalData);
    }

    public GameData Clone()
    {
        var data = new GameData();

        data.Installed      = Installed;

        data.IconPath       = IconPath;

        data.Name           = Name;
        data.Description    = Description;
        
        base.Clone(data);

        return data;
    }

    public virtual void Clone(GameElementData elementData)
    {
        elementData.Installed   = Installed;

        elementData.IconPath    = IconPath;

        elementData.Name        = Name;
        elementData.Description = Description;
        
        base.Clone(elementData);
    }
}
