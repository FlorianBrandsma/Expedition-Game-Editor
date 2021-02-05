using UnityEngine;

public class PhaseBaseData
{
    public int Id                   { get; set; }
    
    public int ChapterId            { get; set; }
    public int DefaultRegionId      { get; set; }

    public int Index                { get; set; }

    public string Name              { get; set; }

    public float DefaultPositionX   { get; set; }
    public float DefaultPositionY   { get; set; }
    public float DefaultPositionZ   { get; set; }

    public int DefaultRotationX     { get; set; }
    public int DefaultRotationY     { get; set; }
    public int DefaultRotationZ     { get; set; }

    public int DefaultTime          { get; set; }

    public string EditorNotes       { get; set; }
    public string GameNotes         { get; set; }

    public virtual void GetOriginalValues(PhaseData originalData)
    {
        Id                  = originalData.Id;

        ChapterId           = originalData.ChapterId;
        DefaultRegionId     = originalData.DefaultRegionId;

        Index               = originalData.Index;

        Name                = originalData.Name;

        DefaultPositionX    = originalData.DefaultPositionX;
        DefaultPositionY    = originalData.DefaultPositionY;
        DefaultPositionZ    = originalData.DefaultPositionZ;

        DefaultRotationX    = originalData.DefaultRotationX;
        DefaultRotationY    = originalData.DefaultRotationY;
        DefaultRotationZ    = originalData.DefaultRotationZ;

        DefaultTime         = originalData.DefaultTime;

        EditorNotes         = originalData.EditorNotes;
        GameNotes           = originalData.GameNotes;
    }

    public virtual void Clone(PhaseData data)
    {
        data.Id                 = Id;

        data.ChapterId          = ChapterId;
        data.DefaultRegionId    = DefaultRegionId;

        data.Index              = Index;

        data.Name               = Name;

        data.DefaultPositionX   = DefaultPositionX;
        data.DefaultPositionY   = DefaultPositionY;
        data.DefaultPositionZ   = DefaultPositionZ;

        data.DefaultRotationX   = DefaultRotationX;
        data.DefaultRotationY   = DefaultRotationY;
        data.DefaultRotationZ   = DefaultRotationZ;

        data.DefaultTime        = DefaultTime;

        data.EditorNotes        = EditorNotes;
        data.GameNotes          = GameNotes;
    }
}
