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

    public List<IElementData> GetData(SearchProperties searchProperties)
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

            case Search.WorldInteractable.RequestType.GetQuestAndObjectiveWorldInteractables:

                GetQuestAndObjectiveWorldInteractableData(searchParameters);
                break;
        }
        
        if (worldInteractableDataList.Count == 0) return new List<IElementData>();

        //for(int i = 0; i < worldInteractableDataList.Count; i++)
        //{
        //    worldInteractableDataList[i].index = i;
        //}

        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        var list = (from worldInteractableData  in worldInteractableDataList
                    join interactableData       in interactableDataList     on worldInteractableData.interactableId     equals interactableData.id
                    join objectGraphicData      in objectGraphicDataList    on interactableData.objectGraphicId         equals objectGraphicData.id
                    join iconData               in iconDataList             on objectGraphicData.iconId                 equals iconData.id

                    select new WorldInteractableElementData()
                    {
                        Id = worldInteractableData.id,

                        Type = worldInteractableData.type,

                        PhaseId = worldInteractableData.phaseId,
                        QuestId = worldInteractableData.questId,
                        ObjectiveId = worldInteractableData.objectiveId,

                        ChapterInteractableId = worldInteractableData.chapterInteractableId,
                        InteractableId = worldInteractableData.interactableId,
                        
                        interactableName =  interactableData.name,
                        objectGraphicIconPath = iconData.path,

                        height = objectGraphicData.height,
                        width = objectGraphicData.width,
                        depth = objectGraphicData.depth

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    internal void GetCustomWorldInteractableData(Search.WorldInteractable searchParameters)
    {
        worldInteractableDataList = new List<WorldInteractableData>();
        
        foreach (Fixtures.WorldInteractable worldInteractable in Fixtures.worldInteractableList)
        {
            if (searchParameters.id.Count                       > 0 && !searchParameters.id.Contains(worldInteractable.id))                                         continue;
            if (searchParameters.type.Count                     > 0 && !searchParameters.type.Contains(worldInteractable.type))                                     continue;
            if (searchParameters.chapterInteractableId.Count    > 0 && !searchParameters.chapterInteractableId.Contains(worldInteractable.chapterInteractableId))   continue;
            if (searchParameters.phaseId.Count                  > 0 && !searchParameters.phaseId.Contains(worldInteractable.phaseId))                               continue;
            if (searchParameters.questId.Count                  > 0 && !searchParameters.questId.Contains(worldInteractable.questId))                               continue;
            if (searchParameters.objectiveId.Count              > 0 && !searchParameters.objectiveId.Contains(worldInteractable.objectiveId))                       continue;
            if (searchParameters.interactableId.Count           > 0 && !searchParameters.interactableId.Contains(worldInteractable.interactableId))                 continue;

            var worldInteractableData = new WorldInteractableData();

            worldInteractableData.id = worldInteractable.id;

            worldInteractableData.type = worldInteractable.type;
            
            worldInteractableData.phaseId = worldInteractable.phaseId;
            worldInteractableData.questId = worldInteractable.questId;
            worldInteractableData.objectiveId = worldInteractable.objectiveId;

            worldInteractableData.chapterInteractableId = worldInteractable.chapterInteractableId;
            worldInteractableData.interactableId = worldInteractable.interactableId;

            worldInteractableDataList.Add(worldInteractableData);
        }
    }

    internal void GetRegionWorldInteractableData(Search.WorldInteractable searchParameters)
    {
        worldInteractableDataList = new List<WorldInteractableData>();

        var interactionDestinationList  = Fixtures.interactionDestinationList.Where(x => searchParameters.regionId.Contains(x.regionId)).Distinct().ToList();
        var interactionList             = Fixtures.interactionList.Where(x => interactionDestinationList.Select(y => y.interactionId).Contains(x.id)).Distinct().ToList();
        var taskList                    = Fixtures.taskList.Where(x => interactionList.Select(y => y.taskId).Contains(x.id) && searchParameters.objectiveId.Contains(x.objectiveId)).Distinct().ToList();
        var worldInteractableList       = Fixtures.worldInteractableList.Where(x => taskList.Select(y => y.worldInteractableId).Contains(x.id)).Distinct().ToList();

        foreach (Fixtures.WorldInteractable worldInteractable in worldInteractableList)
        {
            if (searchParameters.id.Count                       > 0 && !searchParameters.id.Contains(worldInteractable.id))                                         continue;
            if (searchParameters.type.Count                     > 0 && !searchParameters.type.Contains(worldInteractable.type))                                     continue;
            if (searchParameters.chapterInteractableId.Count    > 0 && !searchParameters.chapterInteractableId.Contains(worldInteractable.chapterInteractableId))   continue;
            if (searchParameters.phaseId.Count                  > 0 && !searchParameters.phaseId.Contains(worldInteractable.phaseId))                               continue;
            if (searchParameters.questId.Count                  > 0 && !searchParameters.questId.Contains(worldInteractable.questId))                               continue;
            if (searchParameters.objectiveId.Count              > 0 && !searchParameters.objectiveId.Contains(worldInteractable.objectiveId))                       continue;
            if (searchParameters.interactableId.Count           > 0 && !searchParameters.interactableId.Contains(worldInteractable.interactableId))                 continue;

            var worldInteractableData = new WorldInteractableData();

            worldInteractableData.id = worldInteractable.id;

            worldInteractableData.type = worldInteractable.type;
            
            worldInteractableData.phaseId = worldInteractable.phaseId;
            worldInteractableData.questId = worldInteractable.questId;
            worldInteractableData.objectiveId = worldInteractable.objectiveId;

            worldInteractableData.chapterInteractableId = worldInteractable.chapterInteractableId;
            worldInteractableData.interactableId = worldInteractable.interactableId;

            worldInteractableDataList.Add(worldInteractableData);
        }
    }

    internal void GetQuestAndObjectiveWorldInteractableData(Search.WorldInteractable searchParameters)
    {
        worldInteractableDataList = new List<WorldInteractableData>();

        var worldInteractableList = new List<Fixtures.WorldInteractable>();

        Fixtures.worldInteractableList.Where(x => searchParameters.questId.Contains(x.questId)).Distinct().ToList().ForEach(x => worldInteractableList.Add(x));
        Fixtures.worldInteractableList.Where(x => searchParameters.objectiveId.Contains(x.objectiveId)).Distinct().ToList().ForEach(x => worldInteractableList.Add(x));

        foreach (Fixtures.WorldInteractable worldInteractable in worldInteractableList)
        {
            var worldInteractableData = new WorldInteractableData();

            worldInteractableData.id = worldInteractable.id;

            worldInteractableData.type = worldInteractable.type;

            worldInteractableData.phaseId = worldInteractable.phaseId;
            worldInteractableData.questId = worldInteractable.questId;
            worldInteractableData.objectiveId = worldInteractable.objectiveId;

            worldInteractableData.chapterInteractableId = worldInteractable.chapterInteractableId;
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

    internal class WorldInteractableData
    {
        public int id;

        public int type;
        
        public int phaseId;
        public int questId;
        public int objectiveId;

        public int chapterInteractableId;
        public int interactableId;
    }
}
