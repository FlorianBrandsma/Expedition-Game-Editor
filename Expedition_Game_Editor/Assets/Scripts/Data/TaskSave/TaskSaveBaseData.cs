using UnityEngine;

public class TaskSaveBaseData
{
    public int Id                   { get; set; }
    
    public int SaveId               { get; set; }
    public int TaskId               { get; set; }
    public int WorldInteractableId  { get; set; }

    public bool Complete            { get; set; }

    public virtual void GetOriginalValues(TaskSaveData originalData)
    {
        Id                  = originalData.Id;

        SaveId              = originalData.SaveId;
        TaskId              = originalData.TaskId;
        WorldInteractableId = originalData.WorldInteractableId;

        Complete            = originalData.Complete;
    }

    public virtual void Clone(TaskSaveData data)
    {
        data.Id                     = Id;

        data.SaveId                 = SaveId;
        data.TaskId                 = TaskId;
        data.WorldInteractableId    = WorldInteractableId;

        data.Complete               = Complete;
    }
}
