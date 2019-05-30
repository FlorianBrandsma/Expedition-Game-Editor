using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TaskDataManager
{
    private TaskController taskController;
    private List<TaskData> taskDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.TerrainElementData> terrainElementDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.ElementData> elementDataList;

    public void InitializeManager(TaskController taskController)
    {
        this.taskController = taskController;
    }

    public List<IDataElement> GetTaskDataElements(IEnumerable searchParameters)
    {
        var objectiveSearchData = searchParameters.Cast<Search.Task>().FirstOrDefault();

        GetTaskData(objectiveSearchData);

        GetTerrainElementData();
        GetElementData();
        GetObjectGraphicData();

        var list = (from taskData in taskDataList
                    join terrainElementData in terrainElementDataList on taskData.terrainElementId equals terrainElementData.id
                    join elementData in elementDataList on terrainElementData.elementId equals elementData.id
                    join objectGraphicData in objectGraphicDataList on elementData.objectGraphicId equals objectGraphicData.id
                    select new TaskDataElement()
                    {
                        id = taskData.id,
                        table = "Task",

                        Index = taskData.index,
                        Description = taskData.description,

                        objectGraphicIcon = objectGraphicData.icon

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetTaskData(Search.Task searchParameters)
    {
        taskDataList = new List<TaskData>();

        foreach(Fixtures.Task task in Fixtures.taskList)
        {
            if (searchParameters.objectiveId.Count > 0 && !searchParameters.objectiveId.Contains(task.objectiveId)) continue;
            if (searchParameters.terrainElementId.Count > 0 && !searchParameters.terrainElementId.Contains(task.terrainElementId)) continue;

            var taskData = new TaskData();

            taskData.id = task.id;
            taskData.index = task.index;

            taskData.objectiveId = task.objectiveId;
            taskData.terrainElementId = task.terrainElementId;
            taskData.description = task.description;

            taskDataList.Add(taskData);
        }
    }

    internal void GetTerrainElementData()
    {
        terrainElementDataList = dataManager.GetTerrainElementData(taskDataList.Select(x => x.terrainElementId).Distinct().ToList(), true);
    }

    internal void GetElementData()
    {
        elementDataList = dataManager.GetElementData(terrainElementDataList.Select(x => x.elementId).Distinct().ToList(), true);
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicDataList = dataManager.GetObjectGraphicData(elementDataList.Select(x => x.objectGraphicId).Distinct().ToList(), true);
    }

    internal class TaskData : GeneralData
    {
        public int objectiveId;
        public int terrainElementId;
        public string description;
    }
}
