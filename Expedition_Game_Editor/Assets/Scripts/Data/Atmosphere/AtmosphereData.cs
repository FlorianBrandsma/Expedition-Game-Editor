using UnityEngine;

public class AtmosphereData : AtmosphereBaseData
{
    public string RegionName        { get; set; }
    public string TerrainName       { get; set; }

    public string IconPath          { get; set; }
    public string BaseTilePath      { get; set; }

    public bool TimeConflict        { get; set; }
    public bool ContainsActiveTime  { get; set; }

    public override void GetOriginalValues(AtmosphereData originalData)
    {
        RegionName          = originalData.RegionName;
        TerrainName         = originalData.TerrainName;

        IconPath            = originalData.IconPath;
        BaseTilePath        = originalData.BaseTilePath;

        TimeConflict        = originalData.TimeConflict;
        ContainsActiveTime  = originalData.ContainsActiveTime;

        base.GetOriginalValues(originalData);
    }

    public AtmosphereData Clone()
    {
        var data = new AtmosphereData();
        
        data.RegionName         = RegionName;
        data.TerrainName        = TerrainName;

        data.IconPath           = IconPath;
        data.BaseTilePath       = BaseTilePath;

        data.TimeConflict       = TimeConflict;
        data.ContainsActiveTime = ContainsActiveTime;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(AtmosphereElementData elementData)
    {
        elementData.RegionName          = RegionName;
        elementData.TerrainName         = TerrainName;

        elementData.IconPath            = IconPath;
        elementData.BaseTilePath        = BaseTilePath;

        elementData.TimeConflict        = TimeConflict;
        elementData.ContainsActiveTime  = ContainsActiveTime;

        base.Clone(elementData);
    }
}
