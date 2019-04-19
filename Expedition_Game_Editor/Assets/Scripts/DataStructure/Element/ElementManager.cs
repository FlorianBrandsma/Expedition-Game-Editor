using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ElementManager
{
    private ElementController dataController;
    private List<ElementData> elementData_list;

    public List<ElementDataElement> GetElementDataElements(ElementController dataController)
    {
        this.dataController = dataController;

        GetElementData();
        //GetIconData()?

        var list = (from oCore in elementData_list
                    select new ElementDataElement()
                    {
                        id = oCore.id,
                        table = oCore.table,
                        type = oCore.type,
                        index = oCore.index,
                        name = oCore.name,

                        icon = "Textures/Icons/Objects/0"

                    }).OrderBy(x => x.index).ToList();

        list.ForEach(x => x.ClearChanges());

        return list;
    }

    public void GetElementData()
    {
        elementData_list = new List<ElementData>();

        //if(dataController.search_by_id)
        //    Debug.Log(dataController.segmentController.generalData.table + ":" + dataController.search_by_id);

        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var elementData = new ElementData();

            elementData.id = (i + 1);
            elementData.table = "Element";
            elementData.index = i;

            elementData.name = "Element " + (i + 1);

            elementData_list.Add(elementData);
        }
    }

    internal class ElementData : GeneralData
    {
        public int index;
        public string name;
    }
}
