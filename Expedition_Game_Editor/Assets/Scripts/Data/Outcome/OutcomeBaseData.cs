using UnityEngine;

public class OutcomeBaseData
{
    public int Id               { get; set; }
    
    public int InteractionId    { get; set; }

    public int Type             { get; set; }

    public bool CompleteTask    { get; set; }
    public bool ResetObjective  { get; set; }

    public string PublicNotes   { get; set; }
    public string PrivateNotes  { get; set; }

    public virtual void GetOriginalValues(OutcomeData originalData)
    {
        Id              = originalData.Id;

        InteractionId   = originalData.InteractionId;

        Type            = originalData.Type;

        CompleteTask    = originalData.CompleteTask;
        ResetObjective  = originalData.ResetObjective;

        PublicNotes     = originalData.PublicNotes;
        PrivateNotes    = originalData.PrivateNotes;
    }

    public virtual void Clone(OutcomeData data)
    {
        data.Id             = Id;

        data.InteractionId  = InteractionId;

        data.Type           = Type;

        data.CompleteTask   = CompleteTask;
        data.ResetObjective = ResetObjective;

        data.PublicNotes    = PublicNotes;
        data.PrivateNotes   = PrivateNotes;
    }
}
