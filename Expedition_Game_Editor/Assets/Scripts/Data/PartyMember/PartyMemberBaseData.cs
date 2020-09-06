using UnityEngine;

public class PartyMemberBaseData
{
    public int Id               { get; set; }

    public int ChapterId        { get; set; }
    public int InteractableId   { get; set; }

    public virtual void GetOriginalValues(PartyMemberData originalData)
    {
        Id = originalData.Id;

        ChapterId = originalData.ChapterId;
        InteractableId = originalData.InteractableId;
    }

    public virtual void Clone(PartyMemberData data)
    {
        data.Id = Id;

        data.ChapterId = ChapterId;
        data.InteractableId = InteractableId;
    }
}
