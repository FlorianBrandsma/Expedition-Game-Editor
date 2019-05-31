using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PartyElementDataManager
{
    private PartyElementController partyElementController;
    private List<PartyElementData> partyElementDataList = new List<PartyElementData>();

    private DataManager dataManager = new DataManager();

    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.ElementData> elementDataList;

    public void InitializeManager(PartyElementController partyElementController)
    {
        this.partyElementController = partyElementController;
    }

    public List<IDataElement> GetPartyElementDataElements(IEnumerable searchParameters)
    {
        var partyElementSearchData = searchParameters.Cast<Search.PartyElement>().FirstOrDefault();

        switch (partyElementSearchData.requestType)
        {
            case Search.PartyElement.RequestType.Custom:

                GetCustomPartyElementData(partyElementSearchData);
                break;
        }

        if (partyElementDataList.Count == 0) return new List<IDataElement>();

        GetElementData();
        GetObjectGraphicData();

        var list = (from partyElementData in partyElementDataList
                    join elementData in elementDataList on partyElementData.elementId equals elementData.id
                    join objectGraphicData in objectGraphicDataList on elementData.objectGraphicId equals objectGraphicData.id
                    select new PartyElementDataElement()
                    {
                        id = partyElementData.id,
                        table = "PartyElement",

                        ChapterId = partyElementData.chapterId,
                        ElementId = partyElementData.elementId,

                        elementName = elementData.name,
                        objectGraphicIcon = objectGraphicData.icon

                    }).OrderBy(x => x.id).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    internal void GetCustomPartyElementData(Search.PartyElement searchParameters)
    {
        partyElementDataList = new List<PartyElementData>();

        foreach (Fixtures.PartyElement partyElement in Fixtures.partyElementList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(partyElement.id)) continue;
            if (searchParameters.chapterId.Count > 0 && !searchParameters.chapterId.Contains(partyElement.chapterId)) continue;

            var partyElementData = new PartyElementData();

            partyElementData.id = partyElement.id;

            partyElementData.chapterId = partyElement.chapterId;
            partyElementData.elementId = partyElement.elementId;

            partyElementDataList.Add(partyElementData);
        }
    }

    internal void GetElementData()
    {
        elementDataList = dataManager.GetElementData(partyElementDataList.Select(x => x.elementId).Distinct().ToList(), true);
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicDataList = dataManager.GetObjectGraphicData(elementDataList.Select(x => x.objectGraphicId).Distinct().ToList(), true);
    }

    internal class PartyElementData : GeneralData
    {
        public int chapterId;
        public int elementId;
    }
}
