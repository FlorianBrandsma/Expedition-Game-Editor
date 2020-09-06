using UnityEngine;

public class TaskSaveBaseData
{
    public int Id                   { get; set; }
    
    public int SaveId               { get; set; }
    public int WorldInteractableId  { get; set; }
    public int ObjectiveSaveId      { get; set; }
    public int TaskId               { get; set; }

    public int Index                { get; set; }

    public bool Complete            { get; set; }

    public virtual void GetOriginalValues(TaskSaveData originalData)
    {
        Id                  = originalData.Id;

        SaveId              = originalData.SaveId;
        WorldInteractableId = originalData.WorldInteractableId;
        ObjectiveSaveId     = originalData.ObjectiveSaveId;
        TaskId              = originalData.TaskId;

        Index               = originalData.Index;

        Complete            = originalData.Complete;
    }

    public virtual void Clone(TaskSaveData data)
    {
        data.Id                     = Id;

        data.SaveId                 = SaveId;
        data.WorldInteractableId    = WorldInteractableId;
        data.ObjectiveSaveId        = ObjectiveSaveId;
        data.TaskId                 = TaskId;

        data.Index                  = Index;

        data.Complete               = Complete;
    }
}
