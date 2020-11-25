using UnityEngine;

public class CameraFilterBaseData
{
    public int Id           { get; set; }
    
    public string Path      { get; set; }
    public string IconPath  { get; set; }

    public string Name      { get; set; }

    public virtual void GetOriginalValues(CameraFilterData originalData)
    {
        Id          = originalData.Id;

        Path        = originalData.Path;
        IconPath    = originalData.IconPath;
        
        Name        = originalData.Name;
    }

    public virtual void Clone(CameraFilterData data)
    {
        data.Id         = Id;

        data.Path       = Path;
        data.IconPath   = IconPath;
        
        data.Name       = Name;
    }
}
