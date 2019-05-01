using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class RegionManager
{
    private RegionController dataController;
    private List<RegionData> regionData_list;

    public List<RegionDataElement> GetRegionDataElements(RegionController dataController)
    {
        this.dataController = dataController;

        GetRegionData();
        //GetIconData()?

        var list = (from oCore in regionData_list
                    select new RegionDataElement()
                    {
                        id = oCore.id,
                        table = oCore.table,
                        type = oCore.type,
                        index = oCore.index,
                        name = oCore.name

                    }).OrderBy(x => x.index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetRegionData()
    {
        regionData_list = new List<RegionData>();

        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var regionData = new RegionData();

            regionData.id = (i + 1);
            regionData.table = "Region";
            regionData.type = (int)dataController.regionType;
            regionData.index = i;

            regionData.name = "Region " + (i + 1);

            regionData_list.Add(regionData);
        }
    }

    internal class RegionData : GeneralData
    {
        public int index;
        public string name;
    }
}
