﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TaskDataManager
{
    private TaskController taskController;
    private List<TaskData> taskDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.TerrainElementData> terrainElementDataList;
    private List<DataManager.ElementData> elementDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    private List<DataManager.RegionData> regionDataList;
    private List<DataManager.TerrainData> terrainDataList;
    private List<DataManager.TerrainTileData> terrainTileDataList;

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
        GetIconData();

        GetTerrainTileData();
        GetTerrainData();
        GetRegionData();
        
        var list = (from taskData           in taskDataList

                    join terrainElementData in terrainElementDataList   on taskData.terrainElementId    equals terrainElementData.id
                    join elementData        in elementDataList          on terrainElementData.elementId equals elementData.id
                    join objectGraphicData  in objectGraphicDataList    on elementData.objectGraphicId  equals objectGraphicData.id
                    join iconData           in iconDataList             on objectGraphicData.iconId     equals iconData.id

                    join leftJoin in (from terrainTileData  in terrainTileDataList
                                      join terrainData      in terrainDataList  on terrainTileData.terrainId    equals terrainData.id
                                      join regionData       in regionDataList   on terrainData.regionId         equals regionData.id
                                      select new { terrainTileData, terrainData, regionData }) on taskData.terrainTileId equals leftJoin.terrainTileData.id into terrainTileData

                    from region in terrainTileData.DefaultIfEmpty()
                    select new TaskDataElement()
                    {
                        dataType = Enums.DataType.Task,

                        id = taskData.id,
                        index = taskData.index,

                        TerrainElementId = taskData.terrainElementId,
                        TerrainTileId = taskData.terrainTileId,

                        Description = taskData.description,
                        
                        regionId    = region != null ? region.regionData.id     : 0,
                        regionName  = region != null ? region.regionData.name   : "",
                        objectGraphicIconPath = iconData.path

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetTaskData(Search.Task searchParameters)
    {
        taskDataList = new List<TaskData>();

        foreach(Fixtures.Task task in Fixtures.taskList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(task.id)) continue;
            if (searchParameters.objectiveId.Count > 0 && !searchParameters.objectiveId.Contains(task.objectiveId)) continue;
            if (searchParameters.terrainElementId.Count > 0 && !searchParameters.terrainElementId.Contains(task.terrainElementId)) continue;

            var taskData = new TaskData();

            taskData.id = task.id;
            taskData.index = task.index;

            taskData.objectiveId = task.objectiveId;
            taskData.terrainElementId = task.terrainElementId;
            taskData.terrainTileId = task.terrainTileId;
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

    internal void GetIconData()
    {
        iconDataList = dataManager.GetIconData(objectGraphicDataList.Select(x => x.iconId).Distinct().ToList(), true);
    }

    internal void GetTerrainTileData()
    {
        terrainTileDataList = dataManager.GetTerrainTileData(taskDataList.Select(x => x.terrainTileId).Distinct().ToList(), true);
    }

    internal void GetTerrainData()
    {
        terrainDataList = dataManager.GetTerrainData(terrainTileDataList.Select(x => x.terrainId).Distinct().ToList(), true);
    }

    internal void GetRegionData()
    {
        regionDataList = dataManager.GetRegionData(terrainDataList.Select(x => x.regionId).Distinct().ToList(), true);
    }

    internal class TaskData : GeneralData
    {
        public int objectiveId;
        public int terrainElementId;
        public int terrainTileId;
        public string description;
    }
}