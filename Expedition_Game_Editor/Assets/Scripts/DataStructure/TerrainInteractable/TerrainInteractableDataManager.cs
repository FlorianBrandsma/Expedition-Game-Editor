using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainInteractableDataManager
{
    private TerrainInteractableController terrainInteractableController;
    private List<TerrainInteractableData> terrainInteractableDataList = new List<TerrainInteractableData>();

    private DataManager dataManager = new DataManager();

    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    public void InitializeManager(TerrainInteractableController terrainInteractableController)
    {
        this.terrainInteractableController = terrainInteractableController;
    }

    public List<IDataElement> GetTerrainInteractableDataElements(IEnumerable searchParameters)
    {
        var terrainInteractableSearchData = searchParameters.Cast<Search.TerrainInteractable>().FirstOrDefault();

        switch (terrainInteractableSearchData.requestType)
        {
            case Search.TerrainInteractable.RequestType.Custom:

                GetCustomTerrainInteractableData(terrainInteractableSearchData);
                break;

            case Search.TerrainInteractable.RequestType.GetQuestAndObjectiveElements:

                GetQuestAndObjectiveTerrainInteractableData(terrainInteractableSearchData);
                break;

            case Search.TerrainInteractable.RequestType.GetElementsFromTaskRegion:

                GetElementsFromTaskRegion(terrainInteractableSearchData);
                break;
        }

        if (terrainInteractableDataList.Count == 0) return new List<IDataElement>();

        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        var list = (from terrainInteractableData in terrainInteractableDataList
                    join interactableData in interactableDataList on terrainInteractableData.interactableId equals interactableData.id
                    join objectGraphicData in objectGraphicDataList on interactableData.objectGraphicId equals objectGraphicData.id
                    join iconData in iconDataList on objectGraphicData.iconId equals iconData.id
                    select new TerrainInteractableDataElement()
                    {
                        dataType = Enums.DataType.TerrainInteractable,

                        id = terrainInteractableData.id,
                        
                        ChapterId = terrainInteractableData.chapterId,
                        InteractableId = terrainInteractableData.interactableId,

                        interactableName = interactableData.name,
                        objectGraphicIconPath = iconData.path

                    }).OrderBy(x => x.id).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    internal void GetCustomTerrainInteractableData(Search.TerrainInteractable searchParameters)
    {
        terrainInteractableDataList = new List<TerrainInteractableData>();

        foreach (Fixtures.TerrainInteractable terrainInteractable in Fixtures.terrainInteractableList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(terrainInteractable.id)) continue;
            if (searchParameters.chapterId.Count > 0 && !searchParameters.chapterId.Contains(terrainInteractable.chapterId)) continue;
            if (searchParameters.objectiveId.Count > 0 && !searchParameters.objectiveId.Contains(terrainInteractable.objectiveId)) continue;

            var terrainInteractableData = new TerrainInteractableData();

            terrainInteractableData.id = terrainInteractable.id;

            terrainInteractableData.chapterId = terrainInteractable.chapterId;
            terrainInteractableData.objectiveId = terrainInteractable.objectiveId;
            terrainInteractableData.interactableId = terrainInteractable.interactableId;

            terrainInteractableDataList.Add(terrainInteractableData);
        }
    }

    internal void GetQuestAndObjectiveTerrainInteractableData(Search.TerrainInteractable searchParameters)
    {
        terrainInteractableDataList = new List<TerrainInteractableData>();
        
        List<int> terrainInteractableIds = new List<int>();

        Fixtures.phaseInteractableList.Where(x => searchParameters.questId.Contains(x.questId)).Distinct().ToList().ForEach(x => terrainInteractableIds.Add(x.terrainInteractableId));
        Fixtures.terrainInteractableList.Where(x => searchParameters.objectiveId.Contains(x.objectiveId)).Distinct().ToList().ForEach(x => terrainInteractableIds.Add(x.id));

        var terrainInteractables = Fixtures.terrainInteractableList.Where(x => terrainInteractableIds.Contains(x.id)).Distinct().ToList();

        foreach (Fixtures.TerrainInteractable terrainInteractable in terrainInteractables)
        {
            var terrainInteractableData = new TerrainInteractableData();

            terrainInteractableData.id = terrainInteractable.id;

            terrainInteractableData.chapterId = terrainInteractable.chapterId;
            terrainInteractableData.objectiveId = terrainInteractable.objectiveId;
            terrainInteractableData.interactableId = terrainInteractable.interactableId;

            terrainInteractableDataList.Add(terrainInteractableData);
        }
    }

    internal void GetElementsFromTaskRegion(Search.TerrainInteractable searchParameters)
    {
        terrainInteractableDataList = new List<TerrainInteractableData>();

        List<int> terrainInteractableIds = Fixtures.interactionList.Where(x => searchParameters.regionId.Contains(x.regionId)).Select(x => x.terrainInteractableId).Distinct().ToList();

        var terrainInteractables = Fixtures.terrainInteractableList.Where(x => terrainInteractableIds.Contains(x.id)).Distinct().ToList();

        foreach(Fixtures.TerrainInteractable terrainInteractable in terrainInteractables)
        {
            var terrainInteractableData = new TerrainInteractableData();

            terrainInteractableData.id = terrainInteractable.id;

            terrainInteractableData.chapterId = terrainInteractable.chapterId;
            terrainInteractableData.objectiveId = terrainInteractable.objectiveId;
            terrainInteractableData.interactableId = terrainInteractable.interactableId;

            terrainInteractableDataList.Add(terrainInteractableData);
        }
    }

    internal void GetInteractableData()
    {
        interactableDataList = dataManager.GetInteractableData(terrainInteractableDataList.Select(x => x.interactableId).Distinct().ToList(), true);
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicDataList = dataManager.GetObjectGraphicData(interactableDataList.Select(x => x.objectGraphicId).Distinct().ToList(), true);
    }

    internal void GetIconData()
    {
        iconDataList = dataManager.GetIconData(objectGraphicDataList.Select(x => x.iconId).Distinct().ToList(), true);
    }

    internal class TerrainInteractableData : GeneralData
    {
        public int chapterId;
        public int objectiveId;
        public int interactableId;
        public int taskIndex;
    }
}
