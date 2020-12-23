using UnityEngine;
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

            taskDataList.Add(task);
        }
    }

    public static void UpdateData(TaskElementData elementData, DataRequest dataRequest)
    {
        var data = Fixtures.taskList.Where(x => x.Id == elementData.Id).FirstOrDefault();
        
        if (elementData.ChangedName)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.Name = elementData.Name;
            else { }
        }

        if (elementData.ChangedCompleteObjective)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.CompleteObjective = elementData.CompleteObjective;
            else { }
        }

        if (elementData.ChangedRepeatable)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.Repeatable = elementData.Repeatable;
            else { }
        }

        if (elementData.ChangedPublicNotes)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PublicNotes = elementData.PublicNotes;
            else { }
        }

        if (elementData.ChangedPrivateNotes)
        {
            if (dataRequest.requestType == Enums.RequestType.Execute)
                data.PrivateNotes = elementData.PrivateNotes;
            else { }
        }
    }

    static public void UpdateIndex(TaskElementData elementData)
    {
        var data = Fixtures.taskList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;
    }
}
