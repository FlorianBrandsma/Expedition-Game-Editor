using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InteractableSaveDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<InteractableSaveData> interactableSaveDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    public InteractableSaveDataManager(IDataController dataController)
    {
        DataController = dataController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.InteractableSave>().First();

        GetInteractableSaveData(searchParameters);

        if (interactableSaveDataList.Count == 0) return new List<IElementData>();

        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        var list = (from interactableSaveData   in interactableSaveDataList
                    join interactableData       in interactableDataList     on interactableSaveData.interactableId  equals interactableData.Id
                    join objectGraphicData      in objectGraphicDataList    on interactableData.objectGraphicId     equals objectGraphicData.Id
                    join iconData               in iconDataList             on objectGraphicData.iconId             equals iconData.Id
                    select new InteractableSaveElementData()
                    {
                        Id = interactableData.Id,
                        Index = interactableData.Index,

                        objectGraphicId = interactableData.objectGraphicId,

                        interactableName = interactableData.name,

                        health = interactableData.health,
                        hunger = interactableData.hunger,
                        thirst = interactableData.thirst,

                        weight = interactableData.weight,
                        speed = interactableData.speed,
                        stamina = interactableData.stamina,

                        objectGraphicPath = objectGraphicData.path,
                        objectGraphicIconPath = iconData.path

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public void GetInteractableSaveData(Search.InteractableSave searchParameters)
    {
        interactableSaveDataList = new List<InteractableSaveData>();

        foreach (Fixtures.InteractableSave interactableSave in Fixtures.interactableSaveList)
        {
            if (searchParameters.saveId.Count > 0 && !searchParameters.saveId.Contains(interactableSave.saveId)) continue;

            var interactableData = Fixtures.interactableList.Where(x => x.Id == interactableSave.interactableId).First();

            if (searchParameters.type.Count > 0 && !searchParameters.type.Contains(interactableData.type)) continue;

            var interactableSaveData = new InteractableSaveData();

            interactableSaveData.Id = interactableSave.Id;
            interactableSaveData.Index = interactableSave.Index;

            interactableSaveData.saveId = interactableSave.saveId;
            interactableSaveData.interactableId = interactableSave.interactableId;

            interactableSaveDataList.Add(interactableSaveData);
        }
    }

    internal void GetInteractableData()
    {
        var searchParameters = new Search.Interactable();
        searchParameters.id = interactableSaveDataList.Select(x => x.interactableId).Distinct().ToList();

        interactableDataList = dataManager.GetInteractableData(searchParameters);
    }

    internal void GetObjectGraphicData()
    {
        var searchParameters = new Search.ObjectGraphic();
        searchParameters.id = interactableDataList.Select(x => x.objectGraphicId).Distinct().ToList();

        objectGraphicDataList = dataManager.GetObjectGraphicData(searchParameters);
    }

    internal void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = objectGraphicDataList.Select(x => x.iconId).Distinct().ToList();

        iconDataList = dataManager.GetIconData(searchParameters);
    }

    internal class InteractableSaveData : GeneralData
    {
        public int saveId;
        public int interactableId;
    }
}
