using UnityEngine;
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

    public List<IDataElement> GetDataElements(IEnumerable searchParameters)
    {
        var worldInteractableSearchData = searchParameters.Cast<Search.WorldInteractable>().FirstOrDefault();

        switch (worldInteractableSearchData.requestType)
        {
            case Search.WorldInteractable.RequestType.Custom:

                GetCustomWorldInteractableData(worldInteractableSearchData);
                break;

            case Search.WorldInteractable.RequestType.GetQuestAndObjectiveInteractables:

                GetQuestAndObjectiveWorldInteractableData(worldInteractableSearchData);
                break;

            case Search.WorldInteractable.RequestType.GetInteractablesFromInteractionRegion:

                GetElementsFromInteractionRegion(worldInteractableSearchData);
                break;
        }

        if (worldInteractableDataList.Count == 0) return new List<IDataElement>();

        DataManager.SetIndex(worldInteractableDataList.Cast<GeneralData>().ToList());

        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        var list = (from worldInteractableData  in worldInteractableDataList
                    join interactableData       in interactableDataList     on worldInteractableData.interactableId equals interactableData.Id
                    join objectGraphicData      in objectGraphicDataList    on interactableData.objectGraphicId     equals objectGraphicData.Id
                    join iconData               in iconDataList             on objectGraphicData.iconId             equals iconData.Id
                    select new WorldInteractableDataElement()
                    {
                        Id = worldInteractableData.Id,
                        Index = worldInteractableData.Index,
                        
                        ChapterId = worldInteractableData.chapterId,
                        InteractableId = worldInteractableData.interactableId,

                        interactableName = interactableData.name,
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
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(worldInteractable.Id)) continue;
            if (searchParameters.chapterId.Count > 0 && !searchParameters.chapterId.Contains(worldInteractable.chapterId)) continue;
            if (searchParameters.objectiveId.Count > 0 && !searchParameters.objectiveId.Contains(worldInteractable.objectiveId)) continue;

            var worldInteractableData = new WorldInteractableData();

            worldInteractableData.Id = worldInteractable.Id;

            worldInteractableData.chapterId = worldInteractable.chapterId;
            worldInteractableData.objectiveId = worldInteractable.objectiveId;
            worldInteractableData.interactableId = worldInteractable.interactableId;

            worldInteractableDataList.Add(worldInteractableData);
        }
    }

    internal void GetQuestAndObjectiveWorldInteractableData(Search.WorldInteractable searchParameters)
    {
        worldInteractableDataList = new List<WorldInteractableData>();
        
        List<int> worldInteractableIds = new List<int>();

        Fixtures.phaseInteractableList.Where(x => searchParameters.questId.Contains(x.questId)).Distinct().ToList().ForEach(x => worldInteractableIds.Add(x.worldInteractableId));
        Fixtures.worldInteractableList.Where(x => searchParameters.objectiveId.Contains(x.objectiveId)).Distinct().ToList().ForEach(x => worldInteractableIds.Add(x.Id));
        
        var worldInteractables = Fixtures.worldInteractableList.Where(x => worldInteractableIds.Contains(x.Id)).Distinct().ToList();
        
        foreach (Fixtures.WorldInteractable worldInteractable in worldInteractables)
        {
            var worldInteractableData = new WorldInteractableData();

            worldInteractableData.Id = worldInteractable.Id;

            worldInteractableData.chapterId = worldInteractable.chapterId;
            worldInteractableData.objectiveId = worldInteractable.objectiveId;
            worldInteractableData.interactableId = worldInteractable.interactableId;

            worldInteractableDataList.Add(worldInteractableData);
        }
    }

    internal void GetElementsFromInteractionRegion(Search.WorldInteractable searchParameters)
    {
        worldInteractableDataList = new List<WorldInteractableData>();

        var interactionList = Fixtures.interactionList.Where(x => searchParameters.regionId.Contains(x.regionId)).Distinct().ToList();
        var taskList = Fixtures.taskList.Where(x => interactionList.Select(y => y.taskId).Contains(x.Id) && searchParameters.objectiveId.Contains(x.objectiveId)).Distinct().ToList();
        var worldInteractableList = Fixtures.worldInteractableList.Where(x => taskList.Select(y => y.worldInteractableId).Contains(x.Id)).Distinct().ToList();

        foreach (Fixtures.WorldInteractable worldInteractable in worldInteractableList)
        {
            var worldInteractableData = new WorldInteractableData();

            worldInteractableData.Id = worldInteractable.Id;

            worldInteractableData.chapterId = worldInteractable.chapterId;
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
        public int chapterId;
        public int objectiveId;
        public int interactableId;
    }
}
