using UnityEngine;

public class TaskBaseData
{
    public int Id                   { get; set; }
    
    public int WorldInteractableId  { get; set; }
    public int ObjectiveId          { get; set; }

    public bool Default             { get; set; }

    public int Index                { get; set; }

    public string Name              { get; set; }

    public bool CompleteObjective   { get; set; }
    public bool Repeatable          { get; set; }

    public string PublicNotes       { get; set; }
    public string PrivateNotes      { get; set; }

    public virtual void GetOriginalValues(TaskData originalData)
    {
        Id                  = originalData.Id;

        Default             = originalData.Default;

        WorldInteractableId = originalData.WorldInteractableId;
        ObjectiveId         = originalData.ObjectiveId;

        Index               = originalData.Index;

        Name                = originalData.Name;

        CompleteObjective   = originalData.CompleteObjective;
        Repeatable          = originalData.Repeatable;

        PublicNotes         = originalData.PublicNotes;
        PrivateNotes        = originalData.PrivateNotes;
    }

    public virtual void Clone(TaskData data)
    {
        data.Id                     = Id;

        data.Default                = Default;

        data.WorldInteractableId    = WorldInteractableId;
        data.ObjectiveId            = ObjectiveId;

        data.Index                  = Index;

        data.Name                   = Name;

        data.CompleteObjective      = CompleteObjective;
        data.Repeatable             = Repeatable;

        data.PublicNotes            = PublicNotes;
        data.PrivateNotes           = PrivateNotes;
    }
}
