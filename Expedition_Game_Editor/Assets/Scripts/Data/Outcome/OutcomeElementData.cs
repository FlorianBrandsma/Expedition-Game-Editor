using UnityEngine;

public class OutcomeElementData : OutcomeCore, IElementData
{
    public DataElement DataElement { get; set; }

    public OutcomeElementData() : base()
    {
        DataType = Enums.DataType.Outcome;
    }

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
        var elementData = new OutcomeElementData();

        CloneGeneralData(elementData);

        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        SetOriginalValues();
    }
}
