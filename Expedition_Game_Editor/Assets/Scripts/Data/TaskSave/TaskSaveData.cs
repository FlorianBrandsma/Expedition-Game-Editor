using UnityEngine;

public class TaskSaveData : TaskSaveBaseData
{
    public int ObjectiveId      { get; set; }

    public int Index            { get; set; }

    public string Name          { get; set; }

    public bool Repeatable      { get; set; }

    public string EditorNotes   { get; set; }
    public string GameNotes     { get; set; }

    public override void GetOriginalValues(TaskSaveData originalData)
    {
        ObjectiveId     = originalData.ObjectiveId;

        Index           = originalData.Index;

        Name            = originalData.Name;

        Repeatable      = originalData.Repeatable;

        EditorNotes     = originalData.EditorNotes;
        GameNotes       = originalData.GameNotes;

        base.GetOriginalValues(originalData);
    }

    public TaskSaveData Clone()
    {
        var data = new TaskSaveData();

        data.ObjectiveId    = ObjectiveId;

        data.Index          = Index;

        data.Name           = Name;

        data.Repeatable     = Repeatable;

        data.EditorNotes    = EditorNotes;
        data.GameNotes      = GameNotes;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(TaskSaveElementData elementData)
    {
        elementData.ObjectiveId     = ObjectiveId;

        elementData.Index           = Index;

        elementData.Name            = Name;

        elementData.Repeatable      = Repeatable;

        elementData.EditorNotes     = EditorNotes;
        elementData.GameNotes       = GameNotes;

        base.Clone(elementData);
    }
}
