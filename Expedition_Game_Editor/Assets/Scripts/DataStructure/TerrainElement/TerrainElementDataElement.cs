﻿using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TerrainElementDataElement : TerrainElementCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public TerrainElementDataElement() : base() { }

    public string elementName;
    public string objectGraphicIconPath;

    public string originalElementName;
    public string originalObjectGraphicIconPath;

    public override void Update()
    {
        if (!base.Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        originalElementName = elementName;
        originalObjectGraphicIconPath = objectGraphicIconPath;

        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        elementName = originalElementName;
        objectGraphicIconPath = originalObjectGraphicIconPath;
    }

    public override void ClearChanges()
    {
        base.ClearChanges();

        GetOriginalValues();
    }
}
