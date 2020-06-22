using UnityEngine;
using System;
using System.Collections.Generic;

public class RegionDataElement : RegionCore, IDataElement
{
    public DataElement DataElement { get; set; }

    public RegionDataElement() : base()
    {
        DataType = Enums.DataType.Region;
    }

    public Enums.RegionType type;

    public float tileSize;
    public string tileIconPath;

    public string tileSetName;

    public Vector2 startPosition;

    //Original
    public string originalTileIconPath;

    //List
    public List<TerrainDataElement> terrainDataList = new List<TerrainDataElement>();

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

        terrainDataList.ForEach(x => x.SetOriginalValues());

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

        Debug.Log("probably remove this");
        dataElement.DataElement = DataElement;

        dataElement.type = type;

        dataElement.tileSize = tileSize;
        dataElement.tileIconPath = tileIconPath;

        dataElement.tileSetName = tileSetName;

        dataElement.startPosition = startPosition;

        //Original
        dataElement.originalTileIconPath = originalTileIconPath;

        CloneCore(dataElement);
        
        return dataElement;
    }

    public override void Copy(IDataElement dataSource)
    {
        base.Copy(dataSource);

        var regionDataSource = (RegionDataElement)dataSource;

        type = regionDataSource.type;

        tileSize = regionDataSource.tileSize;
        tileIconPath = regionDataSource.tileIconPath;

        tileSetName = regionDataSource.tileSetName;

        startPosition = regionDataSource.startPosition;

        for (int i = 0; i < terrainDataList.Count; i++)
        {
            var terrainDataSource = regionDataSource.terrainDataList[i];
            terrainDataList[i].Copy(terrainDataSource);
        }

        SetOriginalValues();
    }
}
