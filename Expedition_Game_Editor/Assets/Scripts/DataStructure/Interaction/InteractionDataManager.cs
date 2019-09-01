using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class InteractionDataManager
{
    private InteractionController interactionController;
    private List<InteractionData> interactionDataList;

    private DataManager dataManager = new DataManager();

    private List<DataManager.SceneInteractableData> sceneInteractableDataList;
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

        GetSceneInteractableData();
        GetInteractableData();
        GetObjectGraphicData();
        GetIconData();

        GetRegionData();
        
        var list = (from interactionData    in interactionDataList

                    join sceneInteractableData  in sceneInteractableDataList    on interactionData.sceneInteractableId  equals sceneInteractableData.id
                    join interactableData       in interactableDataList         on sceneInteractableData.interactableId equals interactableData.id
                    join objectGraphicData      in objectGraphicDataList        on interactableData.objectGraphicId     equals objectGraphicData.id
                    join iconData               in iconDataList                 on objectGraphicData.iconId             equals iconData.id

                    join leftJoin in (from regionData in regionDataList
                                      select new { regionData }) on interactionData.regionId equals leftJoin.regionData.id into regionData

                    from region in regionData.DefaultIfEmpty()
                    select new InteractionDataElement()
                    {
                        dataType = Enums.DataType.Interaction,

                        id = interactionData.id,
                        index = interactionData.index,

                        SceneInteractableId = interactionData.sceneInteractableId,
                        RegionId = interactionData.regionId,

                        Description = interactionData.description,
                        
                        TerrainTileId = interactionData.terrainTileId,

                        PositionX = interactionData.positionX,
                        PositionY = interactionData.positionY,
                        PositionZ = interactionData.positionZ,

                        RotationX = interactionData.rotationX,
                        RotationY = interactionData.rotationY,
                        RotationZ = interactionData.rotationZ,

                        ScaleMultiplier = interactionData.scaleMultiplier,

                        Animation = interactionData.animation,

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
            if (searchParameters.sceneInteractableId.Count > 0 && !searchParameters.sceneInteractableId.Contains(interaction.sceneInteractableId)) continue;

            var interactionData = new InteractionData();

            interactionData.id = interaction.id;
            interactionData.index = interaction.index;

            interactionData.objectiveId = interaction.objectiveId;
            interactionData.sceneInteractableId = interaction.sceneInteractableId;
            interactionData.regionId = interaction.regionId;

            interactionData.description = interaction.description;

            interactionData.terrainTileId = interaction.terrainTileId;

            interactionData.positionX = interaction.positionX;
            interactionData.positionY = interaction.positionY;
            interactionData.positionZ = interaction.positionZ;

            interactionData.rotationX = interaction.rotationX;
            interactionData.rotationY = interaction.rotationY;
            interactionData.rotationZ = interaction.rotationZ;

            interactionData.scaleMultiplier = interaction.scaleMultiplier;

            interactionData.animation = interaction.animation;

            interactionDataList.Add(interactionData);
        }
    }

    internal void GetSceneInteractableData()
    {
        sceneInteractableDataList = dataManager.GetSceneInteractableData(interactionDataList.Select(x => x.sceneInteractableId).Distinct().ToList(), true);
    }

    internal void GetInteractableData()
    {
        interactableDataList = dataManager.GetInteractableData(sceneInteractableDataList.Select(x => x.interactableId).Distinct().ToList(), true);
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
        var searchParameters = new Search.Region();
        searchParameters.id = interactionDataList.Select(x => x.regionId).Distinct().ToList();

        regionDataList = dataManager.GetRegionData(searchParameters);
    }

    internal class InteractionData : GeneralData
    {
        public int objectiveId;
        public int sceneInteractableId;
        public int regionId;

        public string description;

        public int terrainTileId;

        public float positionX;
        public float positionY;
        public float positionZ;

        public int rotationX;
        public int rotationY;
        public int rotationZ;

        public float scaleMultiplier;

        public int animation;
    }
}
