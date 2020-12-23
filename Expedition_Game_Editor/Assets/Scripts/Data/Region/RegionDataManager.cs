using UnityEngine;
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
            if (searchParameters.id.Count           > 0 && !searchParameters.id.Contains(region.Id))            continue;
            if (searchParameters.excludeId.Count    > 0 && searchParameters.excludeId.Contains(region.Id))      continue;
            if (searchParameters.phaseId.Count      > 0 && !searchParameters.phaseId.Contains(region.PhaseId))  continue;

            regionDataList.Add(region);
        }
    }

    private static void GetTerrainData()
    {
        var searchParameters = new Search.Terrain();
        searchParameters.regionId = regionDataList.Select(x => x.Id).Distinct().ToList();

        terrainDataList = DataManager.GetTerrainData(searchParameters);
    }

    private static void GetTerrainTileData()
    {
        var searchParameters = new Search.TerrainTile();
        searchParameters.terrainId = terrainDataList.Select(x => x.Id).Distinct().ToList();

        terrainTileDataList = DataManager.GetTerrainTileData(searchParameters);
    }

    private static void GetTileSetData()
    {
        var searchParameters = new Search.TileSet();
        searchParameters.id = regionDataList.Select(x => x.TileSetId).Distinct().ToList();

        tileSetDataList = DataManager.GetTileSetData(searchParameters);
    }

    private static void GetTileData()
    {
        var searchParameters = new Search.Tile();
        searchParameters.tileSetId = tileSetDataList.Select(x => x.Id).Distinct().ToList();

        tileDataList = DataManager.GetTileData(searchParameters);
    }

    public static void UpdateData(RegionElementData elementData, DataRequest dataRequest)
    {
        var data = Fixtures.regionList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedChapterRegionId)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.ChapterRegionId = elementData.ChapterRegionId;
            else { }
        }

        if (elementData.ChangedPhaseId)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PhaseId = elementData.PhaseId;
            else { }
        }

        if (elementData.ChangedTileSetId)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.TileSetId = elementData.TileSetId;
            else { }
        }

        if (elementData.ChangedName)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.Name = elementData.Name;
            else { }
        }

        if (elementData.ChangedRegionSize)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.RegionSize = elementData.RegionSize;
            else { }
        }

        if (elementData.ChangedTerrainSize)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.TerrainSize = elementData.TerrainSize;
            else { }
        }
    }

    static public void UpdateIndex(RegionElementData elementData)
    {
        var data = Fixtures.regionList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;
    }
}
