using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class OutcomeDataElement : OutcomeCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public OutcomeDataElement() : base()
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

    public IDataElement Clone()
    {
        var dataElement = new OutcomeDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        SetOriginalValues();
    }
}
