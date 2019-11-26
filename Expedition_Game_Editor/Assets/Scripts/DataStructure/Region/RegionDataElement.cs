using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class RegionDataElement : RegionCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public RegionDataElement() : base() { }

    public SceneDataElement sceneDataElement;

    public Enums.RegionType type;

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
        var dataElement = new RegionDataElement();

        CopyGeneralData(dataElement);

        return dataElement;
    }
}
