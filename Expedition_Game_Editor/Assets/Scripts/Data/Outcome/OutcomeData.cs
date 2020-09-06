using UnityEngine;

public class OutcomeData : OutcomeBaseData
{
    public override void GetOriginalValues(OutcomeData originalData)
    {
        base.GetOriginalValues(originalData);
    }

    public OutcomeData Clone()
    {
        var data = new OutcomeData();
        
        base.Clone(data);

        return data;
    }

    public virtual void Clone(OutcomeElementData elementData)
    {
        base.Clone(elementData);
    }
}
