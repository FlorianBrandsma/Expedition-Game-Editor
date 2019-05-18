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
        
        for (int i = 0; i < searchParameters.temp_id_count; i++)
        {
            var regionData = new RegionData();

            int id = (i + 1);

            regionData.id = id;
            regionData.table = "Region";

            regionData.index = index;
            regionData.name = "Region " + id;

            regionDataList.Add(regionData);

            index++;
        }
    }

    internal class RegionData : GeneralData
    {
        public int index;
        public string name;
    }
}
