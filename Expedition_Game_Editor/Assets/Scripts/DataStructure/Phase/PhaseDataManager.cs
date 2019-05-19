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
                        Description = phaseData.description,

                        icon = "Textures/Characters/1"

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    private void GetCustomPhaseData(Search.Phase searchParameters)
    {
        phaseDataList = new List<PhaseData>();

        int index = 0;

        for (int i = 0; i < searchParameters.temp_id_count; i++)
        {
            var phaseData = new PhaseData();

            var id = (i + 1);

            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(id)) continue;

            var chapterId = (i % 2) + 1;

            if (searchParameters.chapterId.Count > 0 && !searchParameters.chapterId.Contains(chapterId)) continue;

            phaseData.id = (i + 1);
            phaseData.table = "Phase";
            phaseData.index = index;
            index++;

            phaseData.chapterId = chapterId;
            phaseData.name = "Phase " + index;
            phaseData.description = "This is definitely a test";

            phaseDataList.Add(phaseData);
        }
    }

    private void GetPhaseWithQuests(Search.Phase searchParameters)
    {
        
    }

    internal class PhaseData : GeneralData
    {
        public int index;
        public int chapterId;
        public string name;
        public string description;
    }
}
