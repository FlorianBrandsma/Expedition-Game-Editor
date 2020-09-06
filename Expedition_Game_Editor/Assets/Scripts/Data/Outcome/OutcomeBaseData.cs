using UnityEngine;

public class OutcomeBaseData
{
    public int Id               { get; set; }
    
    public int InteractionId    { get; set; }

    public int Type             { get; set; }

    public virtual void GetOriginalValues(OutcomeData originalData)
    {
        Id              = originalData.Id;

        InteractionId   = originalData.InteractionId;

        Type            = originalData.Type;
    }

    public virtual void Clone(OutcomeData data)
    {
        data.Id             = Id;

        data.InteractionId  = InteractionId;

        data.Type           = Type;
    }
}
