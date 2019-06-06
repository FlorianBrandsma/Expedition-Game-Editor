using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ElementDataManager
{
    private ElementController elementController;

    private List<ElementData> elementDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    public void InitializeManager(ElementController elementController)
    {
        this.elementController = elementController;
    }

    public List<IDataElement> GetElementDataElements(IEnumerable searchParameters)
    {
        var elementSearchData = searchParameters.Cast<Search.Element>().FirstOrDefault();

        switch(elementSearchData.requestType)
        {  
            case Search.Element.RequestType.Custom:
                GetCustomElementData(elementSearchData);
                break;
        }

        GetObjectGraphicData();
        GetIconData();

        var list = (from elementData in elementDataList
                    join objectGraphicData in objectGraphicDataList on elementData.objectGraphicId equals objectGraphicData.id
                    join iconData in iconDataList on objectGraphicData.iconId equals iconData.id
                    select new ElementDataElement()
                    {
                        id      = elementData.id,
                        table   = elementData.table,
                        Index   = elementData.index,

                        ObjectGraphicId = elementData.objectGraphicId,
                        Name    = elementData.name,

                        objectGraphicPath = objectGraphicData.path,
                        objectGraphicIconPath = iconData.path

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    internal void GetCustomElementData(Search.Element searchParameters)
    {
        elementDataList = new List<ElementData>();
        
        foreach(Fixtures.Element element in Fixtures.elementList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(element.id)) continue;

            var elementData = new ElementData();

            elementData.id = element.id;
            elementData.table = "Element";
            elementData.index = element.index;

            elementData.objectGraphicId = element.objectGraphicId;
            elementData.name = element.name;

            elementDataList.Add(elementData);
        }
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicDataList = dataManager.GetObjectGraphicData(elementDataList.Select(x => x.objectGraphicId).Distinct().ToList(), true);
    }

    internal void GetIconData()
    {
        iconDataList = dataManager.GetIconData(objectGraphicDataList.Select(x => x.iconId).Distinct().ToList(), true);
    }

    internal class ElementData : GeneralData
    {
        public int objectGraphicId;
        public string name;
    }
}
