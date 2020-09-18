using UnityEngine;

public class QuestSaveData : QuestSaveBaseData
{
    public int Index            { get; set; }

    public string Name          { get; set; }

    public string PublicNotes   { get; set; }
    public string PrivateNotes  { get; set; }

    public override void GetOriginalValues(QuestSaveData originalData)
    {
        Index           = originalData.Index;

        Name            = originalData.Name;

        PublicNotes     = originalData.PublicNotes;
        PrivateNotes    = originalData.PrivateNotes;

        base.GetOriginalValues(originalData);
    }

    public QuestSaveData Clone()
    {
        var data = new QuestSaveData();

        data.Index          = Index;

        data.Name           = Name;

        data.PublicNotes    = PublicNotes;
        data.PrivateNotes   = PrivateNotes;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(QuestSaveElementData elementData)
    {
        elementData.Index           = Index;

        elementData.Name            = Name;

        elementData.PublicNotes     = PublicNotes;
        elementData.PrivateNotes    = PrivateNotes;

        base.Clone(elementData);
    }
}
