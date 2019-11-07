using UnityEngine;
using System.Collections;

public class ItemDataElement : ItemCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public ItemDataElement() : base() { }

    public string objectGraphicPath;
    public string objectGraphicIconPath;

    public string originalObjectGraphicPath;
    public string originalObjectGraphicIconPath;

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

        originalObjectGraphicPath = objectGraphicPath;
        originalObjectGraphicIconPath = objectGraphicIconPath;
        
        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        objectGraphicPath = originalObjectGraphicPath;
        objectGraphicIconPath = originalObjectGraphicIconPath;
    }

    public override void ClearChanges()
    {
        base.ClearChanges();

        GetOriginalValues();
    }
}
