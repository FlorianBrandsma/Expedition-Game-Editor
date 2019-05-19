using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseRegionDataManager
{
    private PhaseRegionController phaseRegionController;
    private List<PhaseRegionData> phaseRegionDataList;

    public void InitializeManager(PhaseRegionController phaseRegionController)
    {
        this.phaseRegionController = phaseRegionController;
    }

    public List<PhaseRegionDataElement> GetPhaseRegionDataElements(IEnumerable searchParameters)
    {
        var phaseRegionSearchData = searchParameters.Cast<Search.PhaseRegion>().FirstOrDefault();

        GetPhaseRegionData(phaseRegionSearchData);

        var list = (from phaseRegionData in phaseRegionDataList
                    select new PhaseRegionDataElement()
                    {
                        id = phaseRegionData.id,
                        table = phaseRegionData.table,

                        Index = phaseRegionData.index,
                        Name = phaseRegionData.name

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetPhaseRegionData(Search.PhaseRegion searchParameters)
    {
        phaseRegionDataList = new List<PhaseRegionData>();

        for (int i = 0; i < searchParameters.temp_id_count; i++)
        {
            var phaseRegionData = new PhaseRegionData();

            phaseRegionData.id = (i + 1);
            phaseRegionData.table = "PhaseRegion";
            phaseRegionData.index = i;

            phaseRegionData.name = "PhaseRegion " + (i + 1);

            phaseRegionDataList.Add(phaseRegionData);
        }
    }

    internal class PhaseRegionData : GeneralData
    {
        public int index;
        public string name;
    }
}
