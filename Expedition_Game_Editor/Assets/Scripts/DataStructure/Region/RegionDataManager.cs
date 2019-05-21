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

    public List<RegionDataElement> GetRegionDataElements(IEnumerable searchParameters)
    {
        var searchRegion = searchParameters.Cast<Search.Region>().FirstOrDefault();

        GetRegionData(searchRegion);

        var list = (from regionData in regionDataList
                    select new RegionDataElement()
                    {
                        id = regionData.id,
                        table = regionData.table,

                        Index = regionData.index,
                        Name = regionData.name,

                        type = (int)regionController.regionType

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetRegionData(Search.Region searchParameters)
    {
        regionDataList = new List<RegionData>();

        int index = 0;
        
        foreach(Fixtures.Region region in Fixtures.regionList)
        {
            var regionData = new RegionData();

            regionData.id = region.id;
            regionData.table = "Region";

            regionData.index = region.index;
            regionData.name = region.name;

            regionDataList.Add(regionData);

            index++;
        }
    }

    internal class RegionData : GeneralData
    {
        public string name;
    }
}
