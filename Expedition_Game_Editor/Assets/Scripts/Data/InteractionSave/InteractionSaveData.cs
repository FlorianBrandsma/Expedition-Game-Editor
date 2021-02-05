using UnityEngine;

public class InteractionSaveData : InteractionSaveBaseData
{
    public int TaskId           { get; set; }

    public bool Default         { get; set; }

    public int StartTime        { get; set; }
    public int EndTime          { get; set; }

    public string EditorNotes   { get; set; }
    public string GameNotes     { get; set; }

    public override void GetOriginalValues(InteractionSaveData originalData)
    {
        TaskId          = originalData.TaskId;

        Default         = originalData.Default;

        StartTime       = originalData.StartTime;
        EndTime         = originalData.EndTime;

        EditorNotes     = originalData.EditorNotes;
        GameNotes       = originalData.GameNotes;

        base.GetOriginalValues(originalData);
    }

    public InteractionSaveData Clone()
    {
        var data = new InteractionSaveData();
        
        data.TaskId         = TaskId;

        data.Default        = Default;

        data.StartTime      = StartTime;
        data.EndTime        = EndTime;

        data.EditorNotes    = EditorNotes;
        data.GameNotes      = GameNotes;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(InteractionSaveElementData elementData)
    {
        elementData.TaskId          = TaskId;

        elementData.Default         = Default;

        elementData.StartTime       = StartTime;
        elementData.EndTime         = EndTime;

        elementData.EditorNotes     = EditorNotes;
        elementData.GameNotes       = GameNotes;

        base.Clone(elementData);
    }
}
