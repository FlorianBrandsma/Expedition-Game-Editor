using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TaskDataManager : IDataManager
{
    public IDataController DataController { get; set; }

    private List<TaskData> taskDataList;

    public TaskDataManager(IDataController dataController)
    {
        DataController = dataController;
    }

    public List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Task>().First();

        GetTaskData(searchParameters);

        if (taskDataList.Count == 0) return new List<IElementData>();

        var list = (from taskData in taskDataList
                    select new TaskElementData()
                    {
                        Id = taskData.Id,
                        Index = taskData.Index,

                        WorldInteractableId = taskData.worldInteractableId,
                        ObjectiveId = taskData.objectiveId,

                        Name = taskData.name,
                        PublicNotes = taskData.notes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public void GetTaskData(Search.Task searchParameters)
    {
        taskDataList = new List<TaskData>();
        
        foreach (Fixtures.Task task in Fixtures.taskList)
        {
            if (searchParameters.id.Count                   > 0 && !searchParameters.id.Contains(task.Id))                                      continue;
            if (searchParameters.worldInteractableId.Count  > 0 && !searchParameters.worldInteractableId.Contains(task.worldInteractableId))    continue;
            if (searchParameters.objectiveId.Count          > 0 && !searchParameters.objectiveId.Contains(task.objectiveId))                    continue;

            var taskData = new TaskData();

            taskData.Id = task.Id;
            taskData.Index = task.Index;

            taskData.worldInteractableId = task.worldInteractableId;
            taskData.objectiveId = task.objectiveId;
            taskData.name = task.name;
            taskData.notes = task.publicNotes;

            taskDataList.Add(taskData);
        }
    }

    internal class TaskData : GeneralData
    {
        public int worldInteractableId;
        public int objectiveId;

        public string name;
        public string notes;
    }
}
