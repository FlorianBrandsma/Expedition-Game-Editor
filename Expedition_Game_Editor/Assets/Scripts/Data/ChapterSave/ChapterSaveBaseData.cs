using UnityEngine;

public class ChapterSaveBaseData
{
    public int Id           { get; set; }
    
    public int SaveId       { get; set; }
    public int ChapterId    { get; set; }

    public bool Complete    { get; set; }

    public virtual void GetOriginalValues(ChapterSaveData originalData)
    {
        Id          = originalData.Id;

        SaveId      = originalData.SaveId;
        ChapterId   = originalData.ChapterId;

        Complete    = originalData.Complete;
    }

    public virtual void Clone(ChapterSaveData data)
    {
        data.Id         = Id;

        data.SaveId     = SaveId;
        data.ChapterId  = ChapterId;

        data.Complete   = Complete;
    }
}
