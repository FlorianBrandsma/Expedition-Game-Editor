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

        if (searchParameters.includeAddElement)
            regionDataList.Add(DefaultData(searchParameters));

        if (searchParameters.includeRemoveElement)
            regionDataList.Add(new RegionBaseData());

        if (regionDataList.Count == 0) return new List<IElementData>();
        
        GetTerrainData();
        GetTerrainTileData();

        GetTileSetData();
        GetTileData();

        var list = (from regionData in regionDataList

                    join leftJoin in (from tileSetData in tileSetDataList

                                      join leftJoin in (from tileData in tileDataList
                                                        select new { tileData }) on tileSetData.Id equals leftJoin.tileData.TileSetId into tileData

                                      select new { tileSetData, tileData }) on regionData.TileSetId equals leftJoin.tileSetData.Id into tileSetData

                    
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

                        TileSize = tileSetData.FirstOrDefault() != null ? tileSetData.FirstOrDefault().tileSetData.TileSize : 0,
                        TileIconPath = tileSetData.FirstOrDefault() != null ? tileSetData.FirstOrDefault().tileData.First().tileData.IconPath : "",

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
                        
                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Index).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static RegionBaseData DefaultData(Search.Region searchParameters)
    {
        return new RegionBaseData()
        {
            TileSetId = 1,
            RegionSize = 1,
            TerrainSize = 1
        };
    }

    private static void SetDefaultAddValues(List<RegionElementData> list)
    {
        var addElementData = list.Where(x => x.Id == 0).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;

        addElementData.Index = list.Count - 1;
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

    public static void AddData(RegionElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.regionList.Count > 0 ? (Fixtures.regionList[Fixtures.regionList.Count - 1].Id + 1) : 1;
            Fixtures.regionList.Add(((RegionData)elementData).Clone());

            var tileSearchParameters = new Search.Tile()
            {
                tileSetId = new List<int>() { elementData.TileSetId }
            };

            var tileData = DataManager.GetTileData(tileSearchParameters);

            for (int region = 0; region < elementData.RegionSize * elementData.RegionSize; region++)
            {
                var terrainElementData = new TerrainElementData()
                {
                    RegionId = elementData.Id,
                    IconId = 1,

                    Index = region,
                };

                terrainElementData.Add(dataRequest);

                var atmosphereElementData = new AtmosphereElementData()
                {
                    TerrainId = terrainElementData.Id,

                    Default = true
                };

                atmosphereElementData.Add(dataRequest);

                for (int terrain = 0; terrain < elementData.TerrainSize * elementData.TerrainSize; terrain++)
                {
                    var terrainTileElementData = new TerrainTileElementData()
                    {
                        TerrainId = terrainElementData.Id,
                        TileId = tileData.FirstOrDefault().Id,

                        Index = terrain
                    };

                    terrainTileElementData.Add(dataRequest);
                }
            }
        }
        else { }
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

    public static void RemoveData(RegionElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.regionList.RemoveAll(x => x.Id == elementData.Id);
        }
        else { }
    }

    static public void UpdateIndex(RegionElementData elementData)
    {
        var data = Fixtures.regionList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;
    }
}
