using UnityEngine;

public class GameTerrainTileData
{
    public int Id                   { get; set; }

    public int TileId               { get; set; }

    public bool Active              { get; set; }

    public GridElement GridElement  { get; set; }

    public virtual void GetOriginalValues(GameTerrainTileData originalData)
    {
        Id          = originalData.Id;

        TileId      = originalData.TileId;

        Active      = originalData.Active;

        GridElement = originalData.GridElement;
    }

    public GameTerrainTileData Clone()
    {
        var data = new GameTerrainTileData();
        
        data.Id             = Id;

        data.TileId         = TileId;

        data.Active         = Active;

        data.GridElement    = GridElement;

        return data;
    }

    public virtual void Clone(GameTerrainTileElementData elementData)
    {
        elementData.Id          = Id;

        elementData.TileId      = TileId;

        elementData.Active      = Active;

        elementData.GridElement = GridElement;
    }
}
