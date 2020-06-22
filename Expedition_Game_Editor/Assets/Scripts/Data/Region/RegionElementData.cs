using UnityEngine;
using System.Collections.Generic;

public class RegionElementData : RegionCore, IElementData
{
    public DataElement DataElement { get; set; }

    public RegionElementData() : base()
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
    public List<TerrainElementData> terrainDataList = new List<TerrainElementData>();

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

    public IElementData Clone()
    {
        var elementData = new RegionElementData();

        Debug.Log("probably remove this");
        elementData.DataElement = DataElement;

        elementData.type = type;

        elementData.tileSize = tileSize;
        elementData.tileIconPath = tileIconPath;

        elementData.tileSetName = tileSetName;

        elementData.startPosition = startPosition;

        //Original
        elementData.originalTileIconPath = originalTileIconPath;

        CloneCore(elementData);
        
        return elementData;
    }

    public override void Copy(IElementData dataSource)
    {
        base.Copy(dataSource);

        var regionDataSource = (RegionElementData)dataSource;

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
