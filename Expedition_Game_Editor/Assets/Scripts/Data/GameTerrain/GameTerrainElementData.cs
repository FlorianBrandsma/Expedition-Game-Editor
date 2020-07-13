using UnityEngine;
using System.Collections.Generic;

public class GameTerrainElementData : GeneralData, IElementData
{
    public DataElement DataElement { get; set; }

    public GameTerrainElementData() : base()
    {
        DataType = Enums.DataType.GameTerrain;
    }

    public string name;

    public GridElement gridElement;

    public List<GameAtmosphereElementData> atmosphereDataList;
    public List<GameTerrainTileElementData> terrainTileDataList;
    public List<GameWorldInteractableElementData> worldInteractableDataList;
    public List<GameWorldObjectElementData> worldObjectDataList;

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
        var elementData = new GameTerrainElementData();

        CloneGeneralData(elementData);

        return elementData;
    }
    #endregion
}
