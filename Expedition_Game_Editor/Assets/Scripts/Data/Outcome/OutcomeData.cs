using UnityEngine;

public class OutcomeData : OutcomeBaseData
{
    public string ModelIconPath     { get; set; }

    public bool DefaultInteraction  { get; set; }

    public int InteractionStartTime { get; set; }
    public int InteractionEndTime   { get; set; }

    public string TaskName          { get; set; }

    public override void GetOriginalValues(OutcomeData originalData)
    {
        ModelIconPath           = originalData.ModelIconPath;

        DefaultInteraction      = originalData.DefaultInteraction;

        InteractionStartTime    = originalData.InteractionStartTime;
        InteractionEndTime      = originalData.InteractionEndTime;

        TaskName                = originalData.TaskName;

        base.GetOriginalValues(originalData);
    }

    public OutcomeData Clone()
    {
        var data = new OutcomeData();

        data.ModelIconPath          = ModelIconPath;

        data.DefaultInteraction     = DefaultInteraction;

        data.InteractionStartTime   = InteractionStartTime;
        data.InteractionEndTime     = InteractionEndTime;

        data.TaskName               = TaskName;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(OutcomeElementData elementData)
    {
        elementData.ModelIconPath           = ModelIconPath;

        elementData.DefaultInteraction      = DefaultInteraction;

        elementData.InteractionStartTime    = InteractionStartTime;
        elementData.InteractionEndTime      = InteractionEndTime;

        elementData.TaskName                = TaskName;

        base.Clone(elementData);
    }
}
