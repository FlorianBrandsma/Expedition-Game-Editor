using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TileDataElement : TileCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public TileDataElement() : base() { }

    public string icon;
    public string originalIcon;

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
        base.ClearChanges();

        GetOriginalValues();
    }
}
