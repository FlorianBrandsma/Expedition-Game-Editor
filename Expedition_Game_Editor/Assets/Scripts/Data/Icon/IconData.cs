using UnityEngine;

public class IconData : IconBaseData
{
    public string BaseIconPath { get; set; }

    public override void GetOriginalValues(IconData originalData)
    {
        BaseIconPath = originalData.BaseIconPath;

        base.GetOriginalValues(originalData);
    }

    public IconData Clone()
    {
        var data = new IconData();
        
        data.BaseIconPath = BaseIconPath;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(IconElementData elementData)
    {
        elementData.BaseIconPath = BaseIconPath;

        base.Clone(elementData);
    }
}
