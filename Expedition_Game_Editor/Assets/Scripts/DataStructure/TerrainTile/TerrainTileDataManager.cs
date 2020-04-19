using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainTileDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<TerrainTileData> terrainTileDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.TileData> tileDataList;

    public TerrainTileDataManager(TerrainTileController terrainTileController)
    {
        DataController = terrainTileController;
    }

    public List<IDataElement> GetDataElements(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.TerrainTile>().First();

        GetTerrainTileData(searchParameters);

        if (terrainTileDataList.Count == 0) return new List<IDataElement>();

        GetTileData();

        var list = (from terrainTileData in terrainTileDataList
                    join tileData in tileDataList on terrainTileData.tileId equals tileData.Id
                    select new TerrainTileDataElement()
                    {
                        Id = terrainTileData.Id,
                        Index = terrainTileData.Index,

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
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(terrainTile.Id)) continue;
            if (searchParameters.regionId.Count > 0)
            {
                var terrain = Fixtures.terrainList.Where(x => x.Id == terrainTile.terrainId).FirstOrDefault();

                if(!searchParameters.regionId.Contains(terrain.regionId)) continue;
            }

            var terrainTileData = new TerrainTileData();

            terrainTileData.Id = terrainTile.Id;
            terrainTileData.Index = terrainTile.Index;

            terrainTileData.terrainId = terrainTile.terrainId;
            terrainTileData.tileId = terrainTile.tileId;

            terrainTileDataList.Add(terrainTileData);
        }
    }

    internal void GetTileData()
    {
        var tileSearchParameters = new Search.Tile();
        tileSearchParameters.id = terrainTileDataList.Select(x => x.tileId).Distinct().ToList();

        tileDataList = dataManager.GetTileData(tileSearchParameters);
    }

    internal class TerrainTileData : GeneralData
    {
        public int terrainId;
        public int tileId;
    }
}
