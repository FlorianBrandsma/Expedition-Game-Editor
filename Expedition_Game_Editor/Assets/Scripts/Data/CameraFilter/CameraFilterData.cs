using UnityEngine;

public class CameraFilterData : CameraFilterBaseData
{
    public override void GetOriginalValues(CameraFilterData originalData)
    {
        base.GetOriginalValues(originalData);
    }

    public CameraFilterData Clone()
    {
        var data = new CameraFilterData();

        base.Clone(data);

        return data;
    }

    public virtual void Clone(CameraFilterElementData elementData)
    {
        base.Clone(elementData);
    }
}
