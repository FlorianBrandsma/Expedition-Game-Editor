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
    private List<DataManager.ElementData> elementDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    public void InitializeManager(PhaseElementController phaseElementController)
    {
        this.phaseElementController = phaseElementController;
    }

    public List<IDataElement> GetQuestElementDataElements(IEnumerable searchParameters)
    {
        var phaseElementSearchData = searchParameters.Cast<Search.PhaseElement>().FirstOrDefault();

        GetPhaseElementData(phaseElementSearchData);

        GetTerrainElementData();
        GetElementData();
        GetObjectGraphicData();
        GetIconData();

        var list = (from phaseElementData in phaseElementDataList
                    join terrainElementData in terrainElementDataList on phaseElementData.terrainElementId equals terrainElementData.id
                    join elementData in elementDataList on terrainElementData.elementId equals elementData.id
                    join objectGraphicData in objectGraphicDataList on elementData.objectGraphicId equals objectGraphicData.id
                    join iconData in iconDataList on objectGraphicData.iconId equals iconData.id
                    select new PhaseElementDataElement()
                    {
                        id = phaseElementData.id,
                        table = phaseElementData.table,

                        Index = phaseElementData.index,
                        PhaseId = phaseElementData.phaseId,
                        QuestId = phaseElementData.questId,
                        TerrainElementId = phaseElementData.terrainElementId,

                        elementStatus = GetElementStatus(phaseElementData),
                        elementName = elementData.name,
                        objectGraphicIcon = iconData.path

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
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

    internal void GetIconData()
    {
        iconDataList = dataManager.GetIconData(objectGraphicDataList.Select(x => x.iconId).Distinct().ToList(), true);
    }

    Enums.ElementStatus GetElementStatus(PhaseElementData phaseData)
    {
        var questData = (QuestDataElement)phaseElementController.SegmentController.editorController.pathController.route.data.DataElement;

        if (phaseData.questId == questData.id)
            return Enums.ElementStatus.Enabled;
        else if (phaseData.questId == 0)
            return Enums.ElementStatus.Disabled;
        else
            return Enums.ElementStatus.Locked;
    }

    internal class PhaseElementData : GeneralData
    {
        public int questId;
        public int phaseId;
        public int terrainElementId;
    }
}
