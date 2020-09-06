using UnityEngine;

public class WorldInteractableBaseData
{
    public int Id                       { get; set; }

    public int PhaseId                  { get; set; }
    public int QuestId                  { get; set; }
    public int ObjectiveId              { get; set; }

    public int ChapterInteractableId    { get; set; }
    public int InteractableId           { get; set; }

    public int Index                    { get; set; }

    public int Type                     { get; set; }

    public virtual void GetOriginalValues(WorldInteractableData originalData)
    {
        Id                      = originalData.Id;

        PhaseId                 = originalData.PhaseId;
        QuestId                 = originalData.QuestId;
        ObjectiveId             = originalData.ObjectiveId;

        ChapterInteractableId   = originalData.ChapterInteractableId;
        InteractableId          = originalData.InteractableId;

        Index                   = originalData.Index;

        Type                    = originalData.Type;
    }

    public virtual void Clone(WorldInteractableData data)
    {
        data.Id                     = Id;

        data.PhaseId                = PhaseId;
        data.QuestId                = QuestId;
        data.ObjectiveId            = ObjectiveId;

        data.ChapterInteractableId  = ChapterInteractableId;
        data.InteractableId         = InteractableId;

        data.Index                  = Index;

        data.Type                   = Type;
    }
}
