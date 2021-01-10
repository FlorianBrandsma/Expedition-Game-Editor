using UnityEngine;

public class ObjectiveSaveBaseData
{
    public int Id           { get; set; }
    
    public int SaveId       { get; set; }
    public int ObjectiveId  { get; set; }

    public bool Complete    { get; set; }

    public virtual void GetOriginalValues(ObjectiveSaveData originalData)
    {
        Id          = originalData.Id;

        SaveId      = originalData.SaveId;
        ObjectiveId = originalData.ObjectiveId;

        Complete    = originalData.Complete;
}

    public virtual void Clone(ObjectiveSaveData data)
    {
        data.Id             = Id;

        data.SaveId         = SaveId;
        data.ObjectiveId    = ObjectiveId;

        data.Complete       = Complete;
    }
}
