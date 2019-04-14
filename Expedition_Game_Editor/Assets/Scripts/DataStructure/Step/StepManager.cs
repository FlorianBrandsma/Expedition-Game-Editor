using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class StepManager
{
    private StepController dataController;
    private List<StepData> stepData_list;

    public List<StepDataElement> GetStepDataElements(StepController dataController)
    {
        this.dataController = dataController;

        GetStepData();
        //GetIconData()?

        var list = (from oCore in stepData_list
                    select new StepDataElement()
                    {
                        id = oCore.id,
                        table = oCore.table,
                        type = oCore.type,
                        index = oCore.index,
                        name = oCore.name,

                        icon = "Textures/Characters/1"

                    }).OrderBy(x => x.index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetStepData()
    {
        stepData_list = new List<StepData>();

        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var stepData = new StepData();

            stepData.id = (i + 1);
            stepData.table = "Step";
            stepData.index = i;

            stepData.name = "Step " + (i + 1);

            stepData_list.Add(stepData);
        }
    }

    internal class StepData : GeneralData
    {
        public int index;
        public string name;
    }
}
