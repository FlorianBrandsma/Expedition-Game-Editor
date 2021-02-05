using UnityEngine;

public class ObjectiveSaveData : ObjectiveSaveBaseData
{
    public int QuestId          { get; set; }

    public int Index            { get; set; }

    public string Name          { get; set; }

    public string EditorNotes   { get; set; }
    public string GameNotes     { get; set; }

    public override void GetOriginalValues(ObjectiveSaveData originalData)
    {
        QuestId         = originalData.QuestId;

        Index           = originalData.Index;

        Name            = originalData.Name;

        EditorNotes     = originalData.EditorNotes;
        GameNotes       = originalData.GameNotes;

        base.GetOriginalValues(originalData);
    }

    public ObjectiveSaveData Clone()
    {
        var data = new ObjectiveSaveData();

        data.QuestId        = QuestId;

        data.Index          = Index;

        data.Name           = Name;

        data.EditorNotes    = EditorNotes;
        data.GameNotes      = GameNotes;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(ObjectiveSaveElementData elementData)
    {
        elementData.QuestId         = QuestId;

        elementData.Index           = Index;

        elementData.Name            = Name;

        elementData.EditorNotes     = EditorNotes;
        elementData.GameNotes    = GameNotes;

        base.Clone(elementData);
    }
}
