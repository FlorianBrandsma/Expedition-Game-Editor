﻿public class InteractionSaveElementData : InteractionSaveCore, IElementData
{
    public DataElement DataElement { get; set; }

    public InteractionSaveElementData() : base()
    {
        DataType = Enums.DataType.InteractionSave;
    }

    public bool isDefault;

    public int startTime;
    public int endTime;

    public string publicNotes;
    public string privateNotes;

    public override void Update()
    {
        if (!Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        ClearChanges();
    }

    public new void GetOriginalValues() { }

    public override void ClearChanges()
    {
        if (!Changed) return;

        base.ClearChanges();

        GetOriginalValues();
    }

    public IElementData Clone()
    {
        var elementData = new InteractionSaveElementData();

        CloneGeneralData(elementData);

        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        SetOriginalValues();
    }
}