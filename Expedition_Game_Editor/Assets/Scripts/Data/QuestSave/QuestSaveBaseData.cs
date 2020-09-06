using UnityEngine;

public class QuestSaveBaseData
{
    public int Id           { get; set; }
    
    public int SaveId       { get; set; }
    public int PhaseSaveId  { get; set; }
    public int QuestId      { get; set; }

    public int Index        { get; set; }

    public bool Complete    { get; set; }

    public virtual void GetOriginalValues(QuestSaveData originalData)
    {
        Id          = originalData.Id;

        SaveId      = originalData.SaveId;
        PhaseSaveId = originalData.PhaseSaveId;
        QuestId     = originalData.QuestId;

        Index       = originalData.Index;

        Complete    = originalData.Complete;
    }

    public virtual void Clone(QuestSaveData data)
    {
        data.Id             = Id;

        data.SaveId         = SaveId;
        data.PhaseSaveId    = PhaseSaveId;
        data.QuestId        = QuestId;

        data.Index          = Index;

        data.Complete       = Complete;
    }
}
