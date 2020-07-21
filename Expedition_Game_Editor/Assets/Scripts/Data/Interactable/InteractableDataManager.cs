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

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Interactable>().First();
        
        GetInteractableData(searchParameters);

        if (interactableDataList.Count == 0) return new List<IElementData>();

        GetObjectGraphicData();
        GetIconData();

        var list = (from interactableData   in interactableDataList
                    join objectGraphicData  in objectGraphicDataList    on interactableData.objectGraphicId equals objectGraphicData.Id
                    join iconData           in iconDataList             on objectGraphicData.iconId         equals iconData.Id
                    select new InteractableElementData()
                    {
                        Id = interactableData.Id,
                        Index = interactableData.Index,

                        Type = interactableData.type,

                        ObjectGraphicId = interactableData.objectGraphicId,

                        Name = interactableData.name,
                        
                        Health = interactableData.health,
                        Hunger = interactableData.hunger,
                        Thirst = interactableData.thirst,

                        Weight = interactableData.weight,
                        Speed = interactableData.speed,
                        Stamina = interactableData.stamina,
                        
                        objectGraphicPath = objectGraphicData.path,
                        objectGraphicIconPath = iconData.path

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    internal void GetInteractableData(Search.Interactable searchParameters)
    {
        interactableDataList = new List<InteractableData>();
        
        foreach(Fixtures.Interactable interactable in Fixtures.interactableList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(interactable.Id)) continue;
            if (searchParameters.type.Count > 0 && !searchParameters.type.Contains(interactable.type)) continue;

            var interactableData = new InteractableData();

            interactableData.Id = interactable.Id;
            interactableData.Index = interactable.Index;

            interactableData.type = interactable.type;

            interactableData.objectGraphicId = interactable.objectGraphicId;

            interactableData.name = interactable.name;

            interactableData.health = interactable.health;
            interactableData.hunger = interactable.hunger;
            interactableData.thirst = interactable.thirst;

            interactableData.weight = interactable.weight;
            interactableData.speed = interactable.speed;
            interactableData.stamina = interactable.stamina;
            
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
        public int type;

        public int objectGraphicId;

        public string name;

        public int health;
        public int hunger;
        public int thirst;

        public float weight;
        public float speed;
        public float stamina;
    }
}
