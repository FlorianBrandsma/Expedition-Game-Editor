using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class TaskDataManager
{
    private static List<TaskBaseData> taskDataList;

    public static List<IElementData> GetData(SearchProperties searchProperties)
    {
        var searchParameters = searchProperties.searchParameters.Cast<Search.Task>().First();

        GetTaskData(searchParameters);

        if (taskDataList.Count == 0) return new List<IElementData>();

        var list = (from taskData in taskDataList
                    select new TaskElementData()
                    {
                        Id = taskData.Id,
                        Index = taskData.Index,

                        WorldInteractableId = taskData.WorldInteractableId,
                        ObjectiveId = taskData.ObjectiveId,

                        Name = taskData.Name,

                        CompleteObjective = taskData.CompleteObjective,
                        Repeatable = taskData.Repeatable,

                        PublicNotes = taskData.PublicNotes,
                        PrivateNotes = taskData.PrivateNotes

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    private static void GetTaskData(Search.Task searchParameters)
    {
        taskDataList = new List<TaskBaseData>();
        
        foreach (TaskBaseData task in Fixtures.taskList)
        {
            if (searchParameters.id.Count                   > 0 && !searchParameters.id.Contains(task.Id))                                      continue;
            if (searchParameters.worldInteractableId.Count  > 0 && !searchParameters.worldInteractableId.Contains(task.WorldInteractableId))    continue;
            if (searchParameters.objectiveId.Count          > 0 && !searchParameters.objectiveId.Contains(task.ObjectiveId))                    continue;

            var taskData = new TaskBaseData();

            taskData.Id = task.Id;
            taskData.Index = task.Index;

            taskData.WorldInteractableId = task.WorldInteractableId;
            taskData.ObjectiveId = task.ObjectiveId;

            taskData.Name = task.Name;

            taskData.CompleteObjective = task.CompleteObjective;
            taskData.Repeatable = task.Repeatable;

            taskData.PublicNotes = task.PublicNotes;
            taskData.PrivateNotes = task.PrivateNotes;
            
            taskDataList.Add(taskData);
        }
    }

    public static void UpdateData(TaskElementData elementData)
    {
        var data = Fixtures.taskList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedName)
            data.Name = elementData.Name;

        if (elementData.ChangedCompleteObjective)
            data.CompleteObjective = elementData.CompleteObjective;

        if (elementData.ChangedRepeatable)
            data.Repeatable = elementData.Repeatable;

        if (elementData.ChangedPublicNotes)
            data.PublicNotes = elementData.PublicNotes;

        if (elementData.ChangedPrivateNotes)
            data.PrivateNotes = elementData.PrivateNotes;
    }

    static public void UpdateIndex(TaskElementData elementData)
    {
        var data = Fixtures.taskList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;
    }
}
