using UnityEngine;

public class PhaseSaveData : PhaseSaveBaseData
{
    public string Name          { get; set; }

    public string PublicNotes   { get; set; }
    public string PrivateNotes  { get; set; }

    public override void GetOriginalValues(PhaseSaveData originalData)
    {
        Name            = originalData.Name;

        PublicNotes     = originalData.PublicNotes;
        PrivateNotes    = originalData.PrivateNotes;

        base.GetOriginalValues(originalData);
    }

    public PhaseSaveData Clone()
    {
        var data = new PhaseSaveData();
        
        data.Name           = Name;

        data.PublicNotes    = PublicNotes;
        data.PrivateNotes   = PrivateNotes;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(PhaseSaveElementData elementData)
    {
        elementData.Name            = Name;

        elementData.PublicNotes     = PublicNotes;
        elementData.PrivateNotes    = PrivateNotes;

        base.Clone(elementData);
    }
}
