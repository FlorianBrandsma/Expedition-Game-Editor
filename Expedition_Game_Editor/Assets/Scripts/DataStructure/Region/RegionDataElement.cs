using UnityEngine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class RegionDataElement : RegionCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public RegionDataElement() : base()
    {
        DataType = Enums.DataType.Region;
    }

    public WorldDataElement worldDataElement;

    public Enums.RegionType type;

    public string tileIconPath;

    public string originalTileIconPath;

    public override void Update()
    {
        if (!Changed) return;

        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        originalTileIconPath = tileIconPath;

        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        tileIconPath = originalTileIconPath;
    }

    public override void ClearChanges()
    {
        if (!Changed) return;

        base.ClearChanges();

        GetOriginalValues();
    }

    public IDataElement Clone()
    {
        var dataElement = new RegionDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var regionDataSource = (RegionDataElement)dataSource;

        worldDataElement = regionDataSource.worldDataElement;

        type = regionDataSource.type;

        tileIconPath = regionDataSource.tileIconPath;

        SetOriginalValues();
    }
}
