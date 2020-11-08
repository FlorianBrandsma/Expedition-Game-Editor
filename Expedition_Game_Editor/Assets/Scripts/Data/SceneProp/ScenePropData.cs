using UnityEngine;

public class ScenePropData : ScenePropBaseData
{
    public string ModelName     { get; set; }
    public string ModelIconPath { get; set; }

    public override void GetOriginalValues(ScenePropData originalData)
    {
        ModelName = originalData.ModelName;
        ModelIconPath = originalData.ModelIconPath;

        base.GetOriginalValues(originalData);
    }

    public ScenePropData Clone()
    {
        var data = new ScenePropData();

        data.ModelName = ModelName;
        data.ModelIconPath = ModelIconPath;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(ScenePropElementData elementData)
    {
        elementData.ModelName = ModelName;
        elementData.ModelIconPath = ModelIconPath;

        base.Clone(elementData);
    }
}