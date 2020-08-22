using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TaskSaveDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<TaskSaveData> taskSaveDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.TaskData> taskDataList;

    public TaskSaveDataManager(TaskSaveController taskController)
    {
        DataController = taskController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.TaskSave>().First();

        GetTaskSaveData(searchParameters);

        if (taskSaveDataList.Count == 0) return new List<IElementData>();

        GetTaskData();

        var list = (from taskSaveData   in taskSaveDataList
                    join taskData       in taskDataList on taskSaveData.taskId equals taskData.id
                    select new TaskSaveElementData()
                    {
                        Id = taskSaveData.id,
                        Index = taskSaveData.index,

                        ObjectiveSaveId = taskSaveData.objectiveSaveId,
                        TaskId = taskSaveData.taskId,

                        Complete = taskSaveData.complete,

                        name = taskData.name,

                        repeatable = taskData.repeatable,

                        publicNotes = taskData.publicNotes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public void GetTaskSaveData(Search.TaskSave searchParameters)
    {
        taskSaveDataList = new List<TaskSaveData>();

        foreach (Fixtures.TaskSave taskSave in Fixtures.taskSaveList)
        {
            if (searchParameters.id.Count                   > 0 && !searchParameters.id.Contains(taskSave.id))                                      continue;
            if (searchParameters.worldInteractableId.Count  > 0 && !searchParameters.worldInteractableId.Contains(taskSave.worldInteractableId))    continue;
            if (searchParameters.objectiveSaveId.Count      > 0 && !searchParameters.objectiveSaveId.Contains(taskSave.objectiveSaveId))            continue;

            var taskSaveData = new TaskSaveData();

            taskSaveData.id = taskSave.id;
            taskSaveData.index = taskSave.index;

            taskSaveData.worldInteractableId = taskSave.worldInteractableId;
            taskSaveData.objectiveSaveId = taskSave.objectiveSaveId;
            taskSaveData.taskId = taskSave.taskId;

            taskSaveData.complete = taskSave.complete;

            taskSaveDataList.Add(taskSaveData);
        }
    }

    internal void GetTaskData()
    {
        var taskSearchParameters = new Search.Task();
        taskSearchParameters.id = taskSaveDataList.Select(x => x.taskId).Distinct().ToList();

        taskDataList = dataManager.GetTaskData(taskSearchParameters);
    }

    internal class TaskSaveData
    {
        public int id;
        public int index;

        public int worldInteractableId;
        public int objectiveSaveId;
        public int taskId;

        public bool complete;
    }
}
