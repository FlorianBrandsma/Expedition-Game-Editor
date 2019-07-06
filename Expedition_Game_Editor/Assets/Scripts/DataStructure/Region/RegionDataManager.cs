using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RegionDataManager
{
    private RegionController regionController;
    private List<RegionData> regionDataList;

    public void InitializeManager(RegionController regionController)
    {
        this.regionController = regionController;
    }

    public List<IDataElement> GetRegionDataElements(IEnumerable searchParameters)
    {
        var searchRegion = searchParameters.Cast<Search.Region>().FirstOrDefault();

        GetRegionData(searchRegion);

        var list = (from regionData in regionDataList
                    select new RegionDataElement()
                    {
                        dataType = Enums.DataType.Region,

                        id = regionData.id,
                        index = regionData.index,

                        ChapterRegionId = regionData.chapterRegionId,
                        PhaseId = regionData.phaseId,
                        TileSetId = regionData.tileSetId,
                        Name = regionData.name,
                        RegionSize = regionData.regionSize,
                        TerrainSize = regionData.terrainSize,

                        type = regionController.regionType

                    }).OrderBy(x => x.index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetRegionData(Search.Region searchParameters)
    {
        regionDataList = new List<RegionData>();

        int index = 0;
        
        foreach(Fixtures.Region region in Fixtures.regionList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(region.id)) continue;
            if (searchParameters.phaseId.Count > 0 && !searchParameters.phaseId.Contains(region.phaseId)) continue;

            var regionData = new RegionData();

            regionData.id = region.id;
            regionData.index = region.index;

            regionData.chapterRegionId = region.chapterRegionId;
            regionData.phaseId = region.phaseId;
            regionData.tileSetId = region.tileSetId;
            regionData.name = region.name;
            regionData.regionSize = region.regionSize;
            regionData.terrainSize = region.terrainSize;

            regionDataList.Add(regionData);

            index++;
        }
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
