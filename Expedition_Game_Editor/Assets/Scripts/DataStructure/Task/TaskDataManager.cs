using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TaskDataManager
{
    private TaskController taskController;
    private List<TaskData> taskDataList;

    public void InitializeManager(TaskController taskController)
    {
        this.taskController = taskController;
    }

    public List<TaskDataElement> GetTaskDataElements(IEnumerable searchParameters)
    {
        var objectiveSearchData = searchParameters.Cast<Search.Task>().FirstOrDefault();

        GetTaskData(objectiveSearchData);

        var list = (from taskData in taskDataList
                    select new TaskDataElement()
                    {
                        id = taskData.id,
                        table = taskData.table,

                        Index = taskData.index,
                        Description = taskData.description

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetTaskData(Search.Task searchParameters)
    {
        taskDataList = new List<TaskData>();

        int index = 0;
        
        for (int i = 0; i < searchParameters.temp_id_count; i++)
        {
            var taskData = new TaskData();

            int id = (i + 1);

            taskData.id = id;
            taskData.table = "Task";
            taskData.index = index;

            taskData.description = "Perform a simple task";

            taskDataList.Add(taskData);

            index++;
        }
    }

    internal class TaskData : GeneralData
    {
        public int index;
        public string description;
    }
}
