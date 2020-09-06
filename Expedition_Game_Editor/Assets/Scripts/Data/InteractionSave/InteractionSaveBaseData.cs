using UnityEngine;

public class InteractionSaveBaseData
{
    public int Id               { get; set; }
    
    public int SaveId           { get; set; }
    public int TaskSaveId       { get; set; }
    public int InteractionId    { get; set; }

    public int Index            { get; set; }

    public bool Complete        { get; set; }

    public virtual void GetOriginalValues(InteractionSaveData originalData)
    {
        Id              = originalData.Id;

        SaveId          = originalData.SaveId;
        TaskSaveId      = originalData.TaskSaveId;
        InteractionId   = originalData.InteractionId;

        Index           = originalData.Index;

        Complete        = originalData.Complete;
}

    public virtual void Clone(InteractionSaveData data)
    {
        data.Id             = Id;

        data.SaveId         = SaveId;
        data.TaskSaveId     = TaskSaveId;
        data.InteractionId  = InteractionId;

        data.Index          = Index;

        data.Complete       = Complete;
    }
}
