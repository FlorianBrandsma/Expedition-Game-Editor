using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainTileDataManager
{
    private TerrainTileController terrainTileController;
    private List<TerrainTileData> terrainTileDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.TileData> tileDataList;
    private List<DataManager.IconData> iconDataList;

    public void InitializeManager(TerrainTileController terrainTileController)
    {
        this.terrainTileController = terrainTileController;
    }

    public List<IDataElement> GetTerrainTileDataElements(IEnumerable searchParameters)
    {
        var terrainTileSearchData = searchParameters.Cast<Search.TerrainTile>().FirstOrDefault();

        GetTerrainTileData(terrainTileSearchData);
        GetTileData();

        var list = (from terrainTileData in terrainTileDataList
                    join tileData in tileDataList on terrainTileData.tileId equals tileData.id
                    select new TerrainTileDataElement()
                    {
                        id = terrainTileData.id,
                        table = "TerrainTile",
                        index = terrainTileData.index,

                        TerrainId = terrainTileData.terrainId,
                        TileId = terrainTileData.tileId,

                        iconPath = tileData.iconPath

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetTerrainTileData(Search.TerrainTile searchParameters)
    {
        terrainTileDataList = new List<TerrainTileData>();

        foreach (Fixtures.TerrainTile terrainTile in Fixtures.terrainTileList)
        {
            if (searchParameters.regionId.Count > 0)
            {
                var terrain = Fixtures.terrainList.Where(x => x.id == terrainTile.terrainId).FirstOrDefault();

                if(!searchParameters.regionId.Contains(terrain.regionId)) continue;
            }

            var terrainTileData = new TerrainTileData();

            terrainTileData.id = terrainTile.id;
            terrainTileData.index = terrainTile.index;

            terrainTileData.terrainId = terrainTile.terrainId;
            terrainTileData.tileId = terrainTile.tileId;

            terrainTileDataList.Add(terrainTileData);
        }
    }

    internal void GetTileData()
    {
        tileDataList = dataManager.GetTileData(terrainTileDataList.Select(x => x.tileId).Distinct().ToList(), true);
    }

    internal class TerrainTileData : GeneralData
    {
        public int terrainId;
        public int tileId;
    }
}
