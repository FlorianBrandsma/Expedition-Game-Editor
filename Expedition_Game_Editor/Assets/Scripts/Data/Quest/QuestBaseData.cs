using UnityEngine;

public class QuestBaseData
{
    public int Id               { get; set; }
    
    public int PhaseId          { get; set; }

    public int Index            { get; set; }

    public string Name          { get; set; }

    public string PublicNotes   { get; set; }
    public string PrivateNotes  { get; set; }

    public virtual void GetOriginalValues(QuestData originalData)
    {
        Id              = originalData.Id;

        PhaseId         = originalData.PhaseId;

        Index           = originalData.Index;

        Name            = originalData.Name;

        PublicNotes     = originalData.PublicNotes;
        PrivateNotes    = originalData.PrivateNotes;
}

    public virtual void Clone(QuestData data)
    {
        data.Id             = Id;

        data.PhaseId        = PhaseId;

        data.Index          = Index;

        data.Name           = Name;

        data.PublicNotes    = PublicNotes;
        data.PrivateNotes   = PrivateNotes;
    }
}
