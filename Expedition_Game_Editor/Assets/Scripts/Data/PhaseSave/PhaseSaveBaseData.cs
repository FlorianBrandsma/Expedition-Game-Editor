using UnityEngine;

public class PhaseSaveBaseData
{
    public int Id               { get; set; }
    
    public int SaveId           { get; set; }
    public int PhaseId          { get; set; }

    public bool Complete        { get; set; }

    public virtual void GetOriginalValues(PhaseSaveData originalData)
    {
        Id              = originalData.Id;

        SaveId          = originalData.SaveId;
        PhaseId         = originalData.PhaseId;

        Complete        = originalData.Complete;
}

    public virtual void Clone(PhaseSaveData data)
    {
        data.Id             = Id;

        data.SaveId         = SaveId;
        data.PhaseId        = PhaseId;

        data.Complete       = Complete;
    }
}
