using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class TerrainTileDataManager
{
    private static List<TerrainTileBaseData> terrainTileDataList;

    private static List<TileBaseData> tileDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.TerrainTile>().First();

        GetTerrainTileData(searchParameters);

        if (terrainTileDataList.Count == 0) return new List<IElementData>();

        GetTileData();

        var list = (from terrainTileData    in terrainTileDataList
                    join tileData           in tileDataList on terrainTileData.TileId equals tileData.Id
                    select new TerrainTileElementData()
                    {
                        Id = terrainTileData.Id,
                        Index = terrainTileData.Index,

                        TerrainId = terrainTileData.TerrainId,
                        TileId = terrainTileData.TileId,

                        IconPath = tileData.IconPath

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetTerrainTileData(Search.TerrainTile searchParameters)
    {
        terrainTileDataList = new List<TerrainTileBaseData>();

        foreach (TerrainTileBaseData terrainTile in Fixtures.terrainTileList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(terrainTile.Id)) continue;
            if (searchParameters.regionId.Count > 0)
            {
                var terrain = Fixtures.terrainList.Where(x => x.Id == terrainTile.TerrainId).FirstOrDefault();

                if(!searchParameters.regionId.Contains(terrain.RegionId)) continue;
            }

            var terrainTileData = new TerrainTileBaseData();

            terrainTileData.Id = terrainTile.Id;
            terrainTileData.Index = terrainTile.Index;

            terrainTileData.TerrainId = terrainTile.TerrainId;
            terrainTileData.TileId = terrainTile.TileId;

            terrainTileDataList.Add(terrainTileData);
        }
    }

    private static void GetTileData()
    {
        var tileSearchParameters = new Search.Tile();
        tileSearchParameters.id = terrainTileDataList.Select(x => x.TileId).Distinct().ToList();

        tileDataList = DataManager.GetTileData(tileSearchParameters);
    }

    public static void UpdateData(TerrainTileElementData elementData)
    {
        var data = Fixtures.terrainTileList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedTileId)
            data.TileId = elementData.TileId;
    }
}
