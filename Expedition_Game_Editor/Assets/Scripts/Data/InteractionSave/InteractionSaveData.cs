using UnityEngine;

public class InteractionSaveData : InteractionSaveBaseData
{
    public bool Default         { get; set; }

    public int StartTime        { get; set; }
    public int EndTime          { get; set; }

    public string PublicNotes   { get; set; }
    public string PrivateNotes  { get; set; }

    public override void GetOriginalValues(InteractionSaveData originalData)
    {
        Default         = originalData.Default;

        StartTime       = originalData.StartTime;
        EndTime         = originalData.EndTime;

        PublicNotes     = originalData.PublicNotes;
        PrivateNotes    = originalData.PrivateNotes;

        base.GetOriginalValues(originalData);
    }

    public InteractionSaveData Clone()
    {
        var data = new InteractionSaveData();
        
        data.Default        = Default;

        data.StartTime      = StartTime;
        data.EndTime        = EndTime;

        data.PublicNotes    = PublicNotes;
        data.PrivateNotes   = PrivateNotes;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(InteractionSaveElementData elementData)
    {
        elementData.Default         = Default;

        elementData.StartTime       = StartTime;
        elementData.EndTime         = EndTime;

        elementData.PublicNotes     = PublicNotes;
        elementData.PrivateNotes    = PrivateNotes;

        base.Clone(elementData);
    }
}
