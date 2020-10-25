using UnityEngine;

public class SceneData : SceneBaseData
{
    public string RegionName    { get; set; }

    public override void GetOriginalValues(SceneData originalData)
    {
        RegionId = originalData.RegionId;

        RegionName = originalData.RegionName;

        base.GetOriginalValues(originalData);
    }

    public SceneData Clone()
    {
        var data = new SceneData();

        data.RegionName = RegionName;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(SceneElementData elementData)
    {
        elementData.RegionName = RegionName;

        base.Clone(elementData);
    }
}
