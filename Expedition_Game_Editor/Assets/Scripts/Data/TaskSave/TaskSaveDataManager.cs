using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class TaskSaveDataManager
{
    private static List<TaskBaseData> taskDataList;

    private static List<TaskSaveBaseData> taskSaveDataList;
    
    public static List<IElementData> GetData(Search.TaskSave searchParameters)
    {
        GetTaskData(searchParameters);
        
        if (taskDataList.Count == 0) return new List<IElementData>();

        GetTaskSaveData(searchParameters);

        var list = (from taskData       in taskDataList
                    join taskSaveData   in taskSaveDataList on taskData.Id equals taskSaveData.TaskId
                    select new TaskSaveElementData()
                    {
                        Id = taskSaveData.Id,

                        TaskId = taskSaveData.TaskId,

                        Complete = taskSaveData.Complete,

                        Index = taskData.Index,

                        Name = taskData.Name,

                        Repeatable = taskData.Repeatable,
                        
                        PublicNotes = taskData.PublicNotes,
                        PrivateNotes = taskData.PrivateNotes,

                        ObjectiveId = taskData.ObjectiveId

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static TaskSaveElementData DefaultData(int saveId, int taskId, int worldInteractableId)
    {
        return new TaskSaveElementData()
        {
            SaveId = saveId,
            TaskId = taskId,
            WorldInteractableId = worldInteractableId
        };
    }

    private static void GetTaskData(Search.TaskSave searchParameters)
    {
        taskDataList = new List<TaskBaseData>();

        foreach(TaskBaseData task in Fixtures.taskList)
        {
            if (searchParameters.worldInteractableId.Count  > 0 && !searchParameters.worldInteractableId.Contains(task.WorldInteractableId))    continue;
            if (searchParameters.objectiveId.Count          > 0 && !searchParameters.objectiveId.Contains(task.ObjectiveId))                    continue;

            taskDataList.Add(task);
        }
    }

    private static void GetTaskSaveData(Search.TaskSave searchParameters)
    {
        searchParameters.taskId = taskDataList.Select(x => x.Id).ToList();
        
        taskSaveDataList = DataManager.GetTaskSaveData(searchParameters);
    }
    
    public static void AddData(TaskSaveElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.taskSaveList.Count > 0 ? (Fixtures.taskSaveList[Fixtures.taskSaveList.Count - 1].Id + 1) : 1;
            Fixtures.taskSaveList.Add(((TaskSaveData)elementData).Clone());

            elementData.SetOriginalValues();

        } else { }
    }

    public static void UpdateData(TaskSaveElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.taskSaveList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedComplete)
            {
                data.Complete = elementData.Complete;
            }

            elementData.SetOriginalValues();

        } else { }
    }

    public static void RemoveData(TaskSaveElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            Fixtures.taskSaveList.RemoveAll(x => x.Id == elementData.Id);

        } else { }
    }
}
