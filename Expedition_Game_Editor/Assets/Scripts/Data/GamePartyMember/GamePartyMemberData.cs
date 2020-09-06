using UnityEngine;

public class GamePartyMemberData
{
    public int Id                   { get; set; }

    public int ModelId              { get; set; }

    public string ModelPath         { get; set; }
    public string ModelIconPath     { get; set; }

    public string InteractableName  { get; set; }

    public float Scale              { get; set; }

    public int Health               { get; set; }
    public int Hunger               { get; set; }
    public int Thirst               { get; set; }

    public float Weight             { get; set; }
    public float Speed              { get; set; }
    public float Stamina            { get; set; }

    public virtual void GetOriginalValues(GamePartyMemberData originalData)
    {
        Id                  = originalData.Id;

        ModelId             = originalData.ModelId;

        ModelPath           = originalData.ModelPath;
        ModelIconPath       = originalData.ModelIconPath;

        InteractableName    = originalData.InteractableName;

        Scale               = originalData.Scale;

        Health              = originalData.Health;
        Hunger              = originalData.Hunger;
        Thirst              = originalData.Thirst;

        Weight              = originalData.Weight;
        Speed               = originalData.Speed;
        Stamina             = originalData.Stamina;
    }

    public GamePartyMemberData Clone()
    {
        var data = new GamePartyMemberData();
        
        data.Id                 = Id;

        data.ModelId            = ModelId;

        data.ModelPath          = ModelPath;
        data.ModelIconPath      = ModelIconPath;

        data.InteractableName   = InteractableName;

        data.Scale              = Scale;

        data.Health             = Health;
        data.Hunger             = Hunger;
        data.Thirst             = Thirst;

        data.Weight             = Weight;
        data.Speed              = Speed;
        data.Stamina            = Stamina;

        return data;
    }

    public virtual void Clone(GamePartyMemberElementData elementData)
    {
        elementData.Id                  = Id;

        elementData.ModelId             = ModelId;

        elementData.ModelPath           = ModelPath;
        elementData.ModelIconPath       = ModelIconPath;

        elementData.InteractableName    = InteractableName;

        elementData.Scale               = Scale;

        elementData.Health              = Health;
        elementData.Hunger              = Hunger;
        elementData.Thirst              = Thirst;

        elementData.Weight              = Weight;
        elementData.Speed               = Speed;
        elementData.Stamina             = Stamina;
    }
}
