using UnityEngine;

public class QuestBaseData
{
    public int Id               { get; set; }
    
    public int PhaseId          { get; set; }

    public int Index            { get; set; }

    public string Name          { get; set; }

    public string EditorNotes   { get; set; }
    public string GameNotes     { get; set; }

    public virtual void GetOriginalValues(QuestData originalData)
    {
        Id              = originalData.Id;

        PhaseId         = originalData.PhaseId;

        Index           = originalData.Index;

        Name            = originalData.Name;

        EditorNotes     = originalData.EditorNotes;
        GameNotes       = originalData.GameNotes;
}

    public virtual void Clone(QuestData data)
    {
        data.Id             = Id;

        data.PhaseId        = PhaseId;

        data.Index          = Index;

        data.Name           = Name;

        data.EditorNotes    = EditorNotes;
        data.GameNotes      = GameNotes;
    }
}
