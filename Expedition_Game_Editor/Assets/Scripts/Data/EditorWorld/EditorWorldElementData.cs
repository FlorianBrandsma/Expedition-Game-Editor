using UnityEngine;
using System.Collections.Generic;

public class EditorWorldElementData : GeneralData, IElementData
{
    public Enums.RegionType regionType;

    public int regionSize;
    public int terrainSize;
    public float tileSize;

    public string tileSetName;

    public Vector3 startPosition;

    public List<TerrainElementData> terrainDataList;

    public List<PhaseElementData> phaseDataList;

    public DataElement DataElement { get; set; }
    public bool Changed { get; set; }

    public void Update() { }

    public void UpdateSearch() { }

    public void UpdateIndex() { }

    public void SetOriginalValues()
    {
        terrainDataList.ForEach(x => x.SetOriginalValues());
        phaseDataList.ForEach(x => x.SetOriginalValues());
    }

    public void GetOriginalValues() { }

    public void ClearChanges() { }

    public IElementData Clone()
    {
        var elementData = new EditorWorldElementData();

        CloneGeneralData(elementData);

        return elementData;
    }

    new public virtual void Copy(IElementData dataSource)
    {
        var worldDataSource = (EditorWorldElementData)dataSource;

        regionType = worldDataSource.regionType;

        regionSize = worldDataSource.regionSize;
        terrainSize = worldDataSource.terrainSize;
        tileSize = worldDataSource.tileSize;

        tileSetName = worldDataSource.tileSetName;

        startPosition = worldDataSource.startPosition;

        for (int i = 0; i < terrainDataList.Count; i++)
        {
            var terrainDataSource = worldDataSource.terrainDataList[i];
            terrainDataList[i].Copy(terrainDataSource);
        }

        for(int i = 0; i < phaseDataList.Count; i++)
        {
            var phaseDataSource = worldDataSource.phaseDataList[i];
            phaseDataList[i].Copy(phaseDataSource);
        }        
    }
}
