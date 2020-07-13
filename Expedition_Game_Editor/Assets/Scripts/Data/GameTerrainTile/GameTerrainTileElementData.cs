using UnityEngine;

public class GameTerrainTileElementData : GeneralData, IElementData
{
    public DataElement DataElement { get; set; }

    public GameTerrainTileElementData() : base()
    {
        DataType = Enums.DataType.GameTerrainTile;
    }

    public int tileId;

    public bool active;

    public GridElement gridElement;

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
        var elementData = new GameTerrainTileElementData();

        CloneGeneralData(elementData);

        return elementData;
    }
    #endregion
}
