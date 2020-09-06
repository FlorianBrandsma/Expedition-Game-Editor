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
