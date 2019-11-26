using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseDataManager
{
    private PhaseController phaseController;
    private List<PhaseData> phaseDataList;

    public PhaseDataManager(PhaseController phaseController)
    {
        this.phaseController = phaseController;
    }

    public List<IDataElement> GetPhaseDataElements(IEnumerable searchParameters)
    {
        var phaseSearchData = searchParameters.Cast<Search.Phase>().FirstOrDefault();

        switch (phaseSearchData.requestType)
        {
            case Search.Phase.RequestType.Custom:               GetCustomPhaseData(phaseSearchData); break;
            case Search.Phase.RequestType.GetPhaseWithQuests:   GetPhaseWithQuests(phaseSearchData); break;
            default: Debug.Log("CASE MISSING"); break;
        }

        var list = (from phaseData in phaseDataList
                    select new PhaseDataElement()
                    {
                        DataType = Enums.DataType.Phase,

                        Id = phaseData.Id,
                        Index = phaseData.Index,

                        ChapterId = phaseData.chapterId,

                        Name = phaseData.name,
                        Notes = phaseData.notes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    private void GetCustomPhaseData(Search.Phase searchParameters)
    {
        phaseDataList = new List<PhaseData>();

        foreach(Fixtures.Phase phase in Fixtures.phaseList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(phase.Id)) continue;
            if (searchParameters.chapterId.Count > 0 && !searchParameters.chapterId.Contains(phase.chapterId)) continue;

            var phaseData = new PhaseData();

            phaseData.Id = phase.Id;
            phaseData.Index = phase.Index;

            phaseData.chapterId = phase.chapterId;
            phaseData.name = phase.name;
            phaseData.notes = phase.notes;

            phaseDataList.Add(phaseData);
        }
    }

    private void GetPhaseWithQuests(Search.Phase searchParameters)
    {
        
    }

    internal class PhaseData : GeneralData
    {
        public int chapterId;
        public string name;
        public string notes;
    }
}
