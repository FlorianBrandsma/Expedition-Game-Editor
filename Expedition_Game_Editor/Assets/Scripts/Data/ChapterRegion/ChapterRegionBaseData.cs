using UnityEngine;

public class ChapterRegionBaseData
{
    public int Id           { get; set; }
    
    public int ChapterId    { get; set; }
    public int RegionId     { get; set; }

    public virtual void GetOriginalValues(ChapterRegionData originalData)
    {
        Id          = originalData.Id;

        ChapterId   = originalData.ChapterId;
        RegionId    = originalData.RegionId;

    }

    public virtual void Clone(ChapterRegionData data)
    {
        data.Id         = Id;

        data.ChapterId  = ChapterId;
        data.RegionId   = RegionId;
    }
}
