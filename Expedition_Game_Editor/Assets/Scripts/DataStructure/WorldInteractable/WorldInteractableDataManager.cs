using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WorldInteractableDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<WorldInteractableData> worldInteractableDataList = new List<WorldInteractableData>();

    private DataManager dataManager = new DataManager();

    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    public WorldInteractableDataManager(IDataController dataController)
    {
        DataController = dataController;
    }

    public List<IDataElement> GetDataElements(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.WorldInteractable>().First();

        switch (searchParameters.requestType)
        {
            case Search.WorldInteractable.RequestType.Custom:

                GetCustomWorldInteractableData(searchParameters);
                break;

            case Search.WorldInteractable.RequestType.GetRegionWorldInteractables:

                GetRegionWorldInteractableData(searchParameters);
                break;
        }
        
        if (worldInteractableDataList.Count == 0) return new List<IDataElement>();

        DataManager.SetIndex(worldInteractableDataList.Cast<GeneralData>().ToList());

        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        var list = (from worldInteractableData  in worldInteractableDataList
                    join interactableData       in interactableDataList     on worldInteractableData.interactableId     equals interactableData.Id
                    join objectGraphicData      in objectGraphicDataList    on interactableData.objectGraphicId         equals objectGraphicData.Id
                    join iconData               in iconDataList             on objectGraphicData.iconId                 equals iconData.Id

                    select new WorldInteractableDataElement()
                    {
                        Id = worldInteractableData.Id,
                        Index = worldInteractableData.Index,

                        Type = worldInteractableData.type,

                        InteractableId = worldInteractableData.interactableId,

                        interactableName =  interactableData.name,
                        objectGraphicIconPath = iconData.path,

                        height = objectGraphicData.height,
                        width = objectGraphicData.width,
                        depth = objectGraphicData.depth

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    internal void GetCustomWorldInteractableData(Search.WorldInteractable searchParameters)
    {
        worldInteractableDataList = new List<WorldInteractableData>();
        
        foreach (Fixtures.WorldInteractable worldInteractable in Fixtures.worldInteractableList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(worldInteractable.Id))                             continue;
            if (searchParameters.type.Count             > 0 && !searchParameters.type.Contains(worldInteractable.type))                         continue;
            if (searchParameters.objectiveId.Count      > 0 && !searchParameters.objectiveId.Contains(worldInteractable.objectiveId))           continue;
            if (searchParameters.interactableId.Count   > 0 && !searchParameters.interactableId.Contains(worldInteractable.interactableId))     continue;
            if (searchParameters.isDefault              > -1 && searchParameters.isDefault != Convert.ToInt32(worldInteractable.isDefault))     continue;

            var worldInteractableData = new WorldInteractableData();

            worldInteractableData.Id = worldInteractable.Id;

            worldInteractableData.type = worldInteractable.type;

            worldInteractableData.objectiveId = worldInteractable.objectiveId;
            worldInteractableData.interactableId = worldInteractable.interactableId;

            worldInteractableData.isDefault = worldInteractable.isDefault;

            worldInteractableDataList.Add(worldInteractableData);
        }
    }

    internal void GetRegionWorldInteractableData(Search.WorldInteractable searchParameters)
    {
        worldInteractableDataList = new List<WorldInteractableData>();

        var interactionList = Fixtures.interactionList.Where(x => searchParameters.regionId.Contains(x.regionId)).Distinct().ToList();
        var taskList = Fixtures.taskList.Where(x => interactionList.Select(y => y.taskId).Contains(x.Id) && searchParameters.objectiveId.Contains(x.objectiveId)).Distinct().ToList();
        var worldInteractableList = Fixtures.worldInteractableList.Where(x => taskList.Select(y => y.worldInteractableId).Contains(x.Id)).Distinct().ToList();

        foreach (Fixtures.WorldInteractable worldInteractable in worldInteractableList)
        {
            if (searchParameters.id.Count               > 0 && !searchParameters.id.Contains(worldInteractable.Id))                             continue;
            if (searchParameters.type.Count             > 0 && !searchParameters.type.Contains(worldInteractable.type))                         continue;
            if (searchParameters.objectiveId.Count      > 0 && !searchParameters.objectiveId.Contains(worldInteractable.objectiveId))           continue;
            if (searchParameters.interactableId.Count   > 0 && !searchParameters.interactableId.Contains(worldInteractable.interactableId))     continue;
            if (searchParameters.isDefault              > -1 && searchParameters.isDefault != Convert.ToInt32(worldInteractable.isDefault))     continue;

            var worldInteractableData = new WorldInteractableData();

            worldInteractableData.Id = worldInteractable.Id;

            worldInteractableData.type = worldInteractable.type;

            worldInteractableData.objectiveId = worldInteractable.objectiveId;
            worldInteractableData.interactableId = worldInteractable.interactableId;

            worldInteractableDataList.Add(worldInteractableData);
        }
    }

    internal void GetInteractableData()
    {
        var interactableSearchParameters = new Search.Interactable();

        interactableSearchParameters.id = worldInteractableDataList.Select(x => x.interactableId).Distinct().ToList();

        interactableDataList = dataManager.GetInteractableData(interactableSearchParameters);
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

    internal class WorldInteractableData : GeneralData
    {
        public int type;

        public int objectiveId;
        public int interactableId;

        public bool isDefault;
    }
}
