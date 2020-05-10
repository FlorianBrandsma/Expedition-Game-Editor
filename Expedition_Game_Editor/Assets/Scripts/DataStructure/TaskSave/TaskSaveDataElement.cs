using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TaskSaveDataElement : TaskSaveCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public TaskSaveDataElement() : base()
    {
        DataType = Enums.DataType.TaskSave;
    }

    public string name;

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
        var dataElement = new TaskDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        SetOriginalValues();
    }
}
