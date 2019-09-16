﻿using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class InteractionDataElement : InteractionCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public InteractionDataElement() : base() { }

    public int objectGraphicId;
    public string objectGraphicPath;

    public string regionName;
    public string objectGraphicIconPath;

    public Vector2 startPosition;

    public override void Update()
    {
        if (!Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        ClearChanges();
    }

    public new void GetOriginalValues() { }

    public override void ClearChanges()
    {
        base.ClearChanges();

        GetOriginalValues();
    }
}
