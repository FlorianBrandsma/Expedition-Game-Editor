using UnityEngine;

public class ChapterInteractableBaseData
{
    public int Id               { get; set; }

    public int ChapterId        { get; set; }
    public int InteractableId   { get; set; }

    public virtual void GetOriginalValues(ChapterInteractableData originalData)
    {
        Id              = originalData.Id;

        ChapterId       = originalData.ChapterId;
        InteractableId  = originalData.InteractableId;
    }

    public virtual void Clone(ChapterInteractableData data)
    {
        data.Id             = Id;

        data.ChapterId      = ChapterId;
        data.InteractableId = InteractableId;
    }
}
