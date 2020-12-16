using UnityEngine;

public class SceneBaseData
{
    public int Id                   { get; set; }

    public int OutcomeId            { get; set; }
    public int RegionId             { get; set; }
    
    public int Index                { get; set; }

    public string Name              { get; set; }

    public bool FreezeTime          { get; set; }
    public bool AutoContinue        { get; set; }
    public bool SetActorsInstantly  { get; set; }

    public float SceneDuration      { get; set; }
    public float ShotDuration       { get; set; }

    public string PublicNotes       { get; set; }
    public string PrivateNotes      { get; set; }

    public virtual void GetOriginalValues(SceneData originalData)
    {
        Id                  = originalData.Id;

        OutcomeId           = originalData.OutcomeId;
        RegionId            = originalData.RegionId;

        Index               = originalData.Index;

        Name                = originalData.Name;

        FreezeTime          = originalData.FreezeTime;
        AutoContinue        = originalData.AutoContinue;
        SetActorsInstantly  = originalData.SetActorsInstantly;

        SceneDuration       = originalData.SceneDuration;
        ShotDuration        = originalData.ShotDuration;
        
        PublicNotes         = originalData.PublicNotes;
        PrivateNotes        = originalData.PrivateNotes;
    }

    public virtual void Clone(SceneData data)
    {
        data.Id                 = Id;

        data.OutcomeId          = OutcomeId;
        data.RegionId           = RegionId;

        data.Index              = Index;

        data.Name               = Name;

        data.FreezeTime         = FreezeTime;
        data.AutoContinue       = AutoContinue;
        data.SetActorsInstantly = SetActorsInstantly;

        data.SceneDuration      = SceneDuration;
        data.ShotDuration       = ShotDuration;
        
        data.PublicNotes        = PublicNotes;
        data.PrivateNotes       = PrivateNotes;
    }
}
