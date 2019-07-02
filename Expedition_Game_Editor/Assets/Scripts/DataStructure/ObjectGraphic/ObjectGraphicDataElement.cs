using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ObjectGraphicDataElement : ObjectGraphicCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public ObjectGraphicDataElement() : base() { }

    public int category;

    public string iconPath;
    public string originalIconPath;

    public override void Update()
    {
        if (!Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        originalIconPath = iconPath;

        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        iconPath = originalIconPath;
    }

    public override void ClearChanges()
    {
        base.ClearChanges();

        GetOriginalValues();
    }
}
