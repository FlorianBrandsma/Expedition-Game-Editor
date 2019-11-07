using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RegionDataManager
{
    private RegionController regionController;
    private List<RegionData> regionDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.TileSetData> tileSetDataList;

    public RegionDataManager(RegionController regionController)
    {
        this.regionController = regionController;
    }

    public List<IDataElement> GetRegionDataElements(IEnumerable searchParameters)
    {
        var searchRegion = searchParameters.Cast<Search.Region>().FirstOrDefault();

        GetRegionData(searchRegion);
        GetTileSetData();

        var list = (from regionData in regionDataList
                    join tileSetData in tileSetDataList on regionData.tileSetId equals tileSetData.Id
                    select new RegionDataElement()
                    {
                        DataType = Enums.DataType.Region,

                        Id = regionData.Id,
                        Index = regionData.Index,

                        ChapterRegionId = regionData.chapterRegionId,
                        PhaseId = regionData.phaseId,
                        TileSetId = regionData.tileSetId,
                        Name = regionData.name,
                        RegionSize = regionData.regionSize,
                        TerrainSize = regionData.terrainSize,

                        type = regionController.regionType

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
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
