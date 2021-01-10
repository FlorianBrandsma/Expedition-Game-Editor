using UnityEngine;

public class ObjectiveSaveData : ObjectiveSaveBaseData
{
    public int QuestId          { get; set; }

    public int Index            { get; set; }

    public string Name          { get; set; }

    public string PublicNotes   { get; set; }
    public string PrivateNotes  { get; set; }

    public override void GetOriginalValues(ObjectiveSaveData originalData)
    {
        QuestId         = originalData.QuestId;

        Index           = originalData.Index;

        Name            = originalData.Name;

        PublicNotes     = originalData.PublicNotes;
        PrivateNotes    = originalData.PrivateNotes;

        base.GetOriginalValues(originalData);
    }

    public ObjectiveSaveData Clone()
    {
        var data = new ObjectiveSaveData();

        data.QuestId        = QuestId;

        data.Index          = Index;

        data.Name           = Name;

        data.PublicNotes    = PublicNotes;
        data.PrivateNotes   = PrivateNotes;

        base.Clone(data);

        return data;
    }

    public virtual void Clone(ObjectiveSaveElementData elementData)
    {
        elementData.QuestId         = QuestId;

        elementData.Index           = Index;

        elementData.Name            = Name;

        elementData.PublicNotes     = PublicNotes;
        elementData.PrivateNotes    = PrivateNotes;

        base.Clone(elementData);
    }
}
