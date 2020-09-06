using UnityEngine;

public class InteractableData : InteractableBaseData
{
    public string ModelPath     { get; set; }
    public string ModelIconPath { get; set; }

    public float Height         { get; set; }
    public float Width          { get; set; }
    public float Depth          { get; set; }

    public override void GetOriginalValues(InteractableData originalData)
    {
        ModelPath       = originalData.ModelPath;
        ModelIconPath   = originalData.ModelIconPath;

        Height          = originalData.Height;
        Width           = originalData.Width;
        Depth           = originalData.Depth;

        base.GetOriginalValues(originalData);
    }

    public InteractableData Clone()
    {
        var data = new InteractableData();

        data.ModelPath      = ModelPath;
        data.ModelIconPath  = ModelIconPath;

        data.Height         = Height;
        data.Width          = Width;
        data.Depth          = Depth;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(InteractableElementData elementData)
    {
        elementData.ModelPath       = ModelPath;
        elementData.ModelIconPath   = ModelIconPath;

        elementData.Height          = Height;
        elementData.Width           = Width;
        elementData.Depth           = Depth;

        base.Clone(elementData);
    }
}
