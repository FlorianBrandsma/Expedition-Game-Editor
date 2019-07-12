using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InteractionDataManager
{
    private InteractionController interactionController;
    private List<InteractionData> interactionDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.TerrainInteractableData> terrainInteractableDataList;
    private List<DataManager.InteractableData> interactableDataList;
    private List<DataManager.ObjectGraphicData> objectGraphicDataList;
    private List<DataManager.IconData> iconDataList;

    private List<DataManager.RegionData> regionDataList;

    public void InitializeManager(InteractionController interactionController)
    {
        this.interactionController = interactionController;
    }

    public List<IDataElement> GetInteractionDataElements(IEnumerable searchParameters)
    {
        var objectiveSearchData = searchParameters.Cast<Search.Interaction>().FirstOrDefault();

        GetInteractionData(objectiveSearchData);

        GetTerrainInteractableData();
        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        GetRegionData();
        
        var list = (from interactionData    in interactionDataList

                    join terrainInteractableData    in terrainInteractableDataList  on interactionData.terrainInteractableId    equals terrainInteractableData.id
                    join interactableData           in interactableDataList         on terrainInteractableData.interactableId   equals interactableData.id
                    join objectGraphicData          in objectGraphicDataList        on interactableData.objectGraphicId         equals objectGraphicData.id
                    join iconData                   in iconDataList                 on objectGraphicData.iconId                 equals iconData.id

                    join leftJoin in (from regionData in regionDataList
                                      select new { regionData }) on interactionData.regionId equals leftJoin.regionData.id into regionData

                    from region in regionData.DefaultIfEmpty()
                    select new InteractionDataElement()
                    {
                        dataType = Enums.DataType.Interaction,

                        id = interactionData.id,
                        index = interactionData.index,

                        TerrainInteractableId = interactionData.terrainInteractableId,
                        RegionId = interactionData.regionId,

                        Description = interactionData.description,
                        
                        regionName  = region != null ? region.regionData.name   : "",
                        objectGraphicIconPath = iconData.path

                    }).OrderBy(x => x.Index).ToList();

        list.ForEach(x => x.SetOriginalValues());

        return list.Cast<IDataElement>().ToList();
    }

    public void GetInteractionData(Search.Interaction searchParameters)
    {
        interactionDataList = new List<InteractionData>();

        foreach(Fixtures.Interaction interaction in Fixtures.interactionList)
        {
            if (searchParameters.id.Count > 0 && !searchParameters.id.Contains(interaction.id)) continue;
            if (searchParameters.objectiveId.Count > 0 && !searchParameters.objectiveId.Contains(interaction.objectiveId)) continue;
            if (searchParameters.terrainInteractableId.Count > 0 && !searchParameters.terrainInteractableId.Contains(interaction.terrainInteractableId)) continue;

            var interactionData = new InteractionData();

            interactionData.id = interaction.id;
            interactionData.index = interaction.index;

            interactionData.objectiveId = interaction.objectiveId;
            interactionData.terrainInteractableId = interaction.terrainInteractableId;
            interactionData.regionId = interaction.regionId;
            interactionData.description = interaction.description;

            interactionDataList.Add(interactionData);
        }
    }

    internal void GetTerrainInteractableData()
    {
        terrainInteractableDataList = dataManager.GetTerrainInteractableData(interactionDataList.Select(x => x.terrainInteractableId).Distinct().ToList(), true);
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

    internal void GetRegionData()
    {
        regionDataList = dataManager.GetRegionData(interactionDataList.Select(x => x.regionId).Distinct().ToList(), true);
    }

    internal class InteractionData : GeneralData
    {
        public int objectiveId;
        public int terrainInteractableId;
        public int regionId;
        public string description;
    }
}
