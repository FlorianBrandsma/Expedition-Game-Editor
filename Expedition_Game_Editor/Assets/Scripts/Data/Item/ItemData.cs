using UnityEngine;

public class ItemData : ItemBaseData
{
    public string ModelPath     { get; set; }
    public string ModelIconPath { get; set; }

    public float Height         { get; set; }
    public float Width          { get; set; }
    public float Depth          { get; set; }

    public override void GetOriginalValues(ItemData originalData)
    {
        ModelPath       = originalData.ModelPath;
        ModelIconPath   = originalData.ModelIconPath;

        Height          = originalData.Height;
        Width           = originalData.Width;
        Depth           = originalData.Depth;

        base.GetOriginalValues(originalData);
    }

    public ItemData Clone()
    {
        var data = new ItemData();

        data.ModelPath      = ModelPath;
        data.ModelIconPath  = ModelIconPath;

        data.Height         = Height;
        data.Width          = Width;
        data.Depth          = Depth;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(ItemElementData elementData)
    {
        elementData.ModelPath       = ModelPath;
        elementData.ModelIconPath   = ModelIconPath;

        elementData.Height          = Height;
        elementData.Width           = Width;
        elementData.Depth           = Depth;

        base.Clone(elementData);
    }
}
