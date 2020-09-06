using UnityEngine;
using System.Collections.Generic;

public class EditorWorldData
{
    public int Id                       { get; set; }

    public Enums.RegionType RegionType  { get; set; }

    public int RegionSize               { get; set; }
    public int TerrainSize              { get; set; }
    public float TileSize               { get; set; }

    public string TileSetName           { get; set; }

    public Vector3 StartPosition        { get; set; }

    public List<TerrainElementData> TerrainDataList { get; set; } = new List<TerrainElementData>();
    public List<PhaseElementData> PhaseDataList     { get; set; } = new List<PhaseElementData>();

    public virtual void GetOriginalValues(EditorWorldData originalData)
    {
        Id              = originalData.Id;

        RegionType      = originalData.RegionType;

        RegionSize      = originalData.RegionSize;
        TerrainSize     = originalData.TerrainSize;
        TileSize        = originalData.TileSize;

        TileSetName     = originalData.TileSetName;

        StartPosition   = originalData.StartPosition;
    }

    public EditorWorldData Clone()
    {
        var data = new EditorWorldData();
        
        data.Id             = Id;

        data.RegionType     = RegionType;

        data.RegionSize     = RegionSize;
        data.TerrainSize    = TerrainSize;
        data.TileSize       = TileSize;

        data.TileSetName    = TileSetName;

        data.StartPosition  = StartPosition;

        return data;
    }

    public virtual void Clone(EditorWorldElementData elementData)
    {
        elementData.Id              = Id;

        elementData.RegionType      = RegionType;

        elementData.RegionSize      = RegionSize;
        elementData.TerrainSize     = TerrainSize;
        elementData.TileSize        = TileSize;

        elementData.TileSetName     = TileSetName;

        elementData.StartPosition   = StartPosition;
    }
}
