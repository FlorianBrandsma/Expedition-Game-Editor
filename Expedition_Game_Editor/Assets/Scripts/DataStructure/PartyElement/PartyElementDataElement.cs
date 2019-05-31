using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PartyElementDataElement : PartyElementCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public PartyElementDataElement() : base() { }

    public string elementName;
    public string objectGraphicIcon;

    public string originalElementName;
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

        originalElementName = elementName;
        originalObjectGraphicIcon = objectGraphicIcon;

        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        elementName = originalElementName;
        objectGraphicIcon = originalObjectGraphicIcon;
    }

    public override void ClearChanges()
    {
        base.ClearChanges();

        GetOriginalValues();
    }

    public bool Changed { get { return changed; } }
}
