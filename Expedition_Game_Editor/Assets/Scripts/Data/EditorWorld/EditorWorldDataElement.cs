using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EditorWorldDataElement : GeneralData, IDataElement
{
    public Enums.RegionType regionType;

    public int regionSize;
    public int terrainSize;
    public float tileSize;

    public string tileSetName;

    public Vector3 startPosition;

    public List<TerrainDataElement> terrainDataList;

    public List<PhaseDataElement> phaseDataList;

    #region DataElement
    public SelectionElement SelectionElement { get; set; }

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

    public IDataElement Clone()
    {
        var dataElement = new EditorWorldDataElement();

        CloneGeneralData(dataElement);

        return dataElement;
    }

    new public virtual void Copy(IDataElement dataSource)
    {
        var worldDataSource = (EditorWorldDataElement)dataSource;

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
    #endregion
}
