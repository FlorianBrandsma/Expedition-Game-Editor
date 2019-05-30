using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TerrainElementDataManager
{
    private TerrainElementController terrainElementController;
    private List<TerrainElementData> terrainElementDataList = new List<TerrainElementData>();

    private DataManager dataManager = new DataManager();

    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.ElementData> elementDataList;

    public void InitializeManager(TerrainElementController terrainElementController)
    {
        this.terrainElementController = terrainElementController;
    }

    public List<IDataElement> GetTerrainElementDataElements(IEnumerable searchParameters)
    {
        var terrainElementSearchData = searchParameters.Cast<Search.TerrainElement>().FirstOrDefault();

        switch (terrainElementSearchData.requestType)
        {
            case Search.TerrainElement.RequestType.Custom:

                GetCustomTerrainElementData(terrainElementSearchData);
                break;

            case Search.TerrainElement.RequestType.GetQuestAndObjectiveElements:

                GetQuestAndObjectiveTerrainElementData(terrainElementSearchData);
                break;
        }

        if (terrainElementDataList.Count == 0) return new List<IDataElement>();

        GetElementData();
        GetObjectGraphicData();

        var list = (from terrainElementData in terrainElementDataList
                    join elementData in elementDataList on terrainElementData.elementId equals elementData.id
                    join objectGraphicData in objectGraphicDataList on elementData.objectGraphicId equals objectGraphicData.id
                    select new TerrainElementDataElement()
                    {
                        id = terrainElementData.id,
                        table = "TerrainElement",

                        ChapterId = terrainElementData.chapterId,
                        ElementId = terrainElementData.elementId,

                        elementName = elementData.name,
                        objectGraphicIcon = objectGraphicData.icon

                    }).OrderBy(x => x.id).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    internal void GetCustomTerrainElementData(Search.TerrainElement searchParameters)
    {
        terrainElementDataList = new List<TerrainElementData>();

        foreach (Fixtures.TerrainElement terrainElement in Fixtures.terrainElementList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(terrainElement.id)) continue;
            if (searchParameters.chapterId.Count > 0 && !searchParameters.chapterId.Contains(terrainElement.chapterId)) continue;
            if (searchParameters.objectiveId.Count > 0 && !searchParameters.objectiveId.Contains(terrainElement.objectiveId)) continue;

            var terrainElementData = new TerrainElementData();

            terrainElementData.id = terrainElement.id;

            terrainElementData.chapterId = terrainElement.chapterId;
            terrainElementData.objectiveId = terrainElement.objectiveId;
            terrainElementData.elementId = terrainElement.elementId;

            terrainElementDataList.Add(terrainElementData);
        }
    }

    internal void GetQuestAndObjectiveTerrainElementData(Search.TerrainElement searchParameters)
    {
        terrainElementDataList = new List<TerrainElementData>();

        var objectiveData = (ObjectiveDataElement)terrainElementController.SegmentController.path.FindLastRoute("Objective").data.DataElement;
        var questData = (QuestDataElement)terrainElementController.SegmentController.path.FindLastRoute("Quest").data.DataElement;

        List<int> terrainElementIds = new List<int>();

        Fixtures.phaseElementList.Where(x => x.questId == questData.id).Distinct().ToList().ForEach(x => terrainElementIds.Add(x.terrainElementId));
        Fixtures.terrainElementList.Where(x => x.objectiveId == objectiveData.id).Distinct().ToList().ForEach(x => terrainElementIds.Add(x.id));

        foreach(Fixtures.TerrainElement terrainElement in Fixtures.terrainElementList.Where(x => terrainElementIds.Contains(x.id)).Distinct().ToList())
        {
            var terrainElementData = new TerrainElementData();

            terrainElementData.id = terrainElement.id;
            terrainElementData.table = "TerrainElement";

            terrainElementData.chapterId = terrainElement.chapterId;
            terrainElementData.objectiveId = terrainElement.objectiveId;
            terrainElementData.elementId = terrainElement.elementId;

            terrainElementDataList.Add(terrainElementData);
        }
    }

    internal void GetElementData()
    {
        elementDataList = dataManager.GetElementData(terrainElementDataList.Select(x => x.elementId).Distinct().ToList(), true);
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicDataList = dataManager.GetObjectGraphicData(elementDataList.Select(x => x.objectGraphicId).Distinct().ToList(), true);
    }

    internal class TerrainElementData : GeneralData
    {
        public int chapterId;
        public int objectiveId;
        public int elementId;
        public int taskIndex;
    }
}
