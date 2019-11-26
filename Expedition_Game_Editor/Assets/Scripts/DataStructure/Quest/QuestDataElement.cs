using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class QuestDataElement : QuestCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public QuestDataElement() : base() { }

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
        var dataElement = new QuestDataElement();

        CopyGeneralData(dataElement);

        return dataElement;
    }
}