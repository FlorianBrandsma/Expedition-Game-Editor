using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class RegionDataManager
{
    private static List<RegionBaseData> regionDataList;

    private static List<TerrainBaseData> terrainDataList;
    private static List<TerrainTileBaseData> terrainTileDataList;
    private static List<TileSetBaseData> tileSetDataList;
    private static List<TileBaseData> tileDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Region>().First();

        GetRegionData(searchParameters);

        if (regionDataList.Count == 0) return new List<IElementData>();

        GetTerrainData();
        GetTerrainTileData();

        GetTileSetData();
        GetTileData();

        var list = (from regionData     in regionDataList
                    join tileSetData    in tileSetDataList on regionData.TileSetId equals tileSetData.Id

                    //join leftJoin in (from terrainData in terrainDataList
                    //                  select new { terrainData }) on regionData.Id equals leftJoin.terrainData.RegionId into terrainData

                    //join leftJoin in (from terrainTileData in terrainTileDataList
                    //                  select new { terrainTileData }) on terrainData.FirstOrDefault().terrainData.Id equals leftJoin.terrainTileData.TerrainId into terrainTileData

                    join leftJoin in (from tileData in tileDataList
                                      select new { tileData }) on tileSetData.Id equals leftJoin.tileData.TileSetId into tileData

                    select new RegionElementData()
                    {
                        Id = regionData.Id,
                        
                        ChapterRegionId = regionData.ChapterRegionId,
                        PhaseId = regionData.PhaseId,
                        TileSetId = regionData.TileSetId,

                        Index = regionData.Index,

                        Name = regionData.Name,

                        RegionSize = regionData.RegionSize,
                        TerrainSize = regionData.TerrainSize,

                        Type = searchParameters.type,

                        TileSize = tileSetData.TileSize,
                        TileIconPath = tileData.First().tileData.IconPath,

                        TerrainDataList = 
                        (from terrainData in terrainDataList where regionData.Id == terrainData.RegionId
                         select new TerrainElementData()
                         {
                             Id = terrainData.Id,

                             RegionId = terrainData.RegionId,

                             Index = terrainData.Index,
                             
                             TerrainTileDataList =
                             (from terrainTileData in terrainTileDataList where terrainData.Id == terrainTileData.TerrainId
                              select new TerrainTileElementData()
                              {
                                  Id = terrainTileData.Id,

                                  TerrainId = terrainTileData.TerrainId,

                                  Index = terrainTileData.Index

                              }).ToList()
                         }).ToList()
                        
                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetRegionData(Search.Region searchParameters)
    {
        regionDataList = new List<RegionBaseData>();

        foreach(RegionBaseData region in Fixtures.regionList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(region.Id)) continue;
            if (searchParameters.phaseId.Count  > 0 && !searchParameters.phaseId.Contains(region.PhaseId)) continue;

            var regionData = new RegionBaseData();

            regionData.Id = region.Id;
            regionData.Index = region.Index;

            regionData.ChapterRegionId = region.ChapterRegionId;
            regionData.PhaseId = region.PhaseId;
            regionData.TileSetId = region.TileSetId;
            regionData.Name = region.Name;
            regionData.RegionSize = region.RegionSize;
            regionData.TerrainSize = region.TerrainSize;

            regionDataList.Add(regionData);
        }
    }

    private static void GetTerrainData()
    {
        var terrainSearchParameters = new Search.Terrain();
        terrainSearchParameters.regionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        terrainDataList = DataManager.GetTerrainData(terrainSearchParameters);
    }

    private static void GetTerrainTileData()
    {
        var terrainTileSearchParameters = new Search.TerrainTile();
        terrainTileSearchParameters.terrainId = terrainDataList.Select(x => x.Id).Distinct().ToList();

        terrainTileDataList = DataManager.GetTerrainTileData(terrainTileSearchParameters);
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

    public static void UpdateData(RegionElementData elementData)
    {
        var data = Fixtures.regionList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedChapterRegionId)
            data.ChapterRegionId = elementData.ChapterRegionId;

        if (elementData.ChangedPhaseId)
            data.PhaseId = elementData.PhaseId;

        if (elementData.ChangedTileSetId)
            data.TileSetId = elementData.TileSetId;

        if (elementData.ChangedName)
            data.Name = elementData.Name;

        if (elementData.ChangedRegionSize)
            data.RegionSize = elementData.RegionSize;

        if (elementData.ChangedTerrainSize)
            data.TerrainSize = elementData.TerrainSize;
    }

    static public void UpdateIndex(RegionElementData elementData)
    {
        var data = Fixtures.regionList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;
    }
}
