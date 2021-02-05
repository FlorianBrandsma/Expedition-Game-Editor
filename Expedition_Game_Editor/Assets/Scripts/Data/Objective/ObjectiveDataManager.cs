using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public static class ObjectiveDataManager
{
    private static List<ObjectiveBaseData> objectiveDataList;

    public static List<IElementData> GetData(Search.Objective searchParameters)
    {
        GetObjectiveData(searchParameters);

        if (searchParameters.includeAddElement)
            objectiveDataList.Add(DefaultData(searchParameters.questId.First()));

        if (objectiveDataList.Count == 0) return new List<IElementData>();

        var list = (from objectiveData in objectiveDataList
                    select new ObjectiveElementData()
                    {
                        Id = objectiveData.Id,
                        
                        QuestId = objectiveData.QuestId,

                        Index = objectiveData.Index,

                        Name = objectiveData.Name,

                        Journal = objectiveData.Journal,

                        EditorNotes = objectiveData.EditorNotes,
                        GameNotes = objectiveData.GameNotes

                    }).OrderBy(x => x.Id > 0).ThenBy(x => x.Index).ToList();

        if (searchParameters.includeAddElement)
            SetDefaultAddValues(list);

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IElementData>().ToList();
    }

    public static ObjectiveElementData DefaultData(int questId)
    {
        return new ObjectiveElementData()
        {
            Id = -1,

            QuestId = questId
        };
    }

    public static void SetDefaultAddValues(List<ObjectiveElementData> list)
    {
        var addElementData = list.Where(x => x.Id == -1).First();

        addElementData.ExecuteType = Enums.ExecuteType.Add;

        addElementData.Index = list.Count - 1;
    }

    private static void GetObjectiveData(Search.Objective searchParameters)
    {
        objectiveDataList = new List<ObjectiveBaseData>();

        foreach(ObjectiveBaseData objective in Fixtures.objectiveList)
        {
            if (searchParameters.id.Count       > 0 && !searchParameters.id.Contains(objective.Id))             continue;
            if (searchParameters.questId.Count  > 0 && !searchParameters.questId.Contains(objective.QuestId))   continue;

            objectiveDataList.Add(objective);
        }
    }

    public static void AddData(ObjectiveElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            elementData.Id = Fixtures.objectiveList.Count > 0 ? (Fixtures.objectiveList[Fixtures.objectiveList.Count - 1].Id + 1) : 1;
            Fixtures.objectiveList.Add(((ObjectiveData)elementData).Clone());

            elementData.SetOriginalValues();

            AddDependencies(elementData, dataRequest);

        } else { }
    }

    private static void AddDependencies(ObjectiveElementData elementData, DataRequest dataRequest)
    {
        AddObjectiveSaveData(elementData, dataRequest);

        if (!dataRequest.includeDependencies) return;
    }

    private static void AddObjectiveSaveData(ObjectiveElementData elementData, DataRequest dataRequest)
    {
        //Save
        var saveSearchParameters = new Search.Save();

        var saveDataList = DataManager.GetSaveData(saveSearchParameters);

        if (saveDataList.Count == 0) return;

        saveDataList.ForEach(saveData =>
        {
            var objectiveSaveElementData = ObjectiveSaveDataManager.DefaultData(saveData.Id, elementData.Id);
            objectiveSaveElementData.Add(dataRequest);
        });
    }

    public static void UpdateData(ObjectiveElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.Changed) return;

        var data = Fixtures.objectiveList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            if (elementData.ChangedName)
            {
                data.Name = elementData.Name;
            }

            if (elementData.ChangedJournal)
            {
                data.Journal = elementData.Journal;
            }

            if (elementData.ChangedEditorNotes)
            {
                data.EditorNotes = elementData.EditorNotes;
            }

            if (elementData.ChangedGameNotes)
            {
                data.GameNotes = elementData.GameNotes;
            }

            elementData.SetOriginalValues();

        } else { }   
    }

    static public void UpdateIndex(ObjectiveElementData elementData, DataRequest dataRequest)
    {
        if (!elementData.ChangedIndex) return;

        var data = Fixtures.objectiveList.Where(x => x.Id == elementData.Id).FirstOrDefault();

        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            data.Index = elementData.Index;

            elementData.OriginalData.Index = elementData.Index;

        } else { }
    }

    static public void RemoveData(ObjectiveElementData elementData, DataRequest dataRequest)
    {
        if (dataRequest.requestType == Enums.RequestType.Execute)
        {
            RemoveDependencies(elementData, dataRequest);

            Fixtures.objectiveList.RemoveAll(x => x.Id == elementData.Id);

            elementData.RemoveIndex(dataRequest);

        } else {

            RemoveDependencies(elementData, dataRequest);
        }
    }

    private static void RemoveDependencies(ObjectiveElementData elementData, DataRequest dataRequest)
    {
        RemoveWorldInteractableData(elementData, dataRequest);

        RemoveTaskData(elementData, dataRequest);
        RemoveObjectiveSaveData(elementData, dataRequest);
    }

    private static void RemoveWorldInteractableData(ObjectiveElementData elementData, DataRequest dataRequest)
    {
        var worldInteractableSearchParameters = new Search.WorldInteractable()
        {
            objectiveId = new List<int>() { elementData.Id }
        };

        var worldInteractableDataList = DataManager.GetWorldInteractableData(worldInteractableSearchParameters);

        worldInteractableDataList.ForEach(worldInteractableData =>
        {
            var worldInteractableElementData = new WorldInteractableElementData()
            {
                Id = worldInteractableData.Id
            };

            worldInteractableElementData.Remove(dataRequest);
        });
    }

    private static void RemoveTaskData(ObjectiveElementData elementData, DataRequest dataRequest)
    {
        var taskSearchParameters = new Search.Task()
        {
            objectiveId = new List<int>() { elementData.Id }
        };

        var taskDataList = DataManager.GetTaskData(taskSearchParameters);

        taskDataList.ForEach(taskData =>
        {
            var taskElementData = new TaskElementData()
            {
                Id = taskData.Id
            };

            taskElementData.Remove(dataRequest);
        });
    }

    private static void RemoveObjectiveSaveData(ObjectiveElementData elementData, DataRequest dataRequest)
    {
        var objectiveSaveSearchParameters = new Search.ObjectiveSave()
        {
            objectiveId = new List<int>() { elementData.Id }
        };

        var objectiveSaveDataList = DataManager.GetObjectiveSaveData(objectiveSaveSearchParameters);

        objectiveSaveDataList.ForEach(objectiveSaveData =>
        {
            var objectiveSaveElementData = new ObjectiveSaveElementData()
            {
                Id = objectiveSaveData.Id
            };

            objectiveSaveElementData.Remove(dataRequest);
        });
    }

    public static void RemoveIndex(ObjectiveElementData elementData, DataRequest dataRequest)
    {
        var objectiveSearchParameters = new Search.Objective()
        {
            questId = new List<int>() { elementData.QuestId }
        };

        var objectiveDataList = DataManager.GetObjectiveData(objectiveSearchParameters);

        objectiveDataList.Where(x => x.Index > elementData.Index).ToList().ForEach(objectiveData =>
        {
            var objectiveElementData = new ObjectiveElementData()
            {
                Id = objectiveData.Id,
                Index = objectiveData.Index
            };

            objectiveElementData.SetOriginalValues();

            objectiveElementData.Index--;

            objectiveElementData.UpdateIndex(dataRequest);
        });
    }
}
