using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class TaskDataManager
{
    private static List<TaskBaseData> taskDataList;

    public static List<IElementData> GetData(Search.Task searchParameters)
    {
        GetTaskData(searchParameters);

        if (taskDataList.Count == 0) return new List<IElementData>();

        var list = (from taskData in taskDataList
                    select new TaskElementData()
                    {
                        Id = taskData.Id,
                        
                        WorldInteractableId = taskData.WorldInteractableId,
                        ObjectiveId = taskData.ObjectiveId,

                        Default = taskData.Default,

                        Index = taskData.Index,

                        Name = taskData.Name,

                        CompleteObjective = taskData.CompleteObjective,
                        Repeatable = taskData.Repeatable,

                        PublicNotes = taskData.PublicNotes,
                        PrivateNotes = taskData.PrivateNotes

                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static TaskElementData DefaultData(int worldInteractableId)
    {
        return new TaskElementData()
        {
            WorldInteractableId = worldInteractableId,

            Name = "Default description"
        };
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

    public static void AddData(TaskElementData elementData, DataRequest dataRequest, bool copy = false)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.taskList.Count > 0 ? (Fixtures.taskList[Fixtures.taskList.Count - 1].Id + 1) : 1;
            Fixtures.taskList.Add(((TaskData)elementData).Clone());

            elementData.SetOriginalValues();

            if (copy) return;

            var interactionElementData = InteractionDataManager.DefaultData(elementData.Id, true);
            interactionElementData.Add(dataRequest);

        } else { }
    }
    
    public static void UpdateData(TaskElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.taskList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedName)
            {
                data.Name = elementData.Name;
            }

            if (elementData.ChangedCompleteObjective)
            {
                data.CompleteObjective = elementData.CompleteObjective;
            }

            if (elementData.ChangedRepeatable)
            {
                data.Repeatable = elementData.Repeatable;
            }

            if (elementData.ChangedPublicNotes)
            {
                data.PublicNotes = elementData.PublicNotes;
            }

            if (elementData.ChangedPrivateNotes)
            {
                data.PrivateNotes = elementData.PrivateNotes;
            }

            elementData.SetOriginalValues();

        } else { }
    }

    static public void UpdateIndex(TaskElementData elementData)
    {
        if (!elementData.ChangedIndex) return;

        var data = Fixtures.taskList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        data.Index = elementData.Index;

        elementData.OriginalData.Index = elementData.Index;
    }

    public static void RemoveData(TaskElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            RemoveInteractionData(elementData, dataRequest);

            Fixtures.taskList.RemoveAll(x => x.Id == elementData.Id);

        } else {

            RemoveInteractionData(elementData, dataRequest);
        }
    }

    private static void RemoveInteractionData(TaskElementData elementData, DataRequest dataRequest)
    {
        var interactionSearchParameters = new Search.Interaction()
        {
            taskId = new List<int>() { elementData.Id }
        };

        var interactionDataList = DataManager.GetInteractionData(interactionSearchParameters);

        //if (interactionDataList.Count > 1)
        //    dataRequest.errorList.Add("Task contains interactions");

        interactionDataList.ForEach(interactionData =>
        {
            var interactionElementData = new InteractionElementData()
            {
                Id = interactionData.Id
            };

            interactionElementData.Remove(dataRequest);
        });
    }
}
