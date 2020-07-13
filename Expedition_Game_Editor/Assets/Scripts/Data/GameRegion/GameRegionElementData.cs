using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRegionElementData : GeneralData, IElementData
{
    public DataElement DataElement { get; set; }

    public GameRegionElementData() : base()
    {
        DataType = Enums.DataType.GameRegion;
    }

    public int phaseId;

    public Enums.RegionType type;
    public int regionSize;
    public int terrainSize;

    public string tileSetName;
    public float tileSize;

    public List<GameTerrainElementData> terrainDataList;

    #region ElementData
    public bool Changed { get { return false; } }
    public void Create() { }
    public void Update() { }
    public void UpdateSearch() { }
    public void UpdateIndex() { }
    public virtual void SetOriginalValues() { }
    public void GetOriginalValues() { }
    public virtual void ClearChanges() { }
    public void Delete() { }
    public IElementData Clone()
    {
        var elementData = new GameRegionElementData();

        CloneGeneralData(elementData);

        return elementData;
    }
    #endregion
}
