using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class ElementManager
{
    private List<ElementData> elementDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;

    private DataManager dataManager = new DataManager();

    public List<ElementDataElement> GetElementDataElements(List<int> idList)
    {
        GetElementData(idList);
        GetObjectGraphicData();

        var list = (from elementData in elementDataList
                    join objectGraphicData in objectGraphicDataList on elementData.objectGraphicId equals objectGraphicData.id
                    select new ElementDataElement()
                    {
                        id      = elementData.id,
                        table   = elementData.table,
                        type    = elementData.type,
                        Index   = elementData.index,

                        ObjectGraphicId = elementData.objectGraphicId,
                        Name    = elementData.name,

                        objectGraphicName = objectGraphicData.name,
                        objectGraphicIcon    = objectGraphicData.icon

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    internal void GetElementData(List<int> idList)
    {
        elementDataList = new List<ElementData>();

        //Temporary
        //For filtering out the polearm, just because (index matches id)
        var objectList = new List<int> { 0, 0, 2, 3, 4 };

        int index = 0;

        foreach(int id in idList)
        {
            var elementData = new ElementData();

            elementData.id = id;
            elementData.table = "Element";
            elementData.index = index;

            elementData.objectGraphicId = objectList[id];
            elementData.name = "Element " + (index + 1);

            elementDataList.Add(elementData);

            index++;
        }
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicDataList = dataManager.GetObjectGraphicData(elementDataList.Select(x => x.objectGraphicId).Distinct().ToList(), false);
    }

    internal class ElementData : GeneralData
    {
        public int index;
        public int objectGraphicId;
        public string name;
    }
}
