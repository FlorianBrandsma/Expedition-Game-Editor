using UnityEngine;

public class InteractableSaveBaseData
{
    public int Id               { get; set; }
    
    public int SaveId           { get; set; }
    public int InteractableId   { get; set; }

    public virtual void GetOriginalValues(InteractableSaveData originalData)
    {
        Id              = originalData.Id;

        SaveId          = originalData.SaveId;
        InteractableId  = originalData.InteractableId;
    }

    public virtual void Clone(InteractableSaveData data)
    {
        data.Id             = Id;

        data.SaveId         = SaveId;
        data.InteractableId = InteractableId;
    }
}
