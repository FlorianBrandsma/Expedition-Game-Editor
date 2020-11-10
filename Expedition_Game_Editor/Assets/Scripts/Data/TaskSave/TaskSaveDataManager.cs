using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class TaskSaveDataManager
{
    private static List<TaskSaveBaseData> taskSaveDataList;

    private static List<TaskBaseData> taskDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.TaskSave>().First();

        GetTaskSaveData(searchParameters);

        if (taskSaveDataList.Count == 0) return new List<IElementData>();

        GetTaskData();

        var list = (from taskSaveData   in taskSaveDataList
                    join taskData       in taskDataList on taskSaveData.TaskId equals taskData.Id
                    select new TaskSaveElementData()
                    {
                        Id = taskSaveData.Id,
                        
                        ObjectiveSaveId = taskSaveData.ObjectiveSaveId,
                        TaskId = taskSaveData.TaskId,

                        Complete = taskSaveData.Complete,

                        Index = taskData.Index,

                        Name = taskData.Name,

                        Repeatable = taskData.Repeatable,
                        
                        PublicNotes = taskData.PublicNotes,
                        PrivateNotes = taskData.PrivateNotes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetTaskSaveData(Search.TaskSave searchParameters)
    {
        taskSaveDataList = new List<TaskSaveBaseData>();

        foreach (TaskSaveBaseData taskSave in Fixtures.taskSaveList)
        {
            if (searchParameters.id.Count                   > 0 && !searchParameters.id.Contains(taskSave.Id))                                      continue;
            if (searchParameters.worldInteractableId.Count  > 0 && !searchParameters.worldInteractableId.Contains(taskSave.WorldInteractableId))    continue;
            if (searchParameters.objectiveSaveId.Count      > 0 && !searchParameters.objectiveSaveId.Contains(taskSave.ObjectiveSaveId))            continue;

            taskSaveDataList.Add(taskSave);
        }
    }

    private static void GetTaskData()
    {
        var searchParameters = new Search.Task();
        searchParameters.id = taskSaveDataList.Select(x => x.TaskId).Distinct().ToList();

        taskDataList = DataManager.GetTaskData(searchParameters);
    }

    public static void UpdateData(TaskSaveElementData elementData)
    {
        var data = Fixtures.taskSaveList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedComplete)
            data.Complete = elementData.Complete;
    }
}
