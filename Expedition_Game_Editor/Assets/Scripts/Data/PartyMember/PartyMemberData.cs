using UnityEngine;

public class PartyMemberData : PartyMemberBaseData
{
    public string InteractableName  { get; set; }
    public string ModelIconPath     { get; set; }

    public override void GetOriginalValues(PartyMemberData originalData)
    {
        InteractableName    = originalData.InteractableName;
        ModelIconPath       = originalData.ModelIconPath;

        base.GetOriginalValues(originalData);
    }

    public PartyMemberData Clone()
    {
        var data = new PartyMemberData();

        data.InteractableName   = InteractableName;
        data.ModelIconPath      = ModelIconPath;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(PartyMemberElementData elementData)
    {
        elementData.InteractableName = InteractableName;
        elementData.ModelIconPath    = ModelIconPath;

        base.Clone(elementData);
    }
}
