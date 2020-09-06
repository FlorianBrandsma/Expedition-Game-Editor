using UnityEngine;

public class TaskData : TaskBaseData
{
    public override void GetOriginalValues(TaskData originalData)
    {
        base.GetOriginalValues(originalData);
    }

    public TaskData Clone()
    {
        var data = new TaskData();

        base.Clone(data);

        return data;
    }

    public virtual void Clone(TaskElementData elementData)
    {
        base.Clone(elementData);
    }
}
