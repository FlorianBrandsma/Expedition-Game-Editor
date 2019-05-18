using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ElementDataManager
{
    private ElementController elementController;

    private List<ElementData> elementDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;

    private DataManager dataManager = new DataManager();

    public void InitializeManager(ElementController elementController)
    {
        this.elementController = elementController;
    }

    public List<ElementDataElement> GetElementDataElements(IEnumerable searchParameters)
    {
        var elementSearchData = searchParameters.Cast<Search.Element>().FirstOrDefault();

        switch(elementSearchData.requestType)
        {  
            case Search.Element.RequestType.Custom:
                GetCustomElementData(elementSearchData);
                break;
        }

        GetObjectGraphicData();

        var list = (from elementData in elementDataList
                    join objectGraphicData in objectGraphicDataList on elementData.objectGraphicId equals objectGraphicData.id
                    select new ElementDataElement()
                    {
                        id      = elementData.id,
                        table   = elementData.table,

                        Index   = elementData.index,
                        ObjectGraphicId = elementData.objectGraphicId,
                        Name    = elementData.name,

                        objectGraphicName = objectGraphicData.name,
                        objectGraphicIcon = objectGraphicData.icon

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    internal void GetCustomElementData(Search.Element searchParameters)
    {
        elementDataList = new List<ElementData>();

        //For filtering out the polearm, just because (index matches id)
        var objectList = new List<int> { 0, 2, 3, 4 };

        int index = 0;

        for(int i = 0; i < searchParameters.temp_id_count; i++)
        {
            var elementData = new ElementData();

            var id = (i + 1);

            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(id)) continue;

            elementData.id = id;
            elementData.table = "Element";
            elementData.index = index;

            elementData.objectGraphicId = objectList[i];
            elementData.name = "Element " + id;

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
