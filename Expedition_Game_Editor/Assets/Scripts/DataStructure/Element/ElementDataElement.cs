using UnityEngine;
using System.Collections;

[System.Serializable]
public class ElementDataElement : ElementCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public ElementDataElement() : base() { }

    public string objectGraphicPath;
    public string objectGraphicIcon;

    public string originalObjectGraphicPath;
    public string originalObjectGraphicIcon;

    public override void Update()
    {
        if (!Changed) return;

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
}
