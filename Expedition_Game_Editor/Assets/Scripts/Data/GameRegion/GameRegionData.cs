using UnityEngine;
using System.Collections.Generic;

public class GameRegionData
{
    public int Id                   { get; set; }

    public int PhaseId              { get; set; }

    public Enums.RegionType Type    { get; set; }

    public int RegionSize           { get; set; }
    public int TerrainSize          { get; set; }

    public string TileSetName       { get; set; }
    public float TileSize           { get; set; }

    public List<GameTerrainElementData> TerrainDataList { get; set; } = new List<GameTerrainElementData>();

    public virtual void GetOriginalValues(GameRegionData originalData)
    {
        Id          = originalData.Id;

        PhaseId     = originalData.PhaseId;

        Type        = originalData.Type;

        RegionSize  = originalData.RegionSize;
        TerrainSize = originalData.TerrainSize;

        TileSetName = originalData.TileSetName;
        TileSize    = originalData.TileSize;
    }

    public GameRegionData Clone()
    {
        var data = new GameRegionData();
        
        data.Id             = Id;

        data.PhaseId        = PhaseId;

        data.Type           = Type;

        data.RegionSize     = RegionSize;
        data.TerrainSize    = TerrainSize;

        data.TileSetName    = TileSetName;
        data.TileSize       = TileSize;

        return data;
    }

    public virtual void Clone(GameRegionElementData elementData)
    {
        elementData.Id          = Id;

        elementData.PhaseId     = PhaseId;

        elementData.Type        = Type;

        elementData.RegionSize  = RegionSize;
        elementData.TerrainSize = TerrainSize;

        elementData.TileSetName = TileSetName;
        elementData.TileSize    = TileSize;
    }
}
