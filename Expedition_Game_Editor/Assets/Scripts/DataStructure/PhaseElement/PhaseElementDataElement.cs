﻿using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PhaseElementDataElement : PhaseElementCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public PhaseElementDataElement() : base() { }

    public Enums.ElementStatus elementStatus;

    public string elementName;
    public string objectGraphicIcon;

    public string originalElementName;
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
}