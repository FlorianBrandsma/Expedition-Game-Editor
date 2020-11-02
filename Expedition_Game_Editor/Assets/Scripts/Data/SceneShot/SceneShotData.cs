using UnityEngine;

public class SceneShotData : SceneShotBaseData
{
    public string Description { get; set; }
    
    public override void GetOriginalValues(SceneShotData originalData)
    {
        Description = originalData.Description;

        base.GetOriginalValues(originalData);
    }

    public SceneShotData Clone()
    {
        var data = new SceneShotData();

        data.Description = Description;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(SceneShotElementData elementData)
    {
        elementData.Description = Description;

        base.Clone(elementData);
    }
}