using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ObjectiveDataElement : ObjectiveCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public ObjectiveDataElement() : base() { }

    public override void Update()
    {
        if (!Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        if (!Changed) return;

        base.SetOriginalValues();

        ClearChanges();
    }

    public new void GetOriginalValues() { }

    public override void ClearChanges()
    {
        base.ClearChanges();

        GetOriginalValues();
    }
}
