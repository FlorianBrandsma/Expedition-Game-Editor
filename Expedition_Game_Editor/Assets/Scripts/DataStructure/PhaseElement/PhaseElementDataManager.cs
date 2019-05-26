using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PhaseElementDataManager
{
    private PhaseElementController phaseElementController;
    private List<PhaseElementData> phaseElementDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.TerrainElementData> terrainElementDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.ElementData> elementDataList;

    public void InitializeManager(PhaseElementController phaseElementController)
    {
        this.phaseElementController = phaseElementController;
    }

    public List<PhaseElementDataElement> GetQuestElementDataElements(IEnumerable searchParameters)
    {
        var phaseElementSearchData = searchParameters.Cast<Search.PhaseElement>().FirstOrDefault();

        GetPhaseElementData(phaseElementSearchData);

        GetTerrainElementData();
        GetElementData();
        GetObjectGraphicData();

        var list = (from phaseElementData in phaseElementDataList
                    join terrainElementData in terrainElementDataList on phaseElementData.terrainElementId equals terrainElementData.id
                    join elementData in elementDataList on terrainElementData.elementId equals elementData.id
                    join objectGraphicData in objectGraphicDataList on elementData.objectGraphicId equals objectGraphicData.id
                    select new PhaseElementDataElement()
                    {
                        id = phaseElementData.id,
                        table = phaseElementData.table,

                        Index = phaseElementData.index,
                        PhaseId = phaseElementData.phaseId,
                        QuestId = phaseElementData.questId,
                        TerrainElementId = phaseElementData.terrainElementId,

                        objectGraphicIcon = objectGraphicData.icon

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list;
    }

    public void GetPhaseElementData(Search.PhaseElement searchParameters)
    {
        phaseElementDataList = new List<PhaseElementData>();

        foreach(Fixtures.PhaseElement phaseElement in Fixtures.phaseElementList)
        {
            var phaseElementData = new PhaseElementData();

            phaseElementData.id = phaseElement.id;
            phaseElementData.table = "PhaseElement";
            phaseElementData.index = phaseElement.index;

            if (searchParameters.phaseId.Count > 0 && !searchParameters.phaseId.Contains(phaseElement.phaseId)) continue;

            phaseElementData.phaseId = phaseElement.phaseId;
            phaseElementData.questId = phaseElement.questId;
            phaseElementData.terrainElementId = phaseElement.terrainElementId;

            phaseElementDataList.Add(phaseElementData);
        }
    }

    internal void GetTerrainElementData()
    {
        terrainElementDataList = dataManager.GetTerrainElementData(phaseElementDataList.Select(x => x.terrainElementId).Distinct().ToList(), true);
    }

    internal void GetElementData()
    {
        elementDataList = dataManager.GetElementData(terrainElementDataList.Select(x => x.elementId).Distinct().ToList(), true);
    }

    internal void GetObjectGraphicData()
    {
        objectGraphicDataList = dataManager.GetObjectGraphicData(elementDataList.Select(x => x.objectGraphicId).Distinct().ToList(), true);
    }

    internal class PhaseElementData : GeneralData
    {
        public int questId;
        public int phaseId;
        public int terrainElementId;
    }
}
