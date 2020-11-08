using UnityEngine;

public class SceneShotData : SceneShotBaseData
{
    public override void GetOriginalValues(SceneShotData originalData)
    {
        base.GetOriginalValues(originalData);
    }

    public SceneShotData Clone()
    {
        var data = new SceneShotData();

        base.Clone(data);

        return data;
    }

    public virtual void Clone(SceneShotElementData elementData)
    {
        base.Clone(elementData);
    }
}