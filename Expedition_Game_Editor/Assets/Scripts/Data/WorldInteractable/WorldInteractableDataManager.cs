using UnityEngine;
using System;
using System.Collections;
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

        GetInteractableData();
        GetModelData();
        GetIconData();

        var list = (from worldInteractableData  in worldInteractableDataList
                    join interactableData       in interactableDataList on worldInteractableData.InteractableId equals interactableData.Id
                    join modelData              in modelDataList        on interactableData.ModelId             equals modelData.Id
                    join iconData               in iconDataList         on modelData.IconId                     equals iconData.Id

                    select new WorldInteractableElementData()
                    {
                        Id = worldInteractableData.Id,

                        Type = worldInteractableData.Type,

                        PhaseId = worldInteractableData.PhaseId,
                        QuestId = worldInteractableData.QuestId,
                        ObjectiveId = worldInteractableData.ObjectiveId,

                        ChapterInteractableId = worldInteractableData.ChapterInteractableId,
                        InteractableId = worldInteractableData.InteractableId,
                        
                        InteractableName =  interactableData.Name,
                        ModelIconPath = iconData.Path,

                        Height = modelData.Height,
                        Width = modelData.Width,
                        Depth = modelData.Depth

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetCustomWorldInteractableData(Search.WorldInteractable searchParameters)
    {
        worldInteractableDataList = new List<WorldInteractableBaseData>();
        
        foreach (WorldInteractableBaseData worldInteractable in Fixtures.worldInteractableList)
        {
            if (searchParameters.id.Count                       > 0 && !searchParameters.id.Contains(worldInteractable.Id))                                         continue;
            if (searchParameters.type.Count                     > 0 && !searchParameters.type.Contains(worldInteractable.Type))                                     continue;
            if (searchParameters.chapterInteractableId.Count    > 0 && !searchParameters.chapterInteractableId.Contains(worldInteractable.ChapterInteractableId))   continue;
            if (searchParameters.phaseId.Count                  > 0 && !searchParameters.phaseId.Contains(worldInteractable.PhaseId))                               continue;
            if (searchParameters.questId.Count                  > 0 && !searchParameters.questId.Contains(worldInteractable.QuestId))                               continue;
            if (searchParameters.objectiveId.Count              > 0 && !searchParameters.objectiveId.Contains(worldInteractable.ObjectiveId))                       continue;
            if (searchParameters.interactableId.Count           > 0 && !searchParameters.interactableId.Contains(worldInteractable.InteractableId))                 continue;

            var worldInteractableData = new WorldInteractableBaseData();

            worldInteractableData.Id = worldInteractable.Id;

            worldInteractableData.Type = worldInteractable.Type;
            
            worldInteractableData.PhaseId = worldInteractable.PhaseId;
            worldInteractableData.QuestId = worldInteractable.QuestId;
            worldInteractableData.ObjectiveId = worldInteractable.ObjectiveId;

            worldInteractableData.ChapterInteractableId = worldInteractable.ChapterInteractableId;
            worldInteractableData.InteractableId = worldInteractable.InteractableId;

            worldInteractableDataList.Add(worldInteractableData);
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
            if (searchParameters.phaseId.Count                  > 0 && !searchParameters.phaseId.Contains(worldInteractable.PhaseId))                               continue;
            if (searchParameters.questId.Count                  > 0 && !searchParameters.questId.Contains(worldInteractable.QuestId))                               continue;
            if (searchParameters.objectiveId.Count              > 0 && !searchParameters.objectiveId.Contains(worldInteractable.ObjectiveId))                       continue;
            if (searchParameters.interactableId.Count           > 0 && !searchParameters.interactableId.Contains(worldInteractable.InteractableId))                 continue;

            var worldInteractableData = new WorldInteractableBaseData();

            worldInteractableData.Id = worldInteractable.Id;

            worldInteractableData.Type = worldInteractable.Type;
            
            worldInteractableData.PhaseId = worldInteractable.PhaseId;
            worldInteractableData.QuestId = worldInteractable.QuestId;
            worldInteractableData.ObjectiveId = worldInteractable.ObjectiveId;

            worldInteractableData.ChapterInteractableId = worldInteractable.ChapterInteractableId;
            worldInteractableData.InteractableId = worldInteractable.InteractableId;

            worldInteractableDataList.Add(worldInteractableData);
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
            var worldInteractableData = new WorldInteractableBaseData();

            worldInteractableData.Id = worldInteractable.Id;

            worldInteractableData.Type = worldInteractable.Type;

            worldInteractableData.PhaseId = worldInteractable.PhaseId;
            worldInteractableData.QuestId = worldInteractable.QuestId;
            worldInteractableData.ObjectiveId = worldInteractable.ObjectiveId;

            worldInteractableData.ChapterInteractableId = worldInteractable.ChapterInteractableId;
            worldInteractableData.InteractableId = worldInteractable.InteractableId;

            worldInteractableDataList.Add(worldInteractableData);
        }
    }

    private static void GetInteractableData()
    {
        var interactableSearchParameters = new Search.Interactable();

        interactableSearchParameters.id = worldInteractableDataList.Select(x => x.InteractableId).Distinct().ToList();

        interactableDataList = DataManager.GetInteractableData(interactableSearchParameters);
    }

    private static void GetModelData()
    {
        var modelSearchParameters = new Search.Model();

        modelSearchParameters.id = interactableDataList.Select(x => x.ModelId).Distinct().ToList();

        modelDataList = DataManager.GetModelData(modelSearchParameters);
    }

    private static void GetIconData()
    {
        var iconSearchParameters = new Search.Icon();
        iconSearchParameters.id = modelDataList.Select(x => x.IconId).Distinct().ToList();

        iconDataList = DataManager.GetIconData(iconSearchParameters);
    }

    public static void UpdateData(WorldInteractableElementData elementData)
    {
        var data = Fixtures.worldInteractableList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedQuestId)
            data.QuestId = elementData.QuestId;
    }

    public static void UpdateSearch(WorldInteractableElementData elementData)
    {
        var data = Fixtures.worldInteractableList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (elementData.ChangedInteractableId)
            data.InteractableId = elementData.InteractableId;
    }
}
