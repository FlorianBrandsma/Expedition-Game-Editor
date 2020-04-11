using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InteractableDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<InteractableData> interactableDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    public InteractableDataManager(InteractableController interactableController)
    {
        DataController = interactableController;
    }

    public List<IDataElement> GetDataElements(IEnumerable searchParameters)
    {
        var interactableSearchData = searchParameters.Cast<Search.Interactable>().FirstOrDefault();

        switch(interactableSearchData.requestType)
        {  
            case Search.Interactable.RequestType.Custom:
                GetCustomInteractableData(interactableSearchData);
                break;
        }

        GetObjectGraphicData();
        GetIconData();

        var list = (from interactableData in interactableDataList
                    join objectGraphicData in objectGraphicDataList on interactableData.objectGraphicId equals objectGraphicData.Id
                    join iconData in iconDataList on objectGraphicData.iconId equals iconData.Id
                    select new InteractableDataElement()
                    {
                        Id      = interactableData.Id,
                        Index   = interactableData.Index,

                        ObjectGraphicId = interactableData.objectGraphicId,
                        Name    = interactableData.name,

                        objectGraphicPath = objectGraphicData.path,
                        objectGraphicIconPath = iconData.path

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    internal void GetCustomInteractableData(Search.Interactable searchParameters)
    {
        interactableDataList = new List<InteractableData>();
        
        foreach(Fixtures.Interactable interactable in Fixtures.interactableList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(interactable.Id)) continue;

            var interactableData = new InteractableData();

            interactableData.Id = interactable.Id;
            interactableData.Index = interactable.Index;

            interactableData.objectGraphicId = interactable.objectGraphicId;
            interactableData.name = interactable.name;

            interactableDataList.Add(interactableData);
        }
    }

    internal void GetObjectGraphicData()
    {
        var objectGraphicSearchParameters = new Search.ObjectGraphic();

        objectGraphicSearchParameters.id = interactableDataList.Select(x => x.objectGraphicId).Distinct().ToList();

        objectGraphicDataList = dataManager.GetObjectGraphicData(objectGraphicSearchParameters);
    }

    internal void GetIconData()
    {
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = objectGraphicDataList.Select(x => x.iconId).Distinct().ToList();

        iconDataList = dataManager.GetIconData(iconSearchParameters);
    }

    internal class InteractableData : GeneralData
    {
        public int objectGraphicId;
        public string name;
    }
}
