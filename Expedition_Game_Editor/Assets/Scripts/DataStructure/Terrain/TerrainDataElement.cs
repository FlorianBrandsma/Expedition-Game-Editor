using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class TerrainDataElement : TerrainCore, IDataElement
{
    public SelectionElement SelectionElement { get; set; }

    public TerrainDataElement() : base()
    {
        DataType = Enums.DataType.Terrain;
    }

    public int tileSetId;

    public string iconPath;
    public string baseTilePath;
    
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
        var dataElement = new TerrainDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var terrainDataSource = (TerrainDataElement)dataSource;

        tileSetId = terrainDataSource.tileSetId;

        iconPath = terrainDataSource.iconPath;
        baseTilePath = terrainDataSource.baseTilePath;

        SetOriginalValues();
    }
}