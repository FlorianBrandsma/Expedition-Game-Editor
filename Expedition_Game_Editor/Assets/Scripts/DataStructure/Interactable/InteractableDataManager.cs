using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InteractableDataManager
{
    private InteractableController interactableController;

    private List<InteractableData> interactableDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    public void InitializeManager(InteractableController elementController)
    {
        this.interactableController = elementController;
    }

    public List<IDataElement> GetInteractableDataElements(IEnumerable searchParameters)
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
                    join objectGraphicData in objectGraphicDataList on interactableData.objectGraphicId equals objectGraphicData.id
                    join iconData in iconDataList on objectGraphicData.iconId equals iconData.id
                    select new InteractableDataElement()
                    {
                        dataType = Enums.DataType.Interactable,

                        id      = interactableData.id,
                        index   = interactableData.index,

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
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(interactable.id)) continue;

            var interactableData = new InteractableData();

            interactableData.id = interactable.id;
            interactableData.index = interactable.index;

            interactableData.objectGraphicId = interactable.objectGraphicId;
            interactableData.name = interactable.name;

            interactableDataList.Add(interactableData);
        }
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicDataList = dataManager.GetObjectGraphicData(interactableDataList.Select(x => x.objectGraphicId).Distinct().ToList(), true);
    }

    internal void GetIconData()
    {
        iconDataList = dataManager.GetIconData(objectGraphicDataList.Select(x => x.iconId).Distinct().ToList(), true);
    }

    internal class InteractableData : GeneralData
    {
        public int objectGraphicId;
        public string name;
    }
}
