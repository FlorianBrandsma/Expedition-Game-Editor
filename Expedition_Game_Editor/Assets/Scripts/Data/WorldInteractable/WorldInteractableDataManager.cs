﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class WorldInteractableDataManager
{
    private static List<WorldInteractableBaseData> worldInteractableDataList = new List<WorldInteractableBaseData>();

    private static List<InteractableBaseData> interactableDataList;
    private static List<ModelBaseData> modelDataList;
    private static List<IconBaseData> iconDataList;

    public static List<IElementData> GetData(Search.WorldInteractable searchParameters)
    {
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

        if (searchParameters.includeAddElement)
            worldInteractableDataList.Add(DefaultData(searchParameters.type.First()));

        if (worldInteractableDataList.Count == 0) return new List<IElementData>();
        
        GetInteractableData();
        GetModelData();
        GetIconData();

        var list = (from worldInteractableData  in worldInteractableDataList

                    join leftJoin in (from interactableData in interactableDataList
                                      join modelData        in modelDataList    on interactableData.ModelId equals modelData.Id
                                      join iconData         in iconDataList     on modelData.IconId         equals iconData.Id
                                      select new { interactableData, modelData, iconData }) on worldInteractableData.InteractableId equals leftJoin.interactableData.Id into interactableData

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
                        
                        ModelId = interactableData.FirstOrDefault() != null ? interactableData.FirstOrDefault().modelData.Id : 0,

                        ModelPath = interactableData.FirstOrDefault() != null ? interactableData.FirstOrDefault().modelData.Path : "",

                        InteractableName = interactableData.FirstOrDefault() != null ? interactableData.FirstOrDefault().interactableData.Name : "",
                        ModelIconPath = interactableData.FirstOrDefault() != null ? interactableData.FirstOrDefault().iconData.Path : "",

                        Height = interactableData.FirstOrDefault() != null ? interactableData.FirstOrDefault().modelData.Height : 0,
                        Width = interactableData.FirstOrDefault() != null ? interactableData.FirstOrDefault().modelData.Width : 0,
                        Depth = interactableData.FirstOrDefault() != null ? interactableData.FirstOrDefault().modelData.Depth : 0,

                        Scale = interactableData.FirstOrDefault() != null ? interactableData.FirstOrDefault().interactableData.Scale : 0

                    }).OrderBy(x => x.Id).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static WorldInteractableElementData DefaultData(int type)
    {
        return new WorldInteractableElementData()
        {
            Id = -1,

            Type = type,

            ModelId = 1
        };
    }

    public static void SetDefaultAddValues(List<WorldInteractableElementData> list)
    {
        var addElementData = list.Where(x => x.Id == -1).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;
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
            elementData.Id = Fixtures.worldInteractableList.Count > 0 ? (Fixtures.worldInteractableList[Fixtures.worldInteractableList.Count - 1].Id + 1) : 1;
            Fixtures.worldInteractableList.Add(((WorldInteractableData)elementData).Clone());
            
            elementData.SetOriginalValues();

            if(elementData.Type == (int)Enums.InteractableType.Controllable)
                SaveDataManager.UpdateReferences(dataRequest);
            else
                AddDependencies(elementData, dataRequest);
            
        } else { }
    }

    private static void AddDependencies(WorldInteractableElementData elementData, DataRequest dataRequest)
    {
        if (!dataRequest.includeDependencies) return;

        AddTaskData(elementData, dataRequest);
    }

    private static void AddTaskData(WorldInteractableElementData elementData, DataRequest dataRequest)
    {
        var taskElementData = TaskDataManager.DefaultData(elementData.Id, elementData.ObjectiveId);

        taskElementData.Name = "Default description";

        taskElementData.Add(dataRequest);
    }

    public static void UpdateData(WorldInteractableElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.worldInteractableList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedInteractableId)
            {
                data.InteractableId = elementData.InteractableId;
            }

            if (elementData.ChangedQuestId)
            {
                data.QuestId = elementData.QuestId;
            }

            elementData.SetOriginalValues();

        } else { }
    }

    public static void RemoveData(WorldInteractableElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            RemoveDependencies(elementData, dataRequest);

            Fixtures.worldInteractableList.RemoveAll(x => x.Id == elementData.Id);

            if (elementData.Type == (int)Enums.InteractableType.Controllable)
                SaveDataManager.UpdateReferences(dataRequest);

        } else {

            RemoveDependencies(elementData, dataRequest);
        }
    }

    private static void RemoveDependencies(WorldInteractableElementData elementData, DataRequest dataRequest)
    {
        RemoveTaskData(elementData, dataRequest);
        RemoveSceneActorData(elementData, dataRequest);
    }

    private static void RemoveTaskData(WorldInteractableElementData elementData, DataRequest dataRequest)
    {
        var taskSearchParameters = new Search.Task()
        {
            worldInteractableId = new List<int>() { elementData.Id }
        };

        var taskDataList = DataManager.GetTaskData(taskSearchParameters);

        //if (taskDataList.Count > 1)
        //    dataRequest.errorList.Add("World interactable contains tasks");

        taskDataList.ForEach(taskData =>
        {
            var taskElementData = new TaskElementData()
            {
                Id = taskData.Id
            };

            taskElementData.Remove(dataRequest);
        });
    }

    private static void RemoveSceneActorData(WorldInteractableElementData elementData, DataRequest dataRequest)
    {
        var sceneActorSearchParameters = new Search.SceneActor()
        {
            worldInteractableId = new List<int>() { elementData.Id }
        };

        var sceneActorDataList = DataManager.GetSceneActorData(sceneActorSearchParameters);

        sceneActorDataList.ForEach(sceneActorData =>
        {
            var sceneActorElementData = new SceneActorElementData()
            {
                Id = sceneActorData.Id
            };

            sceneActorElementData.Remove(dataRequest);
        });
    }
}
