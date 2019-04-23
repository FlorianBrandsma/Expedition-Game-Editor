using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ElementManager
{
    private ElementController dataController;
    private List<ElementData> elementData_list;

    private List<DataManager.ObjectGraphicData> objectGraphicData_list;

    private DataManager dataManager = new DataManager();

    public List<ElementDataElement> GetElementDataElements(ElementController dataController)
    {
        this.dataController = dataController;

        GetElementData();
        GetObjectGraphicData();

        var list = (from elementData in elementData_list
                    join objectGraphicData in objectGraphicData_list on elementData.object_id equals objectGraphicData.id
                    select new ElementDataElement()
                    {
                        id = elementData.id,
                        table = elementData.table,
                        type = elementData.type,
                        index = elementData.index,

                        name = elementData.name,

                        object_name = objectGraphicData.name,
                        icon = objectGraphicData.icon

                    }).OrderBy(x => x.index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    internal void GetElementData()
    {
        elementData_list = new List<ElementData>();

        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var elementData = new ElementData();

            elementData.id = (i + 1);
            elementData.table = "Element";
            elementData.index = i;

            elementData.object_id = (i % 2);
            elementData.name = "Element " + (i + 1);

            elementData_list.Add(elementData);
        }
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicData_list = dataManager.GetObjectGraphicData(elementData_list.Select(x => x.object_id).Distinct().ToList(), false);
    }

    internal class ElementData : GeneralData
    {
        public int index;
        public int object_id;
        public string name;
    }
}
