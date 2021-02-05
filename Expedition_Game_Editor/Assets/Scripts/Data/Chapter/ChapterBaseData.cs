using UnityEngine;

public class ChapterBaseData
{
    public int Id               { get; set; }

    public int Index            { get; set; }

    public string Name          { get; set; }

    public float TimeSpeed      { get; set; }

    public string EditorNotes   { get; set; }
    public string PrivateNotes  { get; set; }

    public virtual void GetOriginalValues(ChapterData originalData)
    {
        Id              = originalData.Id;

        Index           = originalData.Index;

        Name            = originalData.Name;

        TimeSpeed       = originalData.TimeSpeed;

        EditorNotes     = originalData.EditorNotes;
        PrivateNotes    = originalData.PrivateNotes;
    }

    public virtual void Clone(ChapterData data)
    {
        data.Id             = Id;

        data.Index          = Index;

        data.Name           = Name;

        data.TimeSpeed      = TimeSpeed;

        data.EditorNotes    = EditorNotes;
        data.PrivateNotes   = PrivateNotes;
    }
}
