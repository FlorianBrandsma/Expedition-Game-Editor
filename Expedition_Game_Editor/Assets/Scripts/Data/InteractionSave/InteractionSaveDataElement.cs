using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InteractionSaveDataElement : InteractionSaveCore, IDataElement
{
    public DataElement DataElement { get; set; }

    public InteractionSaveDataElement() : base()
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

    public IDataElement Clone()
    {
        var dataElement = new InteractionDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        SetOriginalValues();
    }
}
