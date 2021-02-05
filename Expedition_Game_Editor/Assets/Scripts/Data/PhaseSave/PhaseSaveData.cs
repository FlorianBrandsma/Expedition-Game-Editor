using UnityEngine;

public class PhaseSaveData : PhaseSaveBaseData
{
    public int ChapterId        { get; set; }

    public int Index            { get; set; }

    public string Name          { get; set; }

    public string EditorNotes   { get; set; }
    public string GameNotes     { get; set; }

    public override void GetOriginalValues(PhaseSaveData originalData)
    {
        ChapterId       = originalData.ChapterId;

        Index           = originalData.Index;

        Name            = originalData.Name;

        EditorNotes     = originalData.EditorNotes;
        GameNotes       = originalData.GameNotes;

        base.GetOriginalValues(originalData);
    }

    public PhaseSaveData Clone()
    {
        var data = new PhaseSaveData();

        data.ChapterId      = ChapterId;

        data.Index          = Index;

        data.Name           = Name;

        data.EditorNotes    = EditorNotes;
        data.GameNotes      = GameNotes;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(PhaseSaveElementData elementData)
    {
        elementData.ChapterId       = ChapterId;

        elementData.Index           = Index;

        elementData.Name            = Name;

        elementData.EditorNotes     = EditorNotes;
        elementData.GameNotes       = GameNotes;

        base.Clone(elementData);
    }
}
