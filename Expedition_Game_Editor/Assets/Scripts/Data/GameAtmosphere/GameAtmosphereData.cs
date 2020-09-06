using UnityEngine;

public class GameAtmosphereData
{
    public int Id           { get; set; }

    public int TerrainId    { get; set; }

    public bool Default     { get; set; }

    public int StartTime    { get; set; }
    public int EndTime      { get; set; }

    public virtual void GetOriginalValues(GameAtmosphereData originalData)
    {
        Id          = originalData.Id;

        TerrainId   = originalData.TerrainId;

        Default     = originalData.Default;

        StartTime   = originalData.StartTime;
        EndTime     = originalData.EndTime;
    }

    public GameAtmosphereData Clone()
    {
        var data = new GameAtmosphereData();
        
        data.Id         = Id;

        data.TerrainId  = TerrainId;

        data.Default    = Default;

        data.StartTime  = StartTime;
        data.EndTime    = EndTime;

        return data;
    }

    public virtual void Clone(GameAtmosphereElementData elementData)
    {
        elementData.Id          = Id;

        elementData.TerrainId   = TerrainId;

        elementData.Default     = Default;

        elementData.StartTime   = StartTime;
        elementData.EndTime     = EndTime;
    }
}
