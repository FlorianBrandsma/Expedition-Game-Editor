using UnityEngine;

public class ModelBaseData
{
    public int Id       { get; set; }

    public int IconId   { get; set; }

    public string Name  { get; set; }
    public string Path  { get; set; }

    public float Height { get; set; }
    public float Width  { get; set; }
    public float Depth  { get; set; }

    public virtual void GetOriginalValues(ModelData originalData)
    {
        Id      = originalData.Id;

        IconId  = originalData.IconId;

        Name    = originalData.Name;
        Path    = originalData.Path;

        Height  = originalData.Height;
        Width   = originalData.Width;
        Depth   = originalData.Depth;
    }

    public virtual void Clone(ModelData data)
    {
        data.Id     = Id;

        data.IconId = IconId;

        data.Name   = Name;
        data.Path   = Path;

        data.Height = Height;
        data.Width  = Width;
        data.Depth  = Depth;
    }
}
