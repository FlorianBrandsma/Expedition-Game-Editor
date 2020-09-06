using UnityEngine;

public class ModelData : ModelBaseData
{
    public int Category     { get; set; }

    public string IconPath  { get; set; }

    public override void GetOriginalValues(ModelData originalData)
    {
        Category = originalData.Category;

        IconPath = originalData.IconPath;

        base.GetOriginalValues(originalData);
    }

    public ModelData Clone()
    {
        var data = new ModelData();
        
        data.Category = Category;

        data.IconPath = IconPath;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(ModelElementData elementData)
    {
        elementData.Category = Category;

        elementData.IconPath = IconPath;

        base.Clone(elementData);
    }
}
