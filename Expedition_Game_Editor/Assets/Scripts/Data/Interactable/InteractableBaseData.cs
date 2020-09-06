using UnityEngine;

public class InteractableBaseData 
{
    public int Id           { get; set; }
    
    public int ModelId      { get; set; }

    public int Type         { get; set; }

    public int Index        { get; set; }

    public string Name      { get; set; }

    public float Scale      { get; set; }

    public int Health       { get; set; }
    public int Hunger       { get; set; }
    public int Thirst       { get; set; }

    public float Weight     { get; set; }
    public float Speed      { get; set; }
    public float Stamina    { get; set; }

    public virtual void GetOriginalValues(InteractableData originalData)
    {
        Id      = originalData.Id;

        ModelId = originalData.ModelId;

        Type    = originalData.Type;

        Index   = originalData.Index;

        Name    = originalData.Name;

        Scale   = originalData.Scale;

        Health  = originalData.Health;
        Hunger  = originalData.Hunger;
        Thirst  = originalData.Thirst;

        Weight  = originalData.Weight;
        Speed   = originalData.Speed;
        Stamina = originalData.Stamina;
    }

    public virtual void Clone(InteractableData data)
    {
        data.Id         = Id;

        data.ModelId    = ModelId;

        data.Type       = Type;

        data.Index      = Index;

        data.Name       = Name;

        data.Scale      = Scale;

        data.Health     = Health;
        data.Hunger     = Hunger;
        data.Thirst     = Thirst;

        data.Weight     = Weight;
        data.Speed      = Speed;
        data.Stamina    = Stamina;
    }
}
