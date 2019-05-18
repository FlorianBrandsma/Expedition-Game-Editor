using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ChapterElementDataElement : ChapterElementCore, IDataElement
{
    public ChapterElementDataElement() : base() { }

    public string objectGraphicIcon;
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

        originalObjectGraphicIcon = objectGraphicIcon;

        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        objectGraphicIcon = originalObjectGraphicIcon;
    }

    public override void ClearChanges()
    {
        base.ClearChanges();

        GetOriginalValues();
    }
}
