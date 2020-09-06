using UnityEngine;

public class InteractableSaveData : InteractableSaveBaseData
{
    public int ModelId              { get; set; }

    public string InteractableName  { get; set; }

    public int Health               { get; set; }
    public int Hunger               { get; set; }
    public int Thirst               { get; set; }

    public float Weight             { get; set; }
    public float Speed              { get; set; }
    public float Stamina            { get; set; }

    public string ModelPath         { get; set; }
    public string ModelIconPath     { get; set; }

    public override void GetOriginalValues(InteractableSaveData originalData)
    {
        ModelId             = originalData.ModelId;

        InteractableName    = originalData.InteractableName;

        Health              = originalData.Health;
        Hunger              = originalData.Hunger;
        Thirst              = originalData.Thirst;

        Weight              = originalData.Weight;
        Speed               = originalData.Speed;
        Stamina             = originalData.Stamina;

        ModelPath           = originalData.ModelPath;
        ModelIconPath       = originalData.ModelIconPath;

        base.GetOriginalValues(originalData);
    }

    public InteractableSaveData Clone()
    {
        var data = new InteractableSaveData();
        
        data.ModelId            = ModelId;

        data.InteractableName   = InteractableName;

        data.Health             = Health;
        data.Hunger             = Hunger;
        data.Thirst             = Thirst;

        data.Weight             = Weight;
        data.Speed              = Speed;
        data.Stamina            = Stamina;

        data.ModelPath          = ModelPath;
        data.ModelIconPath      = ModelIconPath;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(InteractableSaveElementData elementData)
    {
        elementData.ModelId             = ModelId;

        elementData.InteractableName    = InteractableName;

        elementData.Health              = Health;
        elementData.Hunger              = Hunger;
        elementData.Thirst              = Thirst;

        elementData.Weight              = Weight;
        elementData.Speed               = Speed;
        elementData.Stamina             = Stamina;

        elementData.ModelPath           = ModelPath;
        elementData.ModelIconPath       = ModelIconPath;

        base.Clone(elementData);
    }
}
