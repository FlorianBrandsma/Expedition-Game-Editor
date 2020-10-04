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

    static public int GetGameTerrainTileId(GameRegionElementData regionData, float posX, float posZ)
    {
        var terrainId = GetGameTerrainId(regionData, regionData.TileSize, posX, posZ);

        var terrainTiles = regionData.TerrainDataList.Where(x => x.Id == terrainId).First().TerrainTileDataList;

        var terrainSize = regionData.TerrainSize * regionData.TileSize;

        var terrainCoordinates = new Vector2(Mathf.Floor(posX / terrainSize),
                                             Mathf.Floor(posZ / terrainSize));

        var terrainPosition = new Vector2(terrainCoordinates.x * terrainSize,
                                          terrainCoordinates.y * terrainSize);

        var localPosition = new Vector2(posX - terrainPosition.x,
                                        posZ - terrainPosition.y);

        var tileCoordinates = new Vector2(Mathf.Floor(localPosition.x / regionData.TileSize),
                                          Mathf.Floor(localPosition.y / regionData.TileSize));

        var tileIndex = (regionData.TerrainSize * tileCoordinates.y) + tileCoordinates.x;
        
        var terrainTileId = terrainTiles.Where(x => x.Index == tileIndex).Select(x => x.Id).FirstOrDefault();

        return terrainTileId;
    }

    static public int GetGameTerrainId(GameRegionElementData regionData, float tileSize, float posX, float posZ)
    {
        var terrainSize = regionData.TerrainSize * tileSize;

        var terrainCoordinates = new Vector2(Mathf.Floor(posX / terrainSize),
                                             Mathf.Floor(posZ / terrainSize));
        
        var terrainIndex = (regionData.RegionSize * terrainCoordinates.y) + terrainCoordinates.x;

        var terrainId = regionData.TerrainDataList.Where(x => x.Index == terrainIndex).Select(x => x.Id).FirstOrDefault();

        return terrainId;
    }

    static public int GetTerrainTileId(RegionBaseData regionData, List<TerrainBaseData> terrainDataList, List<TerrainTileBaseData> terrainTileDataList, float tileSize, float posX, float posZ)
    {
        var terrainId = GetTerrainId(regionData, terrainDataList, tileSize, posX, posZ);

        var terrainTiles = terrainTileDataList.Where(x => x.TerrainId == terrainId).Distinct().ToList();

        var terrainSize = regionData.TerrainSize * tileSize;

        var terrainCoordinates = new Vector2(Mathf.Floor(posX / terrainSize),
                                             Mathf.Floor(posZ / terrainSize));

        var terrainPosition = new Vector2(terrainCoordinates.x * terrainSize,
                                          terrainCoordinates.y * terrainSize);

        var localPosition = new Vector2(posX - terrainPosition.x,
                                        posZ - terrainPosition.y);

        var tileCoordinates = new Vector2(Mathf.Floor(localPosition.x / tileSize),
                                          Mathf.Floor(localPosition.y / tileSize));

        var tileIndex = (regionData.TerrainSize * tileCoordinates.y) + tileCoordinates.x;

        var terrainTileId = terrainTiles.Where(x => x.Index == tileIndex).Select(x => x.Id).FirstOrDefault();

        return terrainTileId;
    }

    static public int GetTerrainId(RegionBaseData regionData, List<TerrainBaseData> terrainDataList, float tileSize, float posX, float posZ)
    {
        var terrains = terrainDataList.Where(x => x.RegionId == regionData.Id).Distinct().ToList();

        var terrainSize = regionData.TerrainSize * tileSize;

        var terrainCoordinates = new Vector2(Mathf.Floor(posX / terrainSize),
                                             Mathf.Floor(posZ / terrainSize));

        

        var terrainIndex = (regionData.RegionSize * terrainCoordinates.y) + terrainCoordinates.x;

        var terrainId = terrains.Where(x => x.Index == terrainIndex).Select(x => x.Id).FirstOrDefault();

        return terrainId;
    }
    
    static public Vector2 PositionOnTile(int regionSize, int terrainSize, float tileSize, float posX, float posZ)
    {
        var tilePosition = new Vector2(Mathf.FloorToInt(posX / tileSize) * tileSize + tileSize / 2, 
                                       Mathf.FloorToInt(posZ / tileSize) * tileSize + tileSize / 2);

        var positionOnTile = new Vector2(posX - tilePosition.x, -posZ + tilePosition.y);

        return positionOnTile;
    }

    static public string LocationName(float positionX, float positionZ, float tileSize, RegionBaseData regionData, List<TerrainBaseData> terrainDataList)
    {
        var terrainCoordinates = new Vector2(Mathf.Floor(positionX / (regionData.TerrainSize * tileSize)),
                                             Mathf.Floor(positionZ / (regionData.TerrainSize * tileSize)));

        var terrainIndex = (regionData.RegionSize * terrainCoordinates.y) + terrainCoordinates.x;

        var terrainData = terrainDataList.Where(x => x.RegionId == regionData.Id && x.Index == terrainIndex).FirstOrDefault();

        return regionData.Name + ", " + terrainData.Name;
    }

    static public void SetDisplay(int display, Path path)
    {
        activeDisplay = (Display)display;

        RenderManager.Render(path);
    }
}
