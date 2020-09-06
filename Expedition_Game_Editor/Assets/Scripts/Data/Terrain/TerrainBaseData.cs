using UnityEngine;

public class TerrainBaseData
{
    public int Id       { get; set; }

    public int RegionId { get; set; }
    public int IconId   { get; set; }

    public int Index    { get; set; }

    public string Name  { get; set; }

    public virtual void GetOriginalValues(TerrainData originalData)
    {
        Id          = originalData.Id;

        RegionId    = originalData.RegionId;
        IconId      = originalData.IconId;

        Index       = originalData.Index;

        Name        = originalData.Name;
    }

    public virtual void Clone(TerrainData data)
    {
        data.Id         = Id;

        data.RegionId   = RegionId;
        data.IconId     = IconId;

        data.Index      = Index;

        data.Name       = Name;
    }
}
