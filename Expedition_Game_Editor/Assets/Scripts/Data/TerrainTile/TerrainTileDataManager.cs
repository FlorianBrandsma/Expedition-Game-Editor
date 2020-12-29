using UnityEngine;
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

                    }).ToList();

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

            terrainTileDataList.Add(terrainTile);
        }
    }

    private static void GetTileData()
    {
        var searchParameters = new Search.Tile();
        searchParameters.id = terrainTileDataList.Select(x => x.TileId).Distinct().ToList();

        tileDataList = DataManager.GetTileData(searchParameters);
    }

    public static void AddData(TerrainTileElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.terrainTileList.Count > 0 ? (Fixtures.terrainTileList[Fixtures.terrainTileList.Count - 1].Id + 1) : 1;
            Fixtures.terrainTileList.Add(((TerrainTileData)elementData).Clone());
        }
        else { }
    }

    public static void UpdateSearch(TerrainTileElementData elementData)
    {
        var data = Fixtures.terrainTileList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedTileId)
            data.TileId = elementData.TileId;
    }
}
