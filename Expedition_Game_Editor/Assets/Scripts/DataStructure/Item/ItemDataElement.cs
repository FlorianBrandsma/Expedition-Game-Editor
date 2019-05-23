using UnityEngine;
using System.Collections;

public class ItemDataElement : ItemCore, IDataElement
{
    public ItemDataElement() : base() { }

    public string objectGraphicPath;
    public string objectGraphicIcon;

    public string originalObjectGraphicPath;
    public string originalObjectGraphicIcon;

    public override void Update()
    {
        if (!changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        originalObjectGraphicPath = objectGraphicPath;
        originalObjectGraphicIcon = objectGraphicIcon;
        
        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        objectGraphicPath = originalObjectGraphicPath;
        objectGraphicIcon = originalObjectGraphicIcon;
    }

    public override void ClearChanges()
    {
        base.ClearChanges();

        GetOriginalValues();
    }

    public bool Changed { get { return changed; } }
}
