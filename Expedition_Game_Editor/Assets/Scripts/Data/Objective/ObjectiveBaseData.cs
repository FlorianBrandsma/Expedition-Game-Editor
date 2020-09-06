using UnityEngine;

public class ObjectiveBaseData
{
    public int Id               { get; set; }
    
    public int QuestId          { get; set; }

    public int Index            { get; set; }

    public string Name          { get; set; }
    public string Journal       { get; set; }

    public string PublicNotes   { get; set; }
    public string PrivateNotes  { get; set; }

    public virtual void GetOriginalValues(ObjectiveData originalData)
    {
        Id              = originalData.Id;

        QuestId         = originalData.QuestId;

        Index           = originalData.Index;

        Name            = originalData.Name;
        Journal         = originalData.Journal;

        PublicNotes     = originalData.PublicNotes;
        PrivateNotes    = originalData.PrivateNotes;
    }

    public virtual void Clone(ObjectiveData data)
    {
        data.Id             = Id;

        data.QuestId        = QuestId;

        data.Index          = Index;

        data.Name           = Name;
        data.Journal        = Journal;

        data.PublicNotes    = PublicNotes;
        data.PrivateNotes   = PrivateNotes;
    }
}
