﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class ChapterDataElement : ChapterCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public ChapterDataElement() : base() { }

    public override void Update()
    {
        if (!changed) return;

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

    public bool Changed { get { return changed; } }
}
