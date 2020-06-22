using UnityEngine;
using System.Collections.Generic;

public class ObjectiveDataElement : ObjectiveCore, IDataElement
{
    public DataElement DataElement { get; set; }

    public ObjectiveDataElement() : base()
    {
        DataType = Enums.DataType.Objective;
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
        var dataElement = new ObjectiveDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        SetOriginalValues();
    }
}
