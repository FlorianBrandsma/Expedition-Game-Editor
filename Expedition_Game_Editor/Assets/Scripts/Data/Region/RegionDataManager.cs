using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RegionDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<RegionData> regionDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.TileSetData> tileSetDataList;
    private List<DataManager.TileData> tileDataList;

    public RegionDataManager(RegionController regionController)
    {
        DataController = regionController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var regionController = (RegionController)DataController;

        var searchParameters = searchProperties.searchParameters.Cast<Search.Region>().First();

        GetRegionData(searchParameters);

        if (regionDataList.Count == 0) return new List<IElementData>();

        GetTileSetData();
        GetTileData();

        var list = (from regionData in regionDataList
                    join tileSetData in tileSetDataList on regionData.tileSetId equals tileSetData.Id

                    join leftJoin in (from tileData in tileDataList
                                      select new { tileData }) on tileSetData.Id equals leftJoin.tileData.tileSetId into tileData

                    select new RegionElementData()
                    {
                        Id = regionData.Id,
                        Index = regionData.Index,

                        ChapterRegionId = regionData.chapterRegionId,
                        PhaseId = regionData.phaseId,
                        TileSetId = regionData.tileSetId,

                        Name = regionData.name,

                        RegionSize = regionData.regionSize,
                        TerrainSize = regionData.terrainSize,

                        type = regionController.regionType,

                        tileSize = tileSetData.tileSize,
                        tileIconPath = tileData.First().tileData.iconPath
                        
                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public void GetRegionData(Search.Region searchParameters)
    {
        regionDataList = new List<RegionData>();

        foreach(Fixtures.Region region in Fixtures.regionList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(region.Id)) continue;
            if (searchParameters.phaseId.Count > 0 && !searchParameters.phaseId.Contains(region.phaseId)) continue;

            var regionData = new RegionData();

            regionData.Id = region.Id;
            regionData.Index = region.Index;

            regionData.chapterRegionId = region.chapterRegionId;
            regionData.phaseId = region.phaseId;
            regionData.tileSetId = region.tileSetId;
            regionData.name = region.name;
            regionData.regionSize = region.regionSize;
            regionData.terrainSize = region.terrainSize;

            regionDataList.Add(regionData);
        }
    }

    private void GetTileSetData()
    {
        var tileSetSearchParameters = new Search.TileSet();
        tileSetSearchParameters.id = regionDataList.Select(x => x.tileSetId).Distinct().ToList();

        tileSetDataList = dataManager.GetTileSetData(tileSetSearchParameters);
    }

    private void GetTileData()
    {
        var tileSearchParameters = new Search.Tile();
        tileSearchParameters.tileSetId = tileSetDataList.Select(x => x.Id).Distinct().ToList();

        tileDataList = dataManager.GetTileData(tileSearchParameters);
    }

    internal class RegionData : GeneralData
    {
        public int chapterRegionId;
        public int phaseId;
        public int tileSetId;
        public string name;
        public int regionSize;
        public int terrainSize;
    }
}
