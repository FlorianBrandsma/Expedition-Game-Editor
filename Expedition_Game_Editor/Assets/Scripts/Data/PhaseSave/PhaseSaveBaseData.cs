using UnityEngine;

public class PhaseSaveBaseData
{
    public int Id               { get; set; }
    
    public int SaveId           { get; set; }
    public int ChapterSaveId    { get; set; }
    public int PhaseId          { get; set; }

    public int Index            { get; set; }

    public bool Complete        { get; set; }

    public virtual void GetOriginalValues(PhaseSaveData originalData)
    {
        Id              = originalData.Id;

        SaveId          = originalData.SaveId;
        ChapterSaveId   = originalData.ChapterSaveId;
        PhaseId         = originalData.PhaseId;

        Index           = originalData.Index;

        Complete        = originalData.Complete;
}

    public virtual void Clone(PhaseSaveData data)
    {
        data.Id             = Id;

        data.SaveId         = SaveId;
        data.ChapterSaveId  = ChapterSaveId;
        data.PhaseId        = PhaseId;

        data.Index          = Index;

        data.Complete       = Complete;
    }
}
