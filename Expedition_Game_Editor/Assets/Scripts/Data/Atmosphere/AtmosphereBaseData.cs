using UnityEngine;

public class AtmosphereBaseData
{
    public int Id               { get; set; }

    public int TerrainId        { get; set; }

    public bool Default         { get; set; }

    public int StartTime        { get; set; }
    public int EndTime          { get; set; }

    public string EditorNotes   { get; set; }
    public string GameNotes     { get; set; }

    public virtual void GetOriginalValues(AtmosphereData originalData)
    {
        Id              = originalData.Id;

        TerrainId       = originalData.TerrainId;

        Default         = originalData.Default;

        StartTime       = originalData.StartTime;
        EndTime         = originalData.EndTime;

        EditorNotes     = originalData.EditorNotes;
        GameNotes       = originalData.GameNotes;
    }

    public virtual void Clone(AtmosphereData data)
    {
        data.Id             = Id;

        data.TerrainId      = TerrainId;

        data.Default        = Default;

        data.StartTime      = StartTime;
        data.EndTime        = EndTime;

        data.EditorNotes    = EditorNotes;
        data.GameNotes      = GameNotes;
    }
}
