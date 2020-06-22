using UnityEngine;
using System.Linq;

public class TerrainTileCore : GeneralData
{
    private int terrainId;
    private int tileId;

    //Original
    public int originalTileId;

    //Changed
    private bool changedTileId;

    public bool Changed
    {
        get
        {
            return changedTileId;
        }
    }

    #region Properties
    public int TerrainId
    {
        get { return terrainId; }
        set { terrainId = value; }
    }

    public int TileId
    {
        get { return tileId; }
        set
        {
            if (value == tileId) return;

            changedTileId = (value != originalTileId);

            tileId = value;
        }
    }
    #endregion

    #region Methods
    public void Create() { }

    public virtual void Update()
    {
        var terrainTileData = Fixtures.terrainTileList.Where(x => x.Id == Id).FirstOrDefault();

        if (changedTileId)
            terrainTileData.tileId = tileId;
    }

    public void UpdateSearch() { }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalTileId = TileId;
    }

    public void GetOriginalValues()
    {
        TileId = originalTileId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedTileId = false;
    }

    public void Delete() { }
    #endregion

    new public virtual void Copy(IElementData dataSource)
    {
        var terrainTileDataSource = (TerrainTileElementData)dataSource;

        terrainId = terrainTileDataSource.terrainId;
        tileId = terrainTileDataSource.tileId;
    }
}
