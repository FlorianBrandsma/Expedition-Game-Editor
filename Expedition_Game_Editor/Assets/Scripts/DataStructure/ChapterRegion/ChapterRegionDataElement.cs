using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ChapterRegionDataElement : ChapterRegionCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public ChapterRegionDataElement() : base() { }

    public string name;
    public string originalName;

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

        ClearChanges();
    }

    public new void GetOriginalValues() { }

    public override void ClearChanges()
    {
        base.ClearChanges();

        GetOriginalValues();
    }
}