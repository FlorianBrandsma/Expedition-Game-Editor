using UnityEngine;
using System.Collections;

[System.Serializable]
public class PhaseDataElement : PhaseCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public PhaseDataElement() : base() { }

    public override void Create()
    {
        base.Create();
    }

    public override void Update()
    {
        if (!base.Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        
    }

    public override void ClearChanges()
    {
        base.ClearChanges();

        GetOriginalValues();
    }
}
