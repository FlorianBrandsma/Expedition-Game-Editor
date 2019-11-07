using UnityEngine;
using System.Collections;

[System.Serializable]
public class InteractableDataElement : InteractableCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public InteractableDataElement() : base() { }

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
