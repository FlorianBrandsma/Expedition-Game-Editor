using UnityEngine;

public class WorldInteractableBaseData
{
    public int Id                       { get; set; }

    public int ChapterId                { get; set; }
    public int PhaseId                  { get; set; }
    public int QuestId                  { get; set; }
    public int ObjectiveId              { get; set; }

    public int ChapterInteractableId    { get; set; }
    public int InteractableId           { get; set; }

    public int Type                     { get; set; }

    public virtual void GetOriginalValues(WorldInteractableData originalData)
    {
        Id                      = originalData.Id;

        ChapterId               = originalData.ChapterId;
        PhaseId                 = originalData.PhaseId;
        QuestId                 = originalData.QuestId;
        ObjectiveId             = originalData.ObjectiveId;

        ChapterInteractableId   = originalData.ChapterInteractableId;
        InteractableId          = originalData.InteractableId;

        Type                    = originalData.Type;
    }

    public virtual void Clone(WorldInteractableData data)
    {
        data.Id                     = Id;

        data.ChapterId              = ChapterId;
        data.PhaseId                = PhaseId;
        data.QuestId                = QuestId;
        data.ObjectiveId            = ObjectiveId;

        data.ChapterInteractableId  = ChapterInteractableId;
        data.InteractableId         = InteractableId;

        data.Type                   = Type;
    }
}
