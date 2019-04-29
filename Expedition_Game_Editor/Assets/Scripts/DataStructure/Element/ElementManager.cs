using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ElementManager
{
    private ElementController dataController;
    private List<ElementData> elementDataList;

    private List<DataManager.ObjectGraphicData> objectGraphicData_list;

    private DataManager dataManager = new DataManager();

    public List<ElementDataElement> GetElementDataElements(ElementController dataController)
    {
        this.dataController = dataController;

        GetElementData();
        GetObjectGraphicData();

        var list = (from elementData in elementDataList
                    join objectGraphicData in objectGraphicData_list on elementData.objectGraphicId equals objectGraphicData.id
                    select new ElementDataElement()
                    {
                        id      = elementData.id,
                        table   = elementData.table,
                        type    = elementData.type,
                        index   = elementData.index,

                        objectGraphicId = elementData.objectGraphicId,
                        name    = elementData.name,

                        objectName = objectGraphicData.name,
                        icon    = objectGraphicData.icon

                    }).OrderBy(x => x.index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    internal void GetElementData()
    {
        elementDataList = new List<ElementData>();

        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var elementData = new ElementData();

            elementData.id      = (i + 1);
            elementData.table   = "Element";
            elementData.index   = i;

            elementData.objectGraphicId = (i % 2);
            elementData.name    = "Element " + (i + 1);

            elementDataList.Add(elementData);
        }
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicData_list = dataManager.GetObjectGraphicData(elementDataList.Select(x => x.objectGraphicId).Distinct().ToList(), false);
    }

    internal class ElementData : GeneralData
    {
        public int index;
        public int objectGraphicId;
        public string name;
    }
}
