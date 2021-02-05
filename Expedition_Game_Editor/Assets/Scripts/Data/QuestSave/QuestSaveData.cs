using UnityEngine;

public class QuestSaveData : QuestSaveBaseData
{
    public int PhaseId          { get; set; }

    public int Index            { get; set; }

    public string Name          { get; set; }

    public string EditorNotes   { get; set; }
    public string GameNotes     { get; set; }

    public override void GetOriginalValues(QuestSaveData originalData)
    {
        PhaseId         = originalData.PhaseId;

        Index           = originalData.Index;

        Name            = originalData.Name;

        EditorNotes     = originalData.EditorNotes;
        GameNotes       = originalData.GameNotes;

        base.GetOriginalValues(originalData);
    }

    public QuestSaveData Clone()
    {
        var data = new QuestSaveData();

        data.PhaseId        = PhaseId;

        data.Index          = Index;

        data.Name           = Name;

        data.EditorNotes    = EditorNotes;
        data.GameNotes      = GameNotes;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(QuestSaveElementData elementData)
    {
        elementData.PhaseId         = PhaseId;

        elementData.Index           = Index;

        elementData.Name            = Name;

        elementData.EditorNotes     = EditorNotes;
        elementData.GameNotes       = GameNotes;

        base.Clone(elementData);
    }
}
