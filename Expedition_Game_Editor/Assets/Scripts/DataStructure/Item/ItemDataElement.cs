using UnityEngine;
using System.Collections;

public class ItemDataElement : ItemCore
{
    public ItemDataElement() : base() { }

    public string objectGraphicName;
    public string objectGraphicIcon;

    public string originalObjectGraphicName;
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

        originalObjectGraphicName = objectGraphicName;
        originalObjectGraphicIcon = objectGraphicIcon;
        
        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        objectGraphicName = originalObjectGraphicName;
        objectGraphicIcon = originalObjectGraphicIcon;
    }

    public override void ClearChanges()
    {
        base.ClearChanges();

        GetOriginalValues();
    }
}
