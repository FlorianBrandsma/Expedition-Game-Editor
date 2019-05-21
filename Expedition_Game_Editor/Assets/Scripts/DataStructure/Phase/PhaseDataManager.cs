using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PhaseDataManager
{
    private PhaseController phaseController;
    private List<PhaseData> phaseDataList;

    public void InitializeManager(PhaseController phaseController)
    {
        this.phaseController = phaseController;
    }

    public List<PhaseDataElement> GetPhaseDataElements(Search.Phase searchParameters)
    {
        switch(searchParameters.requestType)
        {
            case Search.Phase.RequestType.Custom:               GetCustomPhaseData(searchParameters); break;
            case Search.Phase.RequestType.GetPhaseWithQuests:   GetPhaseWithQuests(searchParameters); break;
            default: Debug.Log("CASE MISSING"); break;
        }

        var list = (from phaseData in phaseDataList
                    select new PhaseDataElement()
                    {
                        id = phaseData.id,
                        table = phaseData.table,

                        Index = phaseData.index,
                        Name = phaseData.name,
                        Notes = phaseData.notes,

                        icon = "Textures/Characters/1"

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    private void GetCustomPhaseData(Search.Phase searchParameters)
    {
        phaseDataList = new List<PhaseData>();

        foreach(Fixtures.Phase phase in Fixtures.phaseList)
        {
            var phaseData = new PhaseData();

            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(phase.id)) continue;
            
            phaseData.id = phase.id;
            phaseData.table = "Phase";
            phaseData.index = phase.index;

            if (searchParameters.chapterId.Count > 0 && !searchParameters.chapterId.Contains(phase.chapterId)) continue;

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
