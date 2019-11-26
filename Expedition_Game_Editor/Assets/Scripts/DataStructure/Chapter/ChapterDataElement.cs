using UnityEngine;
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

    public IDataElement Copy()
    {
        var dataElement = new ChapterDataElement();

        CopyGeneralData(dataElement);

        return dataElement;
    }
}
