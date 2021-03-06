﻿using UnityEngine;

public class InteractionSaveBaseData
{
    public int Id               { get; set; }
    
    public int SaveId           { get; set; }
    public int InteractionId    { get; set; }

    public bool Complete        { get; set; }

    public virtual void GetOriginalValues(InteractionSaveData originalData)
    {
        Id              = originalData.Id;

        SaveId          = originalData.SaveId;
        InteractionId   = originalData.InteractionId;

        Complete        = originalData.Complete;
}

    public virtual void Clone(InteractionSaveData data)
    {
        data.Id             = Id;

        data.SaveId         = SaveId;
        data.InteractionId  = InteractionId;

        data.Complete       = Complete;
    }
}
