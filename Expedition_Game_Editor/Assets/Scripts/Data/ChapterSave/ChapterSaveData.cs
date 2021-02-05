using UnityEngine;

public class ChapterSaveData : ChapterSaveBaseData
{
    public int Index            { get; set; }

    public string Name          { get; set; }

    public string EditorNotes   { get; set; }
    public string GameNotes     { get; set; }

    public override void GetOriginalValues(ChapterSaveData originalData)
    {
        Index           = originalData.Index;

        Name            = originalData.Name;

        EditorNotes     = originalData.EditorNotes;
        GameNotes       = originalData.GameNotes;

        base.GetOriginalValues(originalData);
    }

    public ChapterSaveData Clone()
    {
        var data = new ChapterSaveData();

        data.Index          = Index;

        data.Name           = Name;

        data.EditorNotes    = EditorNotes;
        data.GameNotes      = GameNotes;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(ChapterSaveElementData elementData)
    {
        elementData.Index           = Index;

        elementData.Name            = Name;

        elementData.EditorNotes     = EditorNotes;
        elementData.GameNotes       = GameNotes;

        base.Clone(elementData);
    }
}
