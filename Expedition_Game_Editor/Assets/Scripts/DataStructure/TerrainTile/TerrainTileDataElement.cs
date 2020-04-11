﻿using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TerrainTileDataElement : TerrainTileCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public TerrainTileDataElement() : base()
    {
        DataType = Enums.DataType.TerrainTile;
    }

    public string iconPath;

    public string originalIconPath;

    public override void Update()
    {
        if (!Changed) return;
        
        base.Update();

        SetOriginalValues();
    }

    public override void SetOriginalValues()
    {
        base.SetOriginalValues();

        originalIconPath = iconPath;

        ClearChanges();
    }

    public new void GetOriginalValues()
    {
        iconPath = originalIconPath;
    }

    public override void ClearChanges()
    {
        if (!Changed) return;

        base.ClearChanges();

        GetOriginalValues();
    }

    public IDataElement Clone()
    {
        var dataElement = new TerrainTileDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var terrainTileDataSource = (TerrainTileDataElement)dataSource;

        iconPath = terrainTileDataSource.iconPath;

        SetOriginalValues();
    }
}
