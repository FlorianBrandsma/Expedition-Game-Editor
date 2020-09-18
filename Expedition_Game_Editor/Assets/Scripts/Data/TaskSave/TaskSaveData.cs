using UnityEngine;

public class TaskSaveData : TaskSaveBaseData
{
    public int Index            { get; set; }

    public string Name          { get; set; }

    public bool Repeatable      { get; set; }

    public string PublicNotes   { get; set; }
    public string PrivateNotes  { get; set; }

    public override void GetOriginalValues(TaskSaveData originalData)
    {
        Index           = originalData.Index;

        Name            = originalData.Name;

        Repeatable      = originalData.Repeatable;

        PublicNotes     = originalData.PublicNotes;
        PrivateNotes    = originalData.PrivateNotes;

        base.GetOriginalValues(originalData);
    }

    public TaskSaveData Clone()
    {
        var data = new TaskSaveData();

        data.Index          = Index;

        data.Name           = Name;

        data.Repeatable     = Repeatable;

        data.PublicNotes    = PublicNotes;
        data.PrivateNotes   = PrivateNotes;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(TaskSaveElementData elementData)
    {
        elementData.Index           = Index;

        elementData.Name            = Name;

        elementData.Repeatable      = Repeatable;

        elementData.PublicNotes     = PublicNotes;
        elementData.PrivateNotes    = PrivateNotes;

        base.Clone(elementData);
    }
}
