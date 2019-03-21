using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PhaseManager
{
    private PhaseController dataController;
    private List<PhaseData> phaseData_list;

    public List<PhaseDataElement> GetPhaseDataElements(PhaseController dataController)
    {
        this.dataController = dataController;

        GetPhaseData();

        var list = (from oCore in phaseData_list
                    select new PhaseDataElement()
                    {
                        id = oCore.id,
                        table = oCore.table,
                        type = oCore.type,
                        index = oCore.index,
                        name = oCore.name,
                        description = oCore.description,

                        icon = "Textures/Characters/1"

                    }).OrderBy(x => x.index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetPhaseData()
    {
        phaseData_list = new List<PhaseData>();

        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var phaseData = new PhaseData();

            phaseData.id = (i + 1);
            phaseData.table = "Phase";
            phaseData.index = i;

            phaseData.name = "Phase " + (i + 1);
            phaseData.description = "This is definitely a test"; //"This is a pretty regular sentence. The structure is something you'd expect. Nothing too long though!";

            phaseData_list.Add(phaseData);
        }
    }

    internal class PhaseData : GeneralData
    {
        public int index;
        public string name;
        public string description;
    }
}
