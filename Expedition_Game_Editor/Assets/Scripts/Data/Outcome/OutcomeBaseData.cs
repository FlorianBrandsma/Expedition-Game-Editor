using UnityEngine;

public class OutcomeBaseData
{
    public int Id                           { get; set; }
    
    public int InteractionId                { get; set; }

    public int Type                         { get; set; }

    public bool CompleteTask                { get; set; }
    public bool ResetObjective              { get; set; }

    public int CancelScenarioType           { get; set; }
    public bool CancelScenarioOnInput       { get; set; }
    public bool CancelScenarioOnInteraction { get; set; }
    public bool CancelScenarioOnRange       { get; set; }
    public bool CancelScenarioOnHit         { get; set; }

    public string PublicNotes               { get; set; }
    public string PrivateNotes              { get; set; }

    public virtual void GetOriginalValues(OutcomeData originalData)
    {
        Id                          = originalData.Id;

        InteractionId               = originalData.InteractionId;

        Type                        = originalData.Type;

        CompleteTask                = originalData.CompleteTask;
        ResetObjective              = originalData.ResetObjective;

        CancelScenarioType          = originalData.CancelScenarioType;
        CancelScenarioOnInteraction = originalData.CancelScenarioOnInteraction;
        CancelScenarioOnInput       = originalData.CancelScenarioOnInput;
        CancelScenarioOnRange       = originalData.CancelScenarioOnRange;
        CancelScenarioOnHit         = originalData.CancelScenarioOnHit;

        PublicNotes                 = originalData.PublicNotes;
        PrivateNotes                = originalData.PrivateNotes;
    }

    public virtual void Clone(OutcomeData data)
    {
        data.Id                             = Id;

        data.InteractionId                  = InteractionId;

        data.Type                           = Type;

        data.CompleteTask                   = CompleteTask;
        data.ResetObjective                 = ResetObjective;

        data.CancelScenarioType             = CancelScenarioType;
        data.CancelScenarioOnInteraction    = CancelScenarioOnInteraction;
        data.CancelScenarioOnInput          = CancelScenarioOnInput;
        data.CancelScenarioOnRange          = CancelScenarioOnRange;
        data.CancelScenarioOnHit            = CancelScenarioOnHit;

        data.PublicNotes                    = PublicNotes;
        data.PrivateNotes                   = PrivateNotes;
    }
}
