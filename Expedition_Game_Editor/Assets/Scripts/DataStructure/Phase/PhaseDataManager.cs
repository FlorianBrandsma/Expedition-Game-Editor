using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PhaseDataManager
{
    private PhaseController dataController;
    private List<PhaseData> phaseDataList;

    public List<PhaseDataElement> GetPhaseDataElements(Search.Phase searchParameters)
    {
        GetPhaseData(searchParameters);

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

    public void GetPhaseData(Search.Phase searchParameters)
    {
        phaseDataList = new List<PhaseData>();

        //Temporary

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
            phaseData.description = "This is definitely a test"; //"This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";

            phaseDataList.Add(phaseData);
        }
    }

    internal class PhaseData : GeneralData
    {
        public int index;
        public int chapterId;
        public string name;
        public string description;
    }
}
