using UnityEngine;
using System.Linq;

public class TerrainTileCore : GeneralData
{
    private int terrainId;
    private int tileId;

    public int originalTerrainId;
    public int originalTileId;

    private bool changedTerrainId;
    private bool changedTileId;

    public bool Changed
    {
        get
        {
            return changedTerrainId || changedTileId;
        }
    }

    #region Properties

    public int TerrainId
    {
        get { return terrainId; }
        set
        {
            if (value == terrainId) return;

            changedTerrainId = (value != originalTerrainId);

            terrainId = value;
        }
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

        if (changedTerrainId)
            terrainTileData.terrainId = terrainId;

        if (changedTileId)
            terrainTileData.tileId = tileId;
    }

    public void UpdateSearch() { }

    public void UpdateIndex() { }

    public virtual void SetOriginalValues()
    {
        originalTerrainId = TerrainId;
        originalTileId = TileId;
    }

    public void GetOriginalValues()
    {
        TerrainId = originalTerrainId;
        TileId = originalTileId;
    }

    public virtual void ClearChanges()
    {
        GetOriginalValues();

        changedTerrainId = false;
        changedTileId = false;
    }

    public void Delete() { }

    #endregion
}
