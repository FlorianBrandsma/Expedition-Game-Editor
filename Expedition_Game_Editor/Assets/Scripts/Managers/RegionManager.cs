using UnityEngine;
using System.Collections.Generic;
using System.Linq;

static public class RegionManager
{
    public enum Display
    {
        World,
        Tiles,
    }

    static public Display activeDisplay;

    //Keeps track of the active region type
    static public Enums.RegionType regionType;

    static public Vector2 PositionOnTile(int regionSize, int terrainSize, float tileSize, float posX, float posZ)
    {
        var worldSize = regionSize * terrainSize * tileSize;

        var tilePosition = new Vector2(Mathf.FloorToInt(posX / tileSize) * tileSize + tileSize / 2, 
                                       Mathf.FloorToInt(posZ / tileSize) * tileSize + tileSize / 2);

        var positionOnTile = new Vector2(posX - tilePosition.x, -posZ + tilePosition.y);

        return positionOnTile;
    }

    static public string LocationName(int regionId, float positionX, float positionZ,
                                      List<DataManager.RegionData> regionDataList, List<DataManager.TileSetData> tileSetDataList, List<DataManager.TerrainData> terrainDataList)
    {
        var region = regionDataList.Where(x => x.Id == regionId).First();
        var tileSet = tileSetDataList.Where(x => x.Id == region.tileSetId).FirstOrDefault();
        var terrains = terrainDataList.Where(x => x.regionId == region.Id).Distinct().ToList();

        var terrainSize = region.terrainSize * tileSet.tileSize;

        var terrainCoordinates = new Vector2(Mathf.Floor(positionX / terrainSize),
                                             Mathf.Floor(positionZ / terrainSize));

        var terrainIndex = (region.regionSize * terrainCoordinates.y) + terrainCoordinates.x;

        var terrainId = terrains.Where(x => x.Index == terrainIndex).Select(x => x.Id).FirstOrDefault();

        var terrain = terrainDataList.Where(x => x.Id == terrainId).FirstOrDefault();
        
        return region.name + ", " + terrain.name;
    }

    static public void SetDisplay(int display, Path path)
    {
        activeDisplay = (Display)display;

        RenderManager.Render(path);
    }
}
