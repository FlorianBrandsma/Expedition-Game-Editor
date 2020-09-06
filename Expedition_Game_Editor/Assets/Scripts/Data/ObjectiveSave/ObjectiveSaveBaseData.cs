using UnityEngine;

public class ObjectiveSaveBaseData
{
    public int Id           { get; set; }
    
    public int SaveId       { get; set; }
    public int QuestSaveId  { get; set; }
    public int ObjectiveId  { get; set; }

    public int Index        { get; set; }

    public bool Complete    { get; set; }

    public virtual void GetOriginalValues(ObjectiveSaveData originalData)
    {
        Id          = originalData.Id;

        SaveId      = originalData.SaveId;
        QuestSaveId = originalData.QuestSaveId;
        ObjectiveId = originalData.ObjectiveId;

        Index       = originalData.Index;

        Complete    = originalData.Complete;
}

    public virtual void Clone(ObjectiveSaveData data)
    {
        data.Id             = Id;

        data.SaveId         = SaveId;
        data.QuestSaveId    = QuestSaveId;
        data.ObjectiveId    = ObjectiveId;

        data.Index          = Index;

        data.Complete       = Complete;
    }
}
