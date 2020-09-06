using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class TerrainDataManager
{
    private static List<TerrainBaseData> terrainDataList;

    private static List<RegionBaseData> regionDataList;
    private static List<TileSetBaseData> tileSetDataList;
    private static List<TileBaseData> tileDataList;

    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Terrain>().First();

        GetTerrainData(searchParameters);

        if (terrainDataList.Count == 0) return new List<IElementData>();

        GetRegionData();
        GetTileSetData();
        GetTileData();

        GetIconData();
        
        var list = (from terrainData    in terrainDataList
                    join regionData     in regionDataList   on terrainData.RegionId equals regionData.Id
                    join tileSetData    in tileSetDataList  on regionData.TileSetId equals tileSetData.Id

                    join leftJoin in (from tileData in tileDataList
                                      select new { tileData }) on tileSetData.Id equals leftJoin.tileData.TileSetId into tileData

                    join iconData       in iconDataList     on terrainData.IconId   equals iconData.Id

                    select new TerrainElementData()
                    {
                        Id = terrainData.Id,
                        Index = terrainData.Index,

                        RegionId = terrainData.RegionId,
                        IconId = terrainData.IconId,
                        Name = terrainData.Name,

                        IconPath = iconData.Path,

                        TileSetId = regionData.TileSetId,
                        BaseTilePath = tileData.First().tileData.IconPath

                    }).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetTerrainData(Search.Terrain searchParameters)
    {
        terrainDataList = new List<TerrainBaseData>();

        foreach (TerrainBaseData terrain in Fixtures.terrainList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(terrain.Id)) continue;
            if (searchParameters.regionId.Count > 0 && !searchParameters.regionId.Contains(terrain.RegionId)) continue;
            
            var terrainData = new TerrainBaseData();

            terrainData.Id = terrain.Id;
            terrainData.Index = terrain.Index;

            terrainData.RegionId = terrain.RegionId;
            terrainData.IconId = terrain.IconId;
            terrainData.Name = terrain.Name;

            terrainDataList.Add(terrainData);
        }
    }

    private static void GetRegionData()
    {
        var regionSearchParameters = new Search.Region();
        regionSearchParameters.id = terrainDataList.Select(x => x.RegionId).Distinct().ToList();

        regionDataList = DataManager.GetRegionData(regionSearchParameters);
    }

    private static void GetTileSetData()
    {
        var tileSetSearchParameters = new Search.TileSet();
        tileSetSearchParameters.id = regionDataList.Select(x => x.TileSetId).Distinct().ToList();

        tileSetDataList = DataManager.GetTileSetData(tileSetSearchParameters);
    }

    private static void GetTileData()
    {
        var tileSearchParameters = new Search.Tile();
        tileSearchParameters.tileSetId = tileSetDataList.Select(x => x.Id).Distinct().ToList();

        tileDataList = DataManager.GetTileData(tileSearchParameters);
    }

    private static void GetIconData()
    {
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = terrainDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(iconSearchParameters);
    }

    public static void UpdateData(TerrainElementData elementData)
    {
        var data = Fixtures.terrainList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedIconId)
            data.IconId = elementData.IconId;

        if (elementData.ChangedName)
            data.Name = elementData.Name;
    }
}
