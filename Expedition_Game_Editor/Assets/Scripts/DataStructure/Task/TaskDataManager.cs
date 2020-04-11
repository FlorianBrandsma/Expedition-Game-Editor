﻿using UnityEngine;
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

    public List<IDataElement> GetDataElements(IEnumerable searchParameters)
    {
        var taskSearchData = searchParameters.Cast<Search.Task>().FirstOrDefault();

        GetTaskData(taskSearchData);

        var list = (from taskData in taskDataList
                    select new TaskDataElement()
                    {
                        Id = taskData.Id,
                        Index = taskData.Index,

                        WorldInteractableId = taskData.worldInteractableId,
                        ObjectiveId = taskData.objectiveId,

                        Name = taskData.name,
                        PublicNotes = taskData.notes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetTaskData(Search.Task searchParameters)
    {
        taskDataList = new List<TaskData>();

        foreach (Fixtures.Task task in Fixtures.taskList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(task.Id)) continue;
            if (searchParameters.worldInteractableId.Count > 0 && !searchParameters.worldInteractableId.Contains(task.worldInteractableId)) continue;
            if (searchParameters.objectiveId.Count > 0 && !searchParameters.objectiveId.Contains(task.objectiveId)) continue;

            var questData = new TaskData();

            questData.Id = task.Id;
            questData.Index = task.Index;

            questData.worldInteractableId = task.worldInteractableId;
            questData.objectiveId = task.objectiveId;
            questData.name = task.name;
            questData.notes = task.publicNotes;

            taskDataList.Add(questData);
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
