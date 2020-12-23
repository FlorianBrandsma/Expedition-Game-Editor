using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class WorldInteractableDataManager
{
    private static List<WorldInteractableBaseData> worldInteractableDataList = new List<WorldInteractableBaseData>();

    private static List<InteractableBaseData> interactableDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var list = new List<WorldInteractableElementData>();

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

            case Search.WorldInteractable.RequestType.GetSceneActorWorldInteractables:

                GetSceneActorWorldInteractableData(searchParameters);
                break;
        }
        
        if (worldInteractableDataList.Count > 0)
        {
            GetInteractableData();
            GetModelData();
            GetIconData();

            list = (from worldInteractableData  in worldInteractableDataList
                    join interactableData       in interactableDataList on worldInteractableData.InteractableId equals interactableData.Id
                    join modelData              in modelDataList        on interactableData.ModelId             equals modelData.Id
                    join iconData               in iconDataList         on modelData.IconId                     equals iconData.Id

                    select new WorldInteractableElementData()
                    {
                        Id = worldInteractableData.Id,

                        Type = worldInteractableData.Type,

                        ChapterId = worldInteractableData.ChapterId,
                        PhaseId = worldInteractableData.PhaseId,
                        QuestId = worldInteractableData.QuestId,
                        ObjectiveId = worldInteractableData.ObjectiveId,

                        ChapterInteractableId = worldInteractableData.ChapterInteractableId,
                        InteractableId = worldInteractableData.InteractableId,
                        
                        ModelId = modelData.Id,

                        ModelPath = modelData.Path,

                        InteractableName =  interactableData.Name,
                        ModelIconPath = iconData.Path,

                        Height = modelData.Height,
                        Width = modelData.Width,
                        Depth = modelData.Depth,

                        Scale = interactableData.Scale

                    }).ToList();
        }

        if (searchParameters.includeAddElement)
            AddDefaultElementData(searchParameters, list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void AddDefaultElementData(Search.WorldInteractable searchParameters, List<WorldInteractableElementData> list)
    {
        list.Insert(0, new WorldInteractableElementData()
        {
            ExecuteType = Enums.ExecuteType.Add,

            Type = searchParameters.type.First()
        });
    }

    private static void GetCustomWorldInteractableData(Search.WorldInteractable searchParameters)
    {
        worldInteractableDataList = new List<WorldInteractableBaseData>();
        
        foreach (WorldInteractableBaseData worldInteractable in Fixtures.worldInteractableList)
        {
            if (searchParameters.id.Count                       > 0 && !searchParameters.id.Contains(worldInteractable.Id))                                         continue;
            if (searchParameters.type.Count                     > 0 && !searchParameters.type.Contains(worldInteractable.Type))                                     continue;
            if (searchParameters.chapterInteractableId.Count    > 0 && !searchParameters.chapterInteractableId.Contains(worldInteractable.ChapterInteractableId))   continue;
            if (searchParameters.chapterId.Count                > 0 && !searchParameters.chapterId.Contains(worldInteractable.ChapterId))                           continue;
            if (searchParameters.phaseId.Count                  > 0 && !searchParameters.phaseId.Contains(worldInteractable.PhaseId))                               continue;
            if (searchParameters.questId.Count                  > 0 && !searchParameters.questId.Contains(worldInteractable.QuestId))                               continue;
            if (searchParameters.objectiveId.Count              > 0 && !searchParameters.objectiveId.Contains(worldInteractable.ObjectiveId))                       continue;
            if (searchParameters.interactableId.Count           > 0 && !searchParameters.interactableId.Contains(worldInteractable.InteractableId))                 continue;

            worldInteractableDataList.Add(worldInteractable);
        }
    }

    private static void GetRegionWorldInteractableData(Search.WorldInteractable searchParameters)
    {
        worldInteractableDataList = new List<WorldInteractableBaseData>();

        var interactionDestinationList  = Fixtures.interactionDestinationList.Where(x => searchParameters.regionId.Contains(x.RegionId)).Distinct().ToList();
        var interactionList             = Fixtures.interactionList.Where(x => interactionDestinationList.Select(y => y.InteractionId).Contains(x.Id)).Distinct().ToList();
        var taskList                    = Fixtures.taskList.Where(x => interactionList.Select(y => y.TaskId).Contains(x.Id) && searchParameters.objectiveId.Contains(x.ObjectiveId)).Distinct().ToList();
        var worldInteractableList       = Fixtures.worldInteractableList.Where(x => taskList.Select(y => y.WorldInteractableId).Contains(x.Id)).Distinct().ToList();

        foreach (WorldInteractableBaseData worldInteractable in worldInteractableList)
        {
            if (searchParameters.id.Count                       > 0 && !searchParameters.id.Contains(worldInteractable.Id))                                         continue;
            if (searchParameters.type.Count                     > 0 && !searchParameters.type.Contains(worldInteractable.Type))                                     continue;
            if (searchParameters.chapterInteractableId.Count    > 0 && !searchParameters.chapterInteractableId.Contains(worldInteractable.ChapterInteractableId))   continue;
            if (searchParameters.chapterId.Count                > 0 && !searchParameters.chapterId.Contains(worldInteractable.ChapterId))                           continue;
            if (searchParameters.phaseId.Count                  > 0 && !searchParameters.phaseId.Contains(worldInteractable.PhaseId))                               continue;
            if (searchParameters.questId.Count                  > 0 && !searchParameters.questId.Contains(worldInteractable.QuestId))                               continue;
            if (searchParameters.objectiveId.Count              > 0 && !searchParameters.objectiveId.Contains(worldInteractable.ObjectiveId))                       continue;
            if (searchParameters.interactableId.Count           > 0 && !searchParameters.interactableId.Contains(worldInteractable.InteractableId))                 continue;

            worldInteractableDataList.Add(worldInteractable);
        }
    }

    private static void GetQuestAndObjectiveWorldInteractableData(Search.WorldInteractable searchParameters)
    {
        worldInteractableDataList = new List<WorldInteractableBaseData>();

        var worldInteractableList = new List<WorldInteractableBaseData>();

        Fixtures.worldInteractableList.Where(x => searchParameters.questId.Contains(x.QuestId)).Distinct().ToList().ForEach(x => worldInteractableList.Add(x));
        Fixtures.worldInteractableList.Where(x => searchParameters.objectiveId.Contains(x.ObjectiveId)).Distinct().ToList().ForEach(x => worldInteractableList.Add(x));

        foreach (WorldInteractableBaseData worldInteractable in worldInteractableList)
        {
            worldInteractableDataList.Add(worldInteractable);
        }
    }

    private static void GetSceneActorWorldInteractableData(Search.WorldInteractable searchParameters)
    {
        worldInteractableDataList = new List<WorldInteractableBaseData>();

        var interactionDestinations = Fixtures.interactionDestinationList.Where(x => searchParameters.regionId.Contains(x.RegionId)).ToList();
        var interactions            = Fixtures.interactionList.Where(x => interactionDestinations.Select(y => y.InteractionId).Contains(x.Id)).ToList();
        var tasks                   = Fixtures.taskList.Where(x => interactions.Select(y => y.TaskId).Contains(x.Id) && x.ObjectiveId == 0).ToList();

        var regionWorldInteractables    = Fixtures.worldInteractableList.Where(x => tasks.Select(y => y.WorldInteractableId).Contains(x.Id)).ToList();
        var chapterWorldInteractables   = Fixtures.worldInteractableList.Where(x => searchParameters.chapterId.Contains(x.ChapterId)).ToList();
        var questWorldInteractables     = Fixtures.worldInteractableList.Where(x => searchParameters.questId.Contains(x.QuestId)).ToList();
        var objectiveWorldInteractables = Fixtures.worldInteractableList.Where(x => searchParameters.objectiveId.Contains(x.ObjectiveId)).ToList();

        var worldInteractableList = regionWorldInteractables.Union(chapterWorldInteractables)
                                                            .Union(questWorldInteractables)
                                                            .Union(objectiveWorldInteractables).ToList();

        foreach (WorldInteractableBaseData worldInteractable in worldInteractableList)
        {
            if (searchParameters.excludeId.Count > 0 && searchParameters.excludeId.Contains(worldInteractable.Id)) continue;

            worldInteractableDataList.Add(worldInteractable);
        }
    }

    private static void GetInteractableData()
    {
        var searchParameters = new Search.Interactable();
        searchParameters.id = worldInteractableDataList.Select(x => x.InteractableId).Distinct().ToList();

        interactableDataList = DataManager.GetInteractableData(searchParameters);
    }

    private static void GetModelData()
    {
        var searchParameters = new Search.Model();
        searchParameters.id = interactableDataList.Select(x => x.ModelId).Distinct().ToList();

        modelDataList = DataManager.GetModelData(searchParameters);
    }

    private static void GetIconData()
    {
        var searchParameters = new Search.Icon();
        searchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(searchParameters);
    }

    public static void AddData(WorldInteractableElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.worldInteractableList[Fixtures.worldInteractableList.Count - 1].Id + 1;
            Fixtures.worldInteractableList.Add(((WorldInteractableData)elementData).Clone());
        } else { }
    }

    public static void UpdateData(WorldInteractableElementData elementData, DataRequest dataRequest)
    {
        var data = Fixtures.worldInteractableList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (elementData.ChangedInteractableId)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.InteractableId = elementData.InteractableId;
            else { }
        }
        
        if (elementData.ChangedQuestId)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.QuestId = elementData.QuestId;
            else { }
        }
    }

    public static void UpdateSearch(WorldInteractableElementData elementData)
    {
        var data = Fixtures.worldInteractableList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (elementData.ChangedInteractableId)
            data.InteractableId = elementData.InteractableId;
    }

    public static void RemoveData(WorldInteractableElementData elementData, DataRequest dataRequest)
    {
        if(dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.worldInteractableList.RemoveAll(x => x.Id == elementData.Id);
        } else { }
    }
}
