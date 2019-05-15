using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PhaseManager
{
    private PhaseController dataController;
    private List<PhaseData> phaseData_list;

    public List<PhaseDataElement> GetPhaseDataElements(Search.Phase searchParameters)
    {
        GetPhaseData(searchParameters);

        var list = (from phaseData in phaseData_list
                    select new PhaseDataElement()
                    {
                        id = phaseData.id,
                        table = phaseData.table,
                        type = phaseData.type,

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
        phaseData_list = new List<PhaseData>();

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

            phaseData_list.Add(phaseData);
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
