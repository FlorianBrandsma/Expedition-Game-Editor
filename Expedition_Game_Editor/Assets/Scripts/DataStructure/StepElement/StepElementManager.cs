using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class StepElementManager
{
    private StepElementController dataController;
    private List<StepElementData> stepElementData_list;

    public List<StepElementDataElement> GetStepElementDataElements(StepElementController dataController)
    {
        this.dataController = dataController;

        GetStepElementData();
        //GetIconData()?

        var list = (from oCore in stepElementData_list
                    select new StepElementDataElement()
                    {
                        id = oCore.id,
                        table = oCore.table,
                        type = oCore.type,
                        icon = "Textures/Icons/Objects/Nothing",
                        name = "<Name>"

                    }).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetStepElementData()
    {
        stepElementData_list = new List<StepElementData>();

        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var stepElementData = new StepElementData();

            stepElementData.id = (i + 1);
            stepElementData.table = "StepElement";

            stepElementData_list.Add(stepElementData);
        }
    }

    internal class StepElementData : GeneralData
    {
        
    }
}
