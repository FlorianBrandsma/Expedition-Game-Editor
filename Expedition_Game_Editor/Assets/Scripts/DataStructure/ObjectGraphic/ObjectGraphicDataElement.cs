using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ObjectGraphicDataElement : ObjectGraphicCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public ObjectGraphicDataElement() : base() { }

    public int category;

    public string iconPath;

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
        var dataElement = new ObjectGraphicDataElement();

        CopyGeneralData(dataElement);

        return dataElement;
    }
}
