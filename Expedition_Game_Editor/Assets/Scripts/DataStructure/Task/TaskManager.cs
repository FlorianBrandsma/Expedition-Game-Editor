using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class TaskManager
{
    private TaskController dataController;
    private List<TaskData> taskData_list;

    public List<TaskDataElement> GetTaskDataElements(TaskController dataController)
    {
        this.dataController = dataController;

        GetTaskData();
        //GetIconData()?

        var list = (from oCore in taskData_list
                    select new TaskDataElement()
                    {
                        id = oCore.id,
                        table = oCore.table,
                        type = oCore.type,
                        index = oCore.index,
                        description = oCore.description

                    }).OrderBy(x => x.index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetTaskData()
    {
        taskData_list = new List<TaskData>();

        //Temporary
        for (int i = 0; i < dataController.temp_id_count; i++)
        {
            var taskData = new TaskData();

            taskData.id = (i + 1);
            taskData.table = "Task";
            taskData.index = i;

            taskData.description = "Perform a simple task";

            taskData_list.Add(taskData);
        }
    }

    internal class TaskData : GeneralData
    {
        public int index;
        public string description;
    }
}
