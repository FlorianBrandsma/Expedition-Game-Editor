using UnityEngine;

public class IconBaseData 
{
    public int Id       { get; set; }

    public int Category { get; set; }
    public string Path  { get; set; }

    public virtual void GetOriginalValues(IconData originalData)
    {
        Id          = originalData.Id;

        Category    = originalData.Category;
        Path        = originalData.Path;
    }

    public virtual void Clone(IconData data)
    {
        data.Id         = Id;

        data.Category   = Category;
        data.Path       = Path;
    }
}
